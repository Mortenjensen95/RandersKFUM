using RandersKFUM.Hjælpeklasser;
using RandersKFUM.Model;
using RandersKFUM.Repository;
using RandersKFUM.Utilities;
using RandersKFUM.View;
using RandersKFUM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RandersKFUM.ViewModel
{
    public class EditBookingViewModel : BookingViewModel
    {
        private readonly Booking existingBooking;


        // Constructor
        public EditBookingViewModel(Booking booking, FieldRepository fieldRepo, LockerRoomRepository lockerRoomRepo, BookingRepository bookingRepo, TeamRepository teamRepo)
            : base(fieldRepo, lockerRoomRepo, bookingRepo, teamRepo)
        {
            existingBooking = booking;

            // Pre-fill properties based on the existing booking
            SelectedDate = booking.DateTimeStart.Date;
            SelectedTimeSlot = booking.DateTimeStart.TimeOfDay;
            SelectedDuration = (int)(booking.DateTimeEnd - booking.DateTimeStart).TotalMinutes;
            SelectedTeam = Teams.FirstOrDefault(t => t.TeamId == booking.TeamId);

            SelectedFields = new ObservableCollection<FieldStatus>(
            FieldAvailability.Where(f => booking.FieldBookings.Any(b => b.FieldId == f.FieldId)));

            SelectedLockerRooms = new ObservableCollection<LockerRoomStatus>(
            LockerRoomAvailability.Where(l => booking.LockerRoomBookings.Any(b => b.LockerRoomId == l.LockerRoomId)));

        }

        // Override ConfirmBooking to handle updating the booking
        public override void ConfirmBooking()
        {
            if (!SelectedFields.Any() || !SelectedLockerRooms.Any() || SelectedTeam == null)
            {
                MessageBox.Show("Vælg venligst mindst én bane, ét omklædningsrum og et hold.", "Validering Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var start = SelectedDate.Date + SelectedTimeSlot;
            var end = start.AddMinutes(SelectedDuration);

            // Opret opdateret booking
            var updatedBooking = new Booking
            {
                BookingNumber = existingBooking.BookingNumber,
                DateTimeStart = start,
                DateTimeEnd = end,
                TeamId = SelectedTeam.TeamId
            };

            // Ekstraher FieldIds og LockerRoomIds fra FieldStatus og LockerRoomStatus
            var fieldIds = SelectedFields.Select(f => f.FieldId).ToList();
            var lockerRoomIds = SelectedLockerRooms.Select(lr => lr.LockerRoomId).ToList();

            // Opdater booking via repository
            bookingRepository.Update(updatedBooking, fieldIds, lockerRoomIds);

            MessageBox.Show("Booking opdateret!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Naviger tilbage
            RandersKFUM.Utilities.NavigationService.NavigateTo(new BookingOverviewView());
        }


    }
}
