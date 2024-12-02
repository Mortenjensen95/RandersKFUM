using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Model
{
    public class LockerRoomBooking
    {
        public int LockerRoomId { get; set; }
        public int BookingNumber { get; set; }

        // Navigation properties
        public LockerRoom LockerRoom { get; set; }
        public Booking Booking { get; set; }
    }
}
