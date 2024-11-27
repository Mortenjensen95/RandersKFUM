using Microsoft.Data.SqlClient;
using RandersKFUM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Repository
{
    public class TeamRepository : IRepository<Team>
    {
        private readonly string _connectionString;

        public TeamRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Team> GetAll()
        {
            var teams = new List<Team>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspGetAllTeams", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teams.Add(new Team
                            {
                                TeamId = Convert.ToInt32(reader["TeamId"]),
                                TeamType = reader["TeamType"]?.ToString(),
                                TeamLeaderId = reader["TeamLeaderId"] != DBNull.Value ? Convert.ToInt32(reader["TeamLeaderId"]) : 0,
                                TeamName = reader["TeamName"]?.ToString()
                            });
                        }
                    }
                }
            }

            return teams;
        }

        public Team GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspGetTeamById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@TeamId", SqlDbType.Int)).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Team
                            {
                                TeamId = Convert.ToInt32(reader["TeamId"]),
                                TeamType = reader["TeamType"]?.ToString(),
                                TeamLeaderId = reader["TeamLeaderId"] != DBNull.Value ? Convert.ToInt32(reader["TeamLeaderId"]) : 0,
                                TeamName = reader["TeamName"]?.ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Add(Team team)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspAddTeam", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@TeamType", SqlDbType.NVarChar, 50)).Value = team.TeamType;
                    command.Parameters.Add(new SqlParameter("@TeamLeaderId", SqlDbType.Int)).Value = team.TeamLeaderId;
                    command.Parameters.Add(new SqlParameter("@TeamName", SqlDbType.NVarChar, 50)).Value = team.TeamName;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Team team)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspUpdateTeam", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@TeamId", SqlDbType.Int)).Value = team.TeamId;
                    command.Parameters.Add(new SqlParameter("@TeamType", SqlDbType.NVarChar, 50)).Value = team.TeamType;
                    command.Parameters.Add(new SqlParameter("@TeamLeaderId", SqlDbType.Int)).Value = team.TeamLeaderId;
                    command.Parameters.Add(new SqlParameter("@TeamName", SqlDbType.NVarChar, 50)).Value = team.TeamName;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspDeleteTeam", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@TeamId", SqlDbType.Int)).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }
    }

}
