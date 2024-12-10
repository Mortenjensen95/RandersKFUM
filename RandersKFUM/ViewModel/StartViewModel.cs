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
    public class StartViewModel : ViewModelBase
    {
        private readonly BookingRepository bookingRepository;

        public ObservableCollection<BookingOverview> AllBookings { get; private set; }
        public ObservableCollection<BookingOverview> FilteredBookings { get; private set; }

        public RelayCommand NavigateToLogInCommand { get; }

        public StartViewModel()
        {
            bookingRepository = new BookingRepository(DatabaseConfig.GetConnectionString());

            NavigateToLogInCommand = new RelayCommand(_ => NavigateToLogIn());

            LoadBookings();

            SelectedDate = DateTime.Today;

        }

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

        private void LoadBookings()
        {
            var bookingOverviews = bookingRepository.GetBookingOverviews();
            AllBookings = new ObservableCollection<BookingOverview>(bookingOverviews);

            FilteredBookings = new ObservableCollection<BookingOverview>(AllBookings);

            OnPropertyChanged(nameof(AllBookings));
            OnPropertyChanged(nameof(FilteredBookings));
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

        private void NavigateToLogIn()
        {
            NavigationService.NavigateTo(new LoginView());
        }
    }

}