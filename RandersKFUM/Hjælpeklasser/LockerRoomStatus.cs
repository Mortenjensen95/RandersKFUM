using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Hjælpeklasser
{
    public class LockerRoomStatus
    {
        public int LockerRoomId { get; set; }
        public int LockerRoomNumber { get; set; } // Nummeret på omklædningsrummet
        public string LockerRoomType { get; set; } // Typen af omklædningsrum (fx Large, Small)
        public bool IsAvailable { get; set; } = false;
    }
}
