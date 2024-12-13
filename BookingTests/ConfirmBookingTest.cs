using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandersKFUM.Hjælpeklasser;
using RandersKFUM.Model;
using RandersKFUM.Repository;
using RandersKFUM.ViewModel;
using System;
using System.Collections.ObjectModel;

namespace BookingTests
{
    [TestClass]
    public class ConfirmBookingTest
    {
        private BookingViewModel _viewModel;
        private BookingRepository _testBookingRepository;

        [TestInitialize]
        public void Setup()
        {
            // Brug test-databaseforbindelse
            string testConnectionString = "Server=localhost;Database=KFUMBookingsystemTests;Trusted_Connection=True;TrustServerCertificate=True;";
            _testBookingRepository = new BookingRepository(testConnectionString);

            // Initialiser ViewModel
            _viewModel = new BookingViewModel();

            // Erstat det eksisterende BookingRepository med test-versionen
            typeof(BookingViewModel)
                .GetField("bookingRepository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_viewModel, _testBookingRepository);

            // Initialiser de nødvendige ressourcer
            _viewModel.Teams = new ObservableCollection<Team>
            {
                new Team { TeamId = 1, TeamName = "Test Team" }
            };
            _viewModel.Fields = new ObservableCollection<Field>
            {
                new Field { FieldId = 1, FieldNumber = 1, FieldType = "7-mands" }
            };
            _viewModel.LockerRooms = new ObservableCollection<LockerRoom>
            {
                new LockerRoom { LockerRoomId = 1, LockerRoomNumber = 1, LockerRoomType = "enkelt" }
            };
        }

        [TestMethod]
        public void ConfirmBooking_Should_Add_Booking_To_Database()
        {
            var testThread = new Thread(() =>
            {
                // Arrange
                _viewModel.SelectedDate = new DateTime(2025, 12, 13);
                _viewModel.SelectedTimeSlot = new TimeSpan(17, 0, 0); // Kl. 17:00
                _viewModel.SelectedDuration = 60; // 1 time
                _viewModel.SelectedFields = new ObservableCollection<FieldStatus>
        {
            new FieldStatus { FieldId = 1, IsAvailable = true }
        };
                _viewModel.SelectedLockerRooms = new ObservableCollection<LockerRoomStatus>
        {
            new LockerRoomStatus { LockerRoomId = 1, IsAvailable = true }
        };
                _viewModel.SelectedTeam = new Team { TeamId = 1, TeamName = "Test Team" };

                // Act
                _viewModel.ConfirmBooking();

                // Assert
                var addedBooking = _testBookingRepository.GetAll().LastOrDefault(); // Hent senest tilføjede booking
                Assert.IsNotNull(addedBooking, "Booking blev ikke tilføjet til databasen.");
                Assert.AreEqual(_viewModel.SelectedDate.Add(_viewModel.SelectedTimeSlot), addedBooking.DateTimeStart, "Starttidspunktet matcher ikke.");
                Assert.AreEqual(_viewModel.SelectedDate.Add(_viewModel.SelectedTimeSlot).AddMinutes(_viewModel.SelectedDuration), addedBooking.DateTimeEnd, "Sluttidspunktet matcher ikke.");
                Assert.AreEqual(_viewModel.SelectedTeam.TeamId, addedBooking.TeamId, "TeamId matcher ikke.");
            });

            // Kør testen i en STA-tråd
            testThread.SetApartmentState(ApartmentState.STA);
            testThread.Start();
            testThread.Join();
        }

    }
}
