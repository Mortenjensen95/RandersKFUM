using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandersKFUM.Repository;
using RandersKFUM.Model;
using System;
using System.Collections.Generic;

namespace BookingTests
{
    [TestClass]
    public class AddBookingTest
    {
        private BookingRepository _bookingRepository;

        [TestInitialize]
        public void Setup()
        {
            // Angiv forbindelsesstrengen til din test-database
            string connectionString = "Server=localhost;Database=KFUMBookingsystemTests;Trusted_Connection=True;TrustServerCertificate=True;";
            _bookingRepository = new BookingRepository(connectionString);
        }

        [TestMethod]
        public void Add_Booking_Should_Add_Booking_To_Database_And_Assign_Correct_Resources()
        {
            // Arrange
            DateTime bookingStart = new DateTime(2025, 12, 13, 17, 0, 0);  // Starter kl. 17:00 den 13. december 2024
            DateTime bookingEnd = new DateTime(2025, 12, 13, 18, 0, 0);    // Slutter kl. 18:00 den 13. december 2024
            var newBooking = new Booking(0, bookingStart, bookingEnd, 1);
            var fieldIds = new List<int> { 1 };  // Bookinger Bane med Id = 1
            var lockerRoomIds = new List<int> { 1 };  // Bookinger omklædningsrum med Id = 1

            // Act
            _bookingRepository.Add(newBooking, fieldIds, lockerRoomIds);

            // Assert - Tjekker om den nye booking er tilføjet til databasen.
            var addedBooking = _bookingRepository.GetAll().LastOrDefault();
            Assert.IsNotNull(addedBooking, "Booking blev ikke tilføjet til databasen.");
            Assert.AreEqual(bookingStart, addedBooking.DateTimeStart, "Starttidspunktet matcher ikke.");
            Assert.AreEqual(bookingEnd, addedBooking.DateTimeEnd, "Sluttidspunktet matcher ikke.");
            Assert.AreEqual(newBooking.TeamId, addedBooking.TeamId, "TeamId matcher ikke.");

            // Assert - Verify that the correct fields and locker rooms have been assigned
            var assignedFields = _bookingRepository.GetFieldsForBooking(addedBooking.BookingNumber);
            var assignedLockerRooms = _bookingRepository.GetLockerRoomsForBooking(addedBooking.BookingNumber);
            Assert.IsTrue(fieldIds.SequenceEqual(assignedFields), "De tildelte baner matcher ikke de forventede.");
            Assert.IsTrue(lockerRoomIds.SequenceEqual(assignedLockerRooms), "De tildelte omklædningsrum matcher ikke de forventede.");
        }

    }
}
