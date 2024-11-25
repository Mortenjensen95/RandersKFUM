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

        public ICollection<LockerRoomBooking> LockerRoomBookings { get; set; }

        public LockerRoom(int lockerRoomId, int lockerRoomNumber, string lockerRoomType)
        {
            LockerRoomId = lockerRoomId;
            LockerRoomNumber = lockerRoomNumber;
            LockerRoomType = lockerRoomType;
            LockerRoomBookings = new List<LockerRoomBooking>();
        }
    }
}
