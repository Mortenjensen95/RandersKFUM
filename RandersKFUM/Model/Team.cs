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
        public string TeamType { get; set; }
        public int TeamLeaderId { get; set; }
        public string TeamName { get; set; }

        public TeamLeader TeamLeader { get; set; }

        public Team () { }

        public Team(int teamId, string teamType, int teamLeaderId, string teamName)
        {
            TeamId = teamId;
            TeamType = teamType;
            TeamLeaderId = teamLeaderId;
            TeamName = teamName;

        }
    }
}
