using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
        public RelayCommand DeleteBookingCommand { get; }

        private BookingOverview selectedBooking;
        public BookingOverview SelectedBooking
        {
            get { return selectedBooking; }
            set
            {
                selectedBooking = value;
                OnPropertyChanged(nameof(SelectedBooking));
            }
        }

        public BookingOverviewViewModel(FieldRepository fieldRepo, LockerRoomRepository lockerRoomRepo, BookingRepository bookingRepo, TeamRepository teamRepo)
        {
            fieldRepository = fieldRepo;
            lockerRoomRepository = lockerRoomRepo;
            bookingRepository = bookingRepo;
            teamRepository = teamRepo;

            NavigateBackToMainMenuCommand = new RelayCommand(_ => NavigateBackToMainMenuView());
            DeleteBookingCommand = new RelayCommand(_ => DeleteBooking(), canExecute => SelectedBooking != null);

            LoadBookings();

            SelectedDate = DateTime.Today;
        }

        private void LoadBookings()
        {
            try
            {
                var bookingOverviews = bookingRepository.GetBookingOverviews();
                AllBookings = new ObservableCollection<BookingOverview>(bookingOverviews);

                FilteredBookings = new ObservableCollection<BookingOverview>(AllBookings);

                OnPropertyChanged(nameof(AllBookings));
                OnPropertyChanged(nameof(FilteredBookings));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunne ikke indlæse bookingdata: " + ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterBookingsByDate()
        {
            if (SelectedDate.HasValue)
            {
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
                FilteredBookings.Clear();
                foreach (var booking in AllBookings)
                {
                    FilteredBookings.Add(booking);
                }
            }

            OnPropertyChanged(nameof(FilteredBookings));
        }

        private void DeleteBooking()
        {
            if (SelectedBooking == null) return;

            var result = MessageBox.Show("Er du sikker på, at du vil slette denne booking?", "Bekræft Sletning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bookingRepository.Delete(SelectedBooking.BookingNumber);
                    LoadBookings();
                    FilterBookingsByDate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kunne ikke slette booking: " + ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void NavigateBackToMainMenuView()
        {
            NavigationService.NavigateTo(new MainMenuView());
        }
    }

}