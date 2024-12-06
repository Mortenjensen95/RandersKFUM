using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandersKFUM.Model;
using RandersKFUM.Repository;
using RandersKFUM.Utilities;
using RandersKFUM.View;
using RandersKFUM.ViewModel;

namespace RandersKFUM.ViewModel
{
    public class BookingOverviewViewModel
    {
        private readonly FieldRepository fieldRepository;
        private readonly LockerRoomRepository lockerRoomRepository;
        private readonly BookingRepository bookingRepository;
        private readonly TeamRepository teamRepository;

        public RelayCommand NavigateBackToMainMenuCommand { get; }
        public RelayCommand EditBookingCommand { get; }


        public BookingOverviewViewModel(FieldRepository fieldRepo, LockerRoomRepository lockerRoomRepo, BookingRepository bookingRepo, TeamRepository teamRepo)
        {
            // Inject dependencies
            fieldRepository = fieldRepo;
            lockerRoomRepository = lockerRoomRepo;
            bookingRepository = bookingRepo;
            teamRepository = teamRepo;

            // Initialize commands
            NavigateBackToMainMenuCommand = new RelayCommand(_ => NavigateBackToMainMenuView());
            EditBookingCommand = new RelayCommand(param => OpenEditBookingView(param as Booking));

        }

        private void NavigateBackToMainMenuView()
        {
            // Navigate to main menu
            NavigationService.NavigateTo(new MainMenuView());
        }

        private void OpenEditBookingView(Booking selectedBooking)
        {
            if (selectedBooking == null) return;

            // Pass the booking to EditBookingView
            var editView = new EditBookingView(selectedBooking);
            NavigationService.NavigateTo(editView);
        }
    }
}

