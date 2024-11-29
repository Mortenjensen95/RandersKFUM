using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Model
{
    public class LockerRoom
    {
        public int LockerRoomId { get; set; }
        public int LockerRoomNumber { get; set; }
        public string LockerRoomType { get; set; }

        //En lockerroom kan være i flere bookings 
        public ICollection<Booking> Bookings { get; set; }

        public LockerRoom () 
        {
            Bookings = new List<Booking>();
        }

        public LockerRoom(int lockerRoomId, int lockerRoomNumber, string lockerRoomType)
        {
            LockerRoomId = lockerRoomId;
            LockerRoomNumber = lockerRoomNumber;
            LockerRoomType = lockerRoomType;
            Bookings = new List<Booking>();
        }
    }
}
