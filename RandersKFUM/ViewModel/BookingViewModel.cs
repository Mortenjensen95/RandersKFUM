using RandersKFUM.Hjælpeklasser;
using RandersKFUM.Model;
using RandersKFUM.Repository;
using RandersKFUM.Utilities;
using System.Collections.ObjectModel;

public class BookingViewModel : ViewModelBase
{
    private readonly FieldRepository fieldRepository;
    private readonly LockerRoomRepository lockerRoomRepository;
    private readonly BookingRepository bookingRepository;

    public ObservableCollection<FieldStatus> FieldAvailability { get; set; } = new ObservableCollection<FieldStatus>();
    public ObservableCollection<LockerRoomStatus> LockerRoomAvailability { get; set; } = new ObservableCollection<LockerRoomStatus>();

    public ObservableCollection<Field> Fields { get; set; } = new ObservableCollection<Field>();
    public ObservableCollection<LockerRoom> LockerRooms { get; set; } = new ObservableCollection<LockerRoom>();
    public ObservableCollection<TimeSpan> TimeSlots { get; set; } = new ObservableCollection<TimeSpan>();
    public ObservableCollection<int> Durations { get; set; } = new ObservableCollection<int> { 30, 60, 90, 120 };

    public RelayCommand UpdateAvailabilityCommand { get; }
    public RelayCommand<Field> SelectFieldCommand { get; }
    public RelayCommand ConfirmBookingCommand { get; }
    public RelayCommand NavigateBackCommand { get; }

    private Field selectedField;
    public Field SelectedField
    {
        get => selectedField;
        set
        {
            selectedField = value;
            OnPropertyChanged(); // Dette sikrer, at UI bliver opdateret, når værdien ændres
        }
    }

    private LockerRoom selectedLockerRoom;
    public LockerRoom SelectedLockerRoom
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

    public BookingViewModel(FieldRepository fieldRepository, LockerRoomRepository lockerRoomRepository, BookingRepository bookingRepository)
    {
        this.fieldRepository = fieldRepository;
        this.lockerRoomRepository = lockerRoomRepository;
        this.bookingRepository = bookingRepository;

        UpdateAvailabilityCommand = new RelayCommand(_ => UpdateAvailability());
        SelectFieldCommand = new RelayCommand<Field>(field => SelectField(field));
        ConfirmBookingCommand = new RelayCommand(_ => ConfirmBooking());
        NavigateBackCommand = new RelayCommand(_ => NavigateBack());

        LoadAllResources();
        InitializeTimeSlots();
    }

    private void LoadAllResources()
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
        if (SelectedDate == default || SelectedTimeSlot == default || SelectedDuration == 0)
            return;

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
                IsAvailable = availableFields.Contains(field.FieldId)
            });
        }

        LockerRoomAvailability.Clear();
        foreach (var lockerRoom in allLockerRooms)
        {
            LockerRoomAvailability.Add(new LockerRoomStatus
            {
                LockerRoomId = lockerRoom.LockerRoomId,
                IsAvailable = availableLockerRooms.Contains(lockerRoom.LockerRoomId)
            });
        }

        OnPropertyChanged(nameof(FieldAvailability));
        OnPropertyChanged(nameof(LockerRoomAvailability));
    }






    private void SelectField(Field field)
    {
        SelectedField = field;
    }

    private void ConfirmBooking()
    {
        if (SelectedField == null || SelectedLockerRoom == null)
        {
            return;
        }

        var start = SelectedDate.Date + SelectedTimeSlot;
        var end = start.AddMinutes(SelectedDuration);

        var booking = new Booking
        {
            FieldId = SelectedField.FieldId,
            LockerRoomId = SelectedLockerRoom.LockerRoomId,
            DateTimeStart = start,
            DateTimeEnd = end
        };

        bookingRepository.Add(booking);
    }

    private void NavigateBack()
    {
        // Logik til at navigere tilbage
    }
}
