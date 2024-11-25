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
        public string FieldName { get; set; }
        public int FieldNumber { get; set; }

        public ICollection<FieldBooking> FieldBookings { get; set; }

        public Field(string fieldName, int fieldNumber)
        {
            FieldName = fieldName;
            FieldNumber = fieldNumber;
            FieldBookings = new List<FieldBooking>();
        }
    }
}
