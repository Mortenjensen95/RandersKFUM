using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Model
{
    public class Booking
    {
        public int BookingNumber { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public int TeamId { get; }

        public Team Team { get; set; } // Navigation til Team

        public ICollection<FieldBooking> FieldBookings { get; set; } // Navigation til mange til mange relation
        public ICollection<LockerRoomBooking> LockerRoomBookings { get; set; }

        public Booking(int bookingNumber, DateTime dateTimeStart, DateTime dateTimeEnd, int teamId)
        {
            BookingNumber = bookingNumber;
            DateTimeStart = dateTimeStart;
            DateTimeEnd = dateTimeEnd;
            TeamId = teamId;
            FieldBookings = new List<FieldBookings>();
            LockerRoomBookings = new List<LockerRoomBookings>();
        }
    }
}
