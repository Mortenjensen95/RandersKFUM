using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Model
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string TeamType { get; set; }
        public int TeamLeaderId { get; set; }

        public TeamLeader TeamLeader { get; set; }

        public Team(int teamId, string name, string teamType, int teamLeaderId)
        {
            TeamId = teamId;
            Name = name;
            TeamType = teamType;
            TeamLeaderId = teamLeaderId;
        }
    }
}
