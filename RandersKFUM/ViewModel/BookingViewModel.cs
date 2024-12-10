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


    private FieldStatus selectedField;
    public FieldStatus SelectedField
    {
        get => selectedField;
        set
        {
            selectedField = value;
            OnPropertyChanged(); // Dette sikrer, at UI bliver opdateret, når værdien ændres
        }
    }

    private LockerRoomStatus selectedLockerRoom;
    public LockerRoomStatus SelectedLockerRoom
    {
        get => selectedLockerRoom;
        set
        {
            selectedLockerRoom = value;
            OnPropertyChanged(); // Sikrer, at UI bliver opdateret
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
            // Log fejlen og/eller giv brugeren besked
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
            // Log eller vis fejlen
            MessageBox.Show("Kunne ikke opdatere tilgængeligheden: " + ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }



    public virtual void ConfirmBooking()
    {
        if (!SelectedFields.Any() || !SelectedLockerRooms.Any() || SelectedTeam == null)
        {
            MessageBox.Show("Vælg venligst mindst én bane, ét omklædningsrum og et hold.", "Validering Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var start = SelectedDate.Date + SelectedTimeSlot;
        var end = start.AddMinutes(SelectedDuration);

        var booking = new Booking
        {
            DateTimeStart = start,
            DateTimeEnd = end,
            TeamId = SelectedTeam.TeamId
        };

        // Saml FieldIds og LockerRoomIds
        var fieldIds = SelectedFields.Select(f => f.FieldId);
        var lockerRoomIds = SelectedLockerRooms.Select(lr => lr.LockerRoomId);
        try
        {


            bookingRepository.Add(booking, fieldIds, lockerRoomIds);
            MessageBox.Show("Booking oprettet!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBackToMainMenu();
        }
        catch (Exception ex)
        {
            // Håndter uforudsete fejl
            MessageBox.Show($"Der opstod en fejl: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void NavigateBackToMainMenu()
    {
        RandersKFUM.Utilities.NavigationService.NavigateTo(new MainMenuView());
    }
}
