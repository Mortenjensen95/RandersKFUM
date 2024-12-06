using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Model
{
    public class BookingOverview
    {
        public int BookingNumber { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public string TeamName { get; set; }
        public string FieldNumbers { get; set; }
        public string LockerRoomNumbers { get; set; }
    }

}
