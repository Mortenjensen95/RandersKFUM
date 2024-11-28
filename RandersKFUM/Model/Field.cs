using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Model
{
    public class Field
    {
        public int FieldId { get; set; }
        public string FieldType { get; set; }
        public int FieldNumber { get; set; }

        public ICollection<FieldBooking> FieldBookings { get; set; }

        
        public Field(string fieldType, int fieldNumber)
        {
            FieldType = fieldType;
            FieldNumber = fieldNumber;
            FieldBookings = new List<FieldBooking>();
        }
        
    }
}
