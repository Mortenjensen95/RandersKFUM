using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Repository
{
    internal class TeamLeaderRepository : IRepository<TeamLeader>
    {
        public List<TeamLeader> GetAll()
        {
            var orders = new List<TeamLeader>();

            using (var command = new SqlCommand("uspGetAllTeamLeaders", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new TeamLeader
                        {
                            TeamLeaderId = (int)reader["TeamLeaderId"],
                            Name = reader["Name"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            Password = reader["Password"].ToString(),
                            Phone = (int)reader["Phone"],
                            Email = reader["Email"].ToString()
                        });
                    }
                }
            }

            return orders;
        }

        public TeamLeader GetById(int id)
        {
            using (var command = new SqlCommand("uspGetTeamLeaderById", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TeamLeaderId", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TeamLeader
                        {
                            TeamLeaderId = (int)reader["TeamLeaderId"],
                            Name = reader["Name"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            Password = reader["Password"].ToString(),
                            Phone = (int)reader["Phone"],
                            Email = reader["Email"].ToString()
                        };
                    }
                }
            }
            return null;
        }
    }
}
