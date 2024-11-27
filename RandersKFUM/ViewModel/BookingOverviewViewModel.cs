using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using RandersKFUM.Model;
using RandersKFUM.Repository;

namespace RandersKFUM.ViewModels
{
    public class BookingOverviewViewModel : INotifyPropertyChanged
    {
        private readonly BookingRepository _bookingRepository;
        private DateTime _selectedDate;
        private ObservableCollection<Booking> _bookings = new ObservableCollection<Booking>();
        private readonly Action _navigateBackAction; // Delegate til navigation

        public BookingOverviewViewModel(BookingRepository bookingRepository, Action navigateBackAction)
        {
            _bookingRepository = bookingRepository;
            _navigateBackAction = navigateBackAction;

            // Sæt standarddato til i dag
            SelectedDate = DateTime.Today;

            // Initialiser kommando
            NavigateBackCommand = new RelayCommand(NavigateBack);

            // Indlæs bookinger fra databasen
            LoadBookings();
        }

        // Egenskab: Den valgte dato i kalenderen
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));

                // Filtrér bookinger for den valgte dato
                FilterBookingsBySelectedDate();
            }
        }

        // Egenskab: Bookinger, der vises i DataGrid
        public ObservableCollection<Booking> Bookings
        {
            get => _bookings;
            private set
            {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
            }
        }

        // Kommando: Naviger tilbage til hovedmenu
        public ICommand NavigateBackCommand { get; }

        // Hent bookinger fra databasen
        private void LoadBookings()
        {
            var allBookings = _bookingRepository.GetAll();
            Bookings = new ObservableCollection<Booking>(allBookings);
            FilterBookingsBySelectedDate();
        }

        // Filtrér bookinger baseret på den valgte dato
        private void FilterBookingsBySelectedDate()
        {
            if (Bookings != null)
            {
                var filteredBookings = _bookingRepository.GetAll()
                    .Where(b => b.DateTimeStart.Date == SelectedDate.Date);

                Bookings = new ObservableCollection<Booking>(filteredBookings);
            }
        }

        // Naviger tilbage til hovedmenuen
        private void NavigateBack()
        {
            _navigateBackAction?.Invoke();
        }

        // Event for property changes
        public event PropertyChangedEventHandler? PropertyChanged; // Markeret som nullable
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
