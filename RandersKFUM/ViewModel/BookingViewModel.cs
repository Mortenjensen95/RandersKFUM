using RandersKFUM.Hjælpeklasser;
using RandersKFUM.Model;
using RandersKFUM.Repository;
using RandersKFUM.Utilities;
using RandersKFUM.View;
using System.Collections.ObjectModel;
using System.Windows;

public class BookingViewModel : ViewModelBase
{
    private readonly FieldRepository fieldRepository;
    private readonly LockerRoomRepository lockerRoomRepository;
    protected readonly BookingRepository bookingRepository;
    private readonly TeamRepository teamRepository;

    public ObservableCollection<FieldStatus> FieldAvailability { get; set; } = new ObservableCollection<FieldStatus>();
    public ObservableCollection<LockerRoomStatus> LockerRoomAvailability { get; set; } = new ObservableCollection<LockerRoomStatus>();

    public ObservableCollection<Team> Teams { get; set; } = new ObservableCollection<Team> { };
    public ObservableCollection<Field> Fields { get; set; } = new ObservableCollection<Field>();
    public ObservableCollection<LockerRoom> LockerRooms { get; set; } = new ObservableCollection<LockerRoom>();
    public ObservableCollection<TimeSpan> TimeSlots { get; set; } = new ObservableCollection<TimeSpan>();
    public ObservableCollection<int> Durations { get; set; } = new ObservableCollection<int> { 30, 60, 90, 120 };

    public ObservableCollection<FieldStatus> SelectedFields { get; set; } = new ObservableCollection<FieldStatus>();
    public ObservableCollection<LockerRoomStatus> SelectedLockerRooms { get; set; } = new ObservableCollection<LockerRoomStatus>();


    public RelayCommand ConfirmBookingCommand { get; }
    public RelayCommand NavigateBackCommand { get; }


    public BookingViewModel()
    {
        fieldRepository = new FieldRepository(DatabaseConfig.GetConnectionString());
        lockerRoomRepository = new LockerRoomRepository(DatabaseConfig.GetConnectionString());
        bookingRepository = new BookingRepository(DatabaseConfig.GetConnectionString());
        teamRepository = new TeamRepository(DatabaseConfig.GetConnectionString());

        ConfirmBookingCommand = new RelayCommand(_ => ConfirmBooking());
        NavigateBackCommand = new RelayCommand(_ => NavigateBackToMainMenu());

        LoadAllResources();
        InitializeTimeSlots();

        SelectedDate = DateTime.Today;
    }

    private FieldStatus selectedField;
    public FieldStatus SelectedField
    {
        get => selectedField;
        set
        {
            selectedField = value;
            OnPropertyChanged();
        }
    }

    private LockerRoomStatus selectedLockerRoom;
    public LockerRoomStatus SelectedLockerRoom
    {
        get => selectedLockerRoom;
        set
        {
            selectedLockerRoom = value;
            OnPropertyChanged();
        }
    }

    private DateTime selectedDate;
    public DateTime SelectedDate
    {
        get => selectedDate;
        set
        {
            selectedDate = value;
            OnPropertyChanged();
            UpdateAvailability();
        }
    }

    private TimeSpan selectedTimeSlot;
    public TimeSpan SelectedTimeSlot
    {
        get => selectedTimeSlot;
        set
        {
            selectedTimeSlot = value;
            OnPropertyChanged();
            UpdateAvailability();
        }
    }

    private int selectedDuration;
    public int SelectedDuration
    {
        get => selectedDuration;
        set
        {
            selectedDuration = value;
            OnPropertyChanged();
            UpdateAvailability();
        }
    }

    private Team selectedTeam;
    public Team SelectedTeam
    {
        get => selectedTeam;
        set
        {
            selectedTeam = value;
            OnPropertyChanged();
            UpdateAvailability();
        }
    }

