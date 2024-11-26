using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandersKFUM.Model;

namespace RandersKFUM.Repository
{
    internal class TeamLeaderRepository : IRepository<TeamLeader>
    {
        public List<TeamLeader> GetAll()
        {
            var teamLeaders = new List<TeamLeader>();

            using (var connection = new SqlConnection(_connection.ConnectionString))
            {
                connection.Open(); // Sørg for, at forbindelsen er åben
                using (var command = new SqlCommand("uspGetAllTeamLeaders", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                teamLeaders.Add(new TeamLeader
                                {
                                    TeamLeaderId = (int)reader["TeamLeaderId"], // Sørg for at matche datatyper
                                    Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : string.Empty,
                                    UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty,
                                    Password = reader["Password"] != DBNull.Value ? reader["Password"].ToString() : string.Empty,
                                    Phone = reader["Phone"] != DBNull.Value ? (int)reader["Phone"] : 0,
                                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error while retrieving TeamLeaders: {ex.Message}");
                    }
                }
            }

            return teamLeaders;
        }


        public TeamLeader GetById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    connection.Open(); // Sørg for at forbindelsen er åben
                    using (var command = new SqlCommand("uspGetTeamLeaderById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Tilføj parameter med korrekt datatype
                        command.Parameters.Add(new SqlParameter("@TeamLeaderId", SqlDbType.Int)).Value = id;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TeamLeader
                                {
                                    TeamLeaderId = Convert.ToInt32(reader["TeamLeaderId"]),
                                    Name = reader["Name"]?.ToString(),
                                    UserName = reader["UserName"]?.ToString(),
                                    Password = reader["Password"]?.ToString(),
                                    Phone = reader["Phone"] != DBNull.Value ? Convert.ToInt32(reader["Phone"]) : 0,
                                    Email = reader["Email"]?.ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log fejl eller håndter den på anden vis
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Returner null, hvis intet resultat findes eller der opstod en fejl
            return null;
        }



        public void Add(TeamLeader teamLeader)
        {
            using (var connection = new SqlConnection(_connection.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspAddTeamLeader", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Tilføj inputparametre
                    command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)).Value = teamLeader.Name;
                    command.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 50)).Value = teamLeader.UserName;
                    command.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 50)).Value = teamLeader.Password;
                    command.Parameters.Add(new SqlParameter("@Phone", SqlDbType.Int)).Value = teamLeader.Phone;
                    command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50)).Value = teamLeader.Email;

                    // Tilføj outputparameter for at få det nye TeamLeaderId
                    var teamLeaderIdParam = new SqlParameter("@TeamLeaderId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(teamLeaderIdParam);

                    // Udfør proceduren
                    command.ExecuteNonQuery();

                    // Hent outputparameteren
                    teamLeader.TeamLeaderId = (int)teamLeaderIdParam.Value;
                }
            }
        }


        public void Delete(int teamLeaderId)
        {
            // Sikrer korrekt forbindelseshåndtering
            using (var connection = new SqlConnection(_connection.ConnectionString))
            {
                connection.Open(); // Sørger for, at forbindelsen er åben
                using (var command = new SqlCommand("uspDeleteTeamLeader", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Tilføj parameter med korrekt datatype
                    command.Parameters.Add(new SqlParameter("@TeamLeaderId", SqlDbType.Int)).Value = teamLeaderId;

                    // Udfør sletning
                    command.ExecuteNonQuery();
                }
            }
        }


        public void Update(TeamLeader teamLeader)
        {
            // Sørg for at forbindelsen håndteres korrekt
            using (var connection = new SqlConnection(_connection.ConnectionString))
            {
                connection.Open(); // Sørg for, at forbindelsen er åben
                using (var command = new SqlCommand("uspUpdateTeamLeader", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Tilføj parametre med korrekte typer
                    command.Parameters.Add(new SqlParameter("@TeamLeaderId", SqlDbType.Int)).Value = teamLeader.TeamLeaderId;
                    command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50)).Value = teamLeader.Name;
                    command.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 50)).Value = teamLeader.UserName;
                    command.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 50)).Value = teamLeader.Password;
                    command.Parameters.Add(new SqlParameter("@Phone", SqlDbType.Int)).Value = teamLeader.Phone;
                    command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50)).Value = teamLeader.Email;

                    // Udfør opdatering
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
