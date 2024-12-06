using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RandersKFUM.Model;
using RandersKFUM.Repository;
using RandersKFUM.Utilities;
using RandersKFUM.View;

namespace RandersKFUM.ViewModel
{
    public class BookingOverviewViewModel : ViewModelBase
    {
        private readonly FieldRepository fieldRepository;
        private readonly LockerRoomRepository lockerRoomRepository;
        private readonly BookingRepository bookingRepository;
        private readonly TeamRepository teamRepository;

        public ObservableCollection<BookingOverview> AllBookings { get; private set; }
        public ObservableCollection<BookingOverview> FilteredBookings { get; private set; }

        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                FilterBookingsByDate();
            }
        }

        public RelayCommand NavigateBackToMainMenuCommand { get; }

        public BookingOverviewViewModel(FieldRepository fieldRepo, LockerRoomRepository lockerRoomRepo, BookingRepository bookingRepo, TeamRepository teamRepo)
        {
            // Inject dependencies
            fieldRepository = fieldRepo;
            lockerRoomRepository = lockerRoomRepo;
            bookingRepository = bookingRepo;
            teamRepository = teamRepo;

            // Initialize commands
            NavigateBackToMainMenuCommand = new RelayCommand(_ => NavigateBackToMainMenuView());

            // Load all bookings
            LoadBookings();

            // Set default date to today's date
            SelectedDate = DateTime.Today;
        }

        private void LoadBookings()
        {
            // Hent bookingoversigten via repository
            var bookingOverviews = bookingRepository.GetBookingOverviews();
            AllBookings = new ObservableCollection<BookingOverview>(bookingOverviews);

            // Initialiser filtreret liste
            FilteredBookings = new ObservableCollection<BookingOverview>(AllBookings);

            OnPropertyChanged(nameof(AllBookings));
            OnPropertyChanged(nameof(FilteredBookings));
        }

        private void FilterBookingsByDate()
        {
            if (SelectedDate.HasValue)
            {
                // Filtrér bookinger for den valgte dato
                var filtered = AllBookings
                    .Where(b => b.DateTimeStart.Date == SelectedDate.Value.Date)
                    .ToList();

                FilteredBookings.Clear();
                foreach (var booking in filtered)
                {
                    FilteredBookings.Add(booking);
                }
            }
            else
            {
                // Hvis ingen dato er valgt, vis alle bookinger
                FilteredBookings.Clear();
                foreach (var booking in AllBookings)
                {
                    FilteredBookings.Add(booking);
                }
            }

            OnPropertyChanged(nameof(FilteredBookings));
        }

        private void NavigateBackToMainMenuView()
        {
            // Navigate to main menu
            NavigationService.NavigateTo(new MainMenuView());
        }

    }
}
