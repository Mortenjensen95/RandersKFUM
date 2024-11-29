using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Model //test
{
    public class Booking
    {
        public int BookingNumber { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public int TeamId { get; set; }

        public Team Team { get; set; } // Navigation til Team

        //En booking kan have flere fields/LockerRooms 
        public ICollection<Field> Fields { get; set; } // Navigation til mange til mange relation
        public ICollection<LockerRoom> LockerRooms { get; set; }


        public Booking () 
        {
            //Hver gang jeg laver en instants af booking så laver jeg en liste af fields
            Fields = new List<Field>();
            LockerRooms = new List<LockerRoom>();
        }

        //en constructor med parametere (constructor overload)
        public Booking(int bookingNumber, DateTime dateTimeStart, DateTime dateTimeEnd, int teamId)
        {
            BookingNumber = bookingNumber;
            DateTimeStart = dateTimeStart;
            DateTimeEnd = dateTimeEnd;
            TeamId = teamId;
            Fields = new List<Field>();
            LockerRooms = new List<LockerRoom>();
        }
    }
}
