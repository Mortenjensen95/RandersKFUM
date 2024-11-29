using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Model
{
    public class Field
    {
        //properties af field
        public int FieldId { get; set; }
        public string FieldType { get; set; }
        public int FieldNumber { get; set; }

        //ICollection er en form for liste. En field kan indgå i mange bookings (liste af bookings)
        public ICollection<Booking> Bookings { get; set; }

        public Field() 
        {
            Bookings = new List<Booking>();
        }

        public Field(string fieldType, int fieldNumber)
        {
            FieldType = fieldType;
            FieldNumber = fieldNumber;
            Bookings = new List<Booking>();
        }
    }
}
