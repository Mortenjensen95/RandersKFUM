using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Model
{
    public class Booking
    {
        // Properties til at holde information om en booking
        public int BookingNumber { get; set; } // Unikt identifikationsnummer for bookingen
        public DateTime DateTimeStart { get; set; } // Starttidspunkt for bookingen
        public DateTime DateTimeEnd { get; set; } // Sluttidspunkt for bookingen
        public int TeamId { get; set; } // ID for det team, der laver bookingen

        public Team Team { get; set; } // Navigationsejendom til Team

        public ICollection<FieldBooking> FieldBookings { get; set; } // En samling af banereservationer forbundet med denne booking
        public ICollection<LockerRoomBooking> LockerRoomBookings { get; set; } // En samling af omklædningsrumsreservationer forbundet med denne booking

        // Konstruktør til at initialisere en ny booking med nødvendige oplysninger

        public Booking(int bookingNumber, DateTime dateTimeStart, DateTime dateTimeEnd, int teamId)
        {
            BookingNumber = bookingNumber;
            DateTimeStart = dateTimeStart;
            DateTimeEnd = dateTimeEnd;
            TeamId = teamId;
            FieldBookings = new List<FieldBooking>(); // Initialiserer listen af banereservationer
            LockerRoomBookings = new List<LockerRoomBooking>(); // Initialiserer listen af omklædningsrumsreservationer
        }
    }
}
