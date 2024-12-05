using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Hjælpeklasser
{
    public class FieldStatus
    {
        public int FieldId { get; set; }
        public int FieldNumber { get; set; }
        public string FieldType { get; set; }
        public bool IsAvailable { get; set; } = false;
    }
}
