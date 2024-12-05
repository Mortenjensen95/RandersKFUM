using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Model
{
    public class TeamLeader
    {
        public int TeamLeaderId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public ICollection<Team> Teams { get; set; } // navigation property

        public TeamLeader ()
        {
            Teams = new List<Team>();
        }

        public TeamLeader(int teamLeaderId, string name, string userName, string password, string phone, string email)
        {
            TeamLeaderId = teamLeaderId;
            Name = name;
            UserName = userName;
            Password = password;
            Phone = phone;
            Email = email;
            Teams = new List<Team>();
        }

        /*public string TeamLeaderName
        {
            get => TeamLeader?.Name; // Returner navnet, hvis TeamLeader ikke er null
            set
            {
                if (TeamLeader == null)
                    TeamLeader = new TeamLeader();

                TeamLeader.Name = value;
            }
        }
        */
    }
}
