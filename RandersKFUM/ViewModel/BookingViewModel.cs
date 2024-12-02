using RandersKFUM.Model;
using RandersKFUM.Repository;
using RandersKFUM.Utilities;
using System.Collections.ObjectModel;

public class BookingViewModel : ViewModelBase
{
    private readonly FieldRepository _fieldRepository;
    private readonly LockerRoomRepository _lockerRoomRepository;
    private readonly BookingRepository _bookingRepository;

    public ObservableCollection<Field> Fields { get; set; } = new ObservableCollection<Field>();
    public ObservableCollection<LockerRoom> LockerRooms { get; set; } = new ObservableCollection<LockerRoom>();
    public ObservableCollection<TimeSpan> TimeSlots { get; set; } = new ObservableCollection<TimeSpan>();
    public ObservableCollection<int> Durations { get; set; } = new ObservableCollection<int> { 30, 60, 90, 120 };

    public RelayCommand UpdateAvailabilityCommand { get; }
    public RelayCommand<Field> SelectFieldCommand { get; }
    public RelayCommand ConfirmBookingCommand { get; }
    public RelayCommand NavigateBackCommand { get; }

    private Field _selectedField;
    public Field SelectedField
    {
        get => _selectedField;
        set
        {
            _selectedField = value;
            OnPropertyChanged(); // Dette sikrer, at UI bliver opdateret, når værdien ændres
        }
    }

    private LockerRoom _selectedLockerRoom;
    public LockerRoom SelectedLockerRoom
    {
        get => _selectedLockerRoom;
        set
        {
            _selectedLockerRoom = value;
            OnPropertyChanged(); // Sikrer, at UI bliver opdateret
        }
    }


    private DateTime _selectedDate;
    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged();
            UpdateAvailability();
        }
    }

    private TimeSpan _selectedTimeSlot;
    public TimeSpan SelectedTimeSlot
    {
        get => _selectedTimeSlot;
        set
        {
            _selectedTimeSlot = value;
            OnPropertyChanged();
            UpdateAvailability();
        }
    }

    private int _selectedDuration;
    public int SelectedDuration
    {
        get => _selectedDuration;
        set
        {
            _selectedDuration = value;
            OnPropertyChanged();
            UpdateAvailability();
        }
    }

    public BookingViewModel(FieldRepository fieldRepository, LockerRoomRepository lockerRoomRepository, BookingRepository bookingRepository)
    {
        _fieldRepository = fieldRepository;
        _lockerRoomRepository = lockerRoomRepository;
        _bookingRepository = bookingRepository;

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
        var allFields = _fieldRepository.GetAll();
        foreach (var field in allFields)
        {
            Fields.Add(field);
        }

        LockerRooms.Clear();
        var allLockerRooms = _lockerRoomRepository.GetAll();
        foreach (var lockerRoom in allLockerRooms)
        {
            LockerRooms.Add(lockerRoom);
        }
    }

    private void InitializeTimeSlots()
    {
        for (int hour = 8; hour <= 22; hour++)
        {
            TimeSlots.Add(new TimeSpan(hour, 0, 0));
            TimeSlots.Add(new TimeSpan(hour, 30, 0));
        }
    }

    private void UpdateAvailability()
    {
        if (SelectedDate == default || SelectedTimeSlot == default || SelectedDuration == 0)
        {
            return;
        }

        var start = SelectedDate.Date + SelectedTimeSlot;
        var end = start.AddMinutes(SelectedDuration);

        var availableFields = _fieldRepository.GetAvailableFields(start, end).Select(f => f.FieldId).ToHashSet();
        var availableLockerRooms = _lockerRoomRepository.GetAvailableLockerRooms(start, end).Select(lr => lr.LockerRoomId).ToHashSet();

        foreach (var field in Fields)
        {
            var isAvailable = availableFields.Contains(field.FieldId);
            OnPropertyChanged(nameof(Fields));
        }

        foreach (var lockerRoom in LockerRooms)
        {
            var isAvailable = availableLockerRooms.Contains(lockerRoom.LockerRoomId);
            OnPropertyChanged(nameof(LockerRooms));
        }
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

        _bookingRepository.Add(booking);
    }

    private void NavigateBack()
    {
        // Logik til at navigere tilbage
    }
}
