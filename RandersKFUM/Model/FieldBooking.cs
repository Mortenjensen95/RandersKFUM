using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Model
{
    public class FieldBooking
    {
        public int FieldId { get; set; }
        public int BookingNumber { get; set; }

        // Navigation properties
        public Field Field { get; set; }
        public Booking Booking { get; set; }
    }
}