    private void LoadAllResources()
    {
        try
        {
            Fields.Clear();
            var allFields = fieldRepository.GetAll();
            foreach (var field in allFields)
            {
                Fields.Add(field);
            }

            LockerRooms.Clear();
            var allLockerRooms = lockerRoomRepository.GetAll();
            foreach (var lockerRoom in allLockerRooms)
            {
                LockerRooms.Add(lockerRoom);
            }

            Teams.Clear();
            var allTeams = teamRepository.GetAll();
            foreach (var team in allTeams)
            {
                Teams.Add(team);
            }
        }
        catch (Exception ex)
        {

            MessageBox.Show("Kunne ikke indlæse ressourcer: " + ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void InitializeTimeSlots()
    {
        for (int hour = 15; hour <= 22; hour++)
        {
            TimeSlots.Add(new TimeSpan(hour, 0, 0));
            TimeSlots.Add(new TimeSpan(hour, 30, 0));
        }
    }

    private void UpdateAvailability()
    {
        if (SelectedDate == default || SelectedTimeSlot == default || SelectedDuration == 0 || SelectedTeam == default)
            return;

        try
        {
            var start = SelectedDate.Date + SelectedTimeSlot;
            var end = start.AddMinutes(SelectedDuration);

            // Hent alle felter og omklædningsrum først
            var allFields = fieldRepository.GetAll();
            var allLockerRooms = lockerRoomRepository.GetAll();

            // Hent kun tilgængelige ressourcer
            var availableFields = fieldRepository.GetAvailableFields(start, end).Select(f => f.FieldId).ToList();
            var availableLockerRooms = lockerRoomRepository.GetAvailableLockerRooms(start, end).Select(lr => lr.LockerRoomId).ToList();

            // Opdater tilgængelighedsstatus for alle ressourcer
            FieldAvailability.Clear();
            foreach (var field in allFields)
            {
                FieldAvailability.Add(new FieldStatus
                {
                    FieldId = field.FieldId,
                    FieldNumber = field.FieldNumber,
                    FieldType = field.FieldType,
                    IsAvailable = availableFields.Contains(field.FieldId)
                });
            }

            LockerRoomAvailability.Clear();
            foreach (var lockerRoom in allLockerRooms)
            {
                LockerRoomAvailability.Add(new LockerRoomStatus
                {
                    LockerRoomId = lockerRoom.LockerRoomId,
                    LockerRoomNumber = lockerRoom.LockerRoomNumber,
                    LockerRoomType = lockerRoom.LockerRoomType,
                    IsAvailable = availableLockerRooms.Contains(lockerRoom.LockerRoomId)
                });
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Kunne ikke opdatere tilgængeligheden: " + ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }



    public void ConfirmBooking()
    {
        // Validerer om de nødvendige felter er valgt
        if (!SelectedFields.Any() || !SelectedLockerRooms.Any())
        {
            MessageBox.Show("Vælg venligst mindst én bane, ét omklædningsrum og et hold.", "Validering Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Beregner start og slut tidspunkter baseret på valgte dato og tidsrum
        var start = SelectedDate.Date + SelectedTimeSlot;
        var end = start.AddMinutes(SelectedDuration);

        // Opretter en ny booking-instans
        var booking = new Booking(
            bookingNumber: 0,
            dateTimeStart: start,
            dateTimeEnd: end,
            teamId: SelectedTeam.TeamId
        );

        // Henter ID'er for valgte baner og omklædningsrum
        // Henter ID'er for de valgte baner ved at anvende LINQ og et lambda-udtryk
        // 'SelectedFields' er en samling af valgte baner, og 'Select' anvendes her til at transformere denne samling til en ny samling, hvor hvert element er et FieldId
        var fieldIds = SelectedFields.Select(f => f.FieldId);

        // På samme måde henter vi ID'er for de valgte omklædningsrum ved brug af LINQ og et lambda-udtryk
        // 'SelectedLockerRooms' er en samling af valgte omklædningsrum, og 'Select' funktionen transformerer denne samling til en liste af LockerRoomId'er
        var lockerRoomIds = SelectedLockerRooms.Select(lr => lr.LockerRoomId);
        try
        {
            // Tilføjer bookingen til databasen via repository
            bookingRepository.Add(booking, fieldIds, lockerRoomIds);
            MessageBox.Show("Booking oprettet!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBackToMainMenu();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Der opstod en fejl: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }



    private void NavigateBackToMainMenu()
    {
        RandersKFUM.Utilities.NavigationService.NavigateTo(new MainMenuView());
    }
}
