using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using RandersKFUM.Repository;
using RandersKFUM.Model;
using RandersKFUM.Utilities;
using RandersKFUM.View;



namespace RandersKFUM.ViewModels
{
    public class BookingViewModel : INotifyPropertyChanged
    {
        private readonly BookingRepository _bookingRepository;
        private readonly FieldRepository _fieldRepository;
        private readonly LockerRoomRepository _lockerRoomRepository;
        private readonly Action _navigateBackAction;

        private DateTime _selectedDate;
        private TimeSpan? _selectedTimeSlot;
        private int? _selectedDuration;
        private string? _selectedLockerRoom;
        private int? _selectedField;

        public BookingViewModel(
            BookingRepository bookingRepository,
            FieldRepository fieldRepository,
            LockerRoomRepository lockerRoomRepository,
            Action navigateBackAction)
        {
            _bookingRepository = bookingRepository;
            _fieldRepository = fieldRepository;
            _lockerRoomRepository = lockerRoomRepository;
            _navigateBackAction = navigateBackAction;

            // Standardværdier
            SelectedDate = DateTime.Today;
            TimeSlots = GenerateTimeSlots();
            Durations = new ObservableCollection<int> { 30, 60, 90, 120 };
            LockerRooms = new ObservableCollection<string>(
                _lockerRoomRepository.GetAll().Select(l => l.Name)
            );
            Fields = new ObservableCollection<string>(
                _fieldRepository.GetAll().Select(f => f.Name)
            );

            // Kommandoer
            SelectFieldCommand = new RelayCommand(SelectField);
            ConfirmBookingCommand = new RelayCommand(ConfirmBooking, CanConfirmBooking);
            NavigateBackCommand = new RelayCommand(NavigateBack);
        }

        // Egenskaber
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                RefreshCanExecute();
            }
        }

        public ObservableCollection<TimeSpan> TimeSlots { get; }
        public ObservableCollection<int> Durations { get; }
        public ObservableCollection<string> LockerRooms { get; }
        public ObservableCollection<string> Fields { get; }

        public TimeSpan? SelectedTimeSlot
        {
            get => _selectedTimeSlot;
            set
            {
                _selectedTimeSlot = value;
                OnPropertyChanged(nameof(SelectedTimeSlot));
                RefreshCanExecute();
            }
        }

        public int? SelectedDuration
        {
            get => _selectedDuration;
            set
            {
                _selectedDuration = value;
                OnPropertyChanged(nameof(SelectedDuration));
                RefreshCanExecute();
            }
        }

        public string? SelectedLockerRoom
        {
            get => _selectedLockerRoom;
            set
            {
                _selectedLockerRoom = value;
                OnPropertyChanged(nameof(SelectedLockerRoom));
                RefreshCanExecute();
            }
        }

        public int? SelectedField
        {
            get => _selectedField;
            private set
            {
                _selectedField = value;
                OnPropertyChanged(nameof(SelectedField));
                RefreshCanExecute();
            }
        }

        public ICommand SelectFieldCommand { get; }
        public ICommand ConfirmBookingCommand { get; }
        public ICommand NavigateBackCommand { get; }

        // Metoder
        private ObservableCollection<TimeSpan> GenerateTimeSlots()
        {
            var slots = new ObservableCollection<TimeSpan>();
            for (var time = TimeSpan.FromHours(15); time <= TimeSpan.FromHours(22); time += TimeSpan.FromMinutes(30))
            {
                slots.Add(time);
            }
            return slots;
        }

        private void SelectField(object parameter)
        {
            if (int.TryParse(parameter.ToString(), out var fieldNumber))
            {
                SelectedField = fieldNumber;
            }
        }

        private void ConfirmBooking()
        {
            if (!SelectedTimeSlot.HasValue || !SelectedDuration.HasValue || SelectedField == null || SelectedLockerRoom == null)
                return;

            var startDateTime = SelectedDate.Date + SelectedTimeSlot.Value;
            var endDateTime = startDateTime.Add(TimeSpan.FromMinutes(SelectedDuration.Value));

            // Opret booking-objekt
            var booking = new Booking
            {
                FieldId = SelectedField.Value,
                LockerRoom = SelectedLockerRoom,
                DateTimeStart = startDateTime,
                DateTimeEnd = endDateTime
            };

            // Gem booking i databasen
            _bookingRepository.Add(booking);

            // Naviger tilbage til menuen efter bekræftelse
            NavigateBack();
        }

        private bool CanConfirmBooking()
        {
            return SelectedTimeSlot.HasValue &&
                   SelectedDuration.HasValue &&
                   SelectedField != null &&
                   SelectedLockerRoom != null;
        }

        private void RefreshCanExecute()
        {
            (ConfirmBookingCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void NavigateBack()
        {
            _navigateBackAction?.Invoke();
        }

        // INotifyPropertyChanged implementering
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
