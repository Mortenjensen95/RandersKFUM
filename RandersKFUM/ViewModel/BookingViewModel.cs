using System;
using System.Collections.ObjectModel;
using System.Linq;
using RandersKFUM.Model;
using RandersKFUM.Repository;
using RandersKFUM.Utilities;

namespace RandersKFUM.ViewModel
{
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

        private Field _selectedField;
        public Field SelectedField
        {
            get => _selectedField;
            set
            {
                _selectedField = value;
                OnPropertyChanged();
            }
        }

        private LockerRoom _selectedLockerRoom;
        public LockerRoom SelectedLockerRoom
        {
            get => _selectedLockerRoom;
            set
            {
                _selectedLockerRoom = value;
                OnPropertyChanged();
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
            foreach (var field in _fieldRepository.GetAll())
            {
                field.IsAvailable = true; // Som standard grøn
                Fields.Add(field);
            }

            LockerRooms.Clear();
            foreach (var lockerRoom in _lockerRoomRepository.GetAll())
            {
                lockerRoom.IsAvailable = true; // Som standard grøn
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
            var start = SelectedDate.Date + SelectedTimeSlot;
            var end = start.AddMinutes(SelectedDuration);

            // Find de optagne baner og omklædningsrum
            var unavailableFields = _fieldRepository.GetUnavailableFields(start, end).Select(f => f.FieldId).ToHashSet();
            var unavailableLockerRooms = _lockerRoomRepository.GetUnavailableLockerRooms(start, end).Select(lr => lr.LockerRoomId).ToHashSet();

            // Opdater tilgængelighed for baner
            foreach (var field in Fields)
            {
                field.IsAvailable = !unavailableFields.Contains(field.FieldId); // Rød, hvis optaget
            }

            // Opdater tilgængelighed for omklædningsrum
            foreach (var lockerRoom in LockerRooms)
            {
                lockerRoom.IsAvailable = !unavailableLockerRooms.Contains(lockerRoom.LockerRoomId); // Rød, hvis optaget
            }

            // Fortæl UI'et, at listerne er opdateret
            OnPropertyChanged(nameof(Fields));
            OnPropertyChanged(nameof(LockerRooms));
        }


        private void SelectField(Field field)
        {
            SelectedField = field;
        }

        private void ConfirmBooking()
        {
            if (SelectedField == null || SelectedLockerRoom == null)
            {
                // Hvis ingen bane eller omklædningsrum er valgt
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
}
