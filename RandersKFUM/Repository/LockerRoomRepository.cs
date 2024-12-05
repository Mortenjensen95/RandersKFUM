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
    public class LockerRoomRepository : IRepository<LockerRoom>
    {
        private readonly string _connectionString;

        public LockerRoomRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<LockerRoom> GetAll()
        {
            var lockerRooms = new List<LockerRoom>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspGetAllLockerRooms", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lockerRooms.Add(new LockerRoom
                            {
                                LockerRoomId = Convert.ToInt32(reader["LockerRoomId"]),
                                LockerRoomNumber = reader["LockerRoomNumber"] != DBNull.Value ? Convert.ToInt32(reader["LockerRoomNumber"]) : 0,
                                LockerRoomType = reader["LockerRoomType"]?.ToString()
                            });
                        }
                    }
                }
            }

            return lockerRooms;
        }

        public LockerRoom GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspGetLockerRoomById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@LockerRoomId", SqlDbType.Int)).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new LockerRoom
                            {
                                LockerRoomId = Convert.ToInt32(reader["LockerRoomId"]),
                                LockerRoomNumber = reader["LockerRoomNumber"] != DBNull.Value ? Convert.ToInt32(reader["LockerRoomNumber"]) : 0,
                                LockerRoomType = reader["LockerRoomType"]?.ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Add(LockerRoom lockerRoom)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspAddLockerRoom", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@LockerRoomNumber", SqlDbType.Int)).Value = lockerRoom.LockerRoomNumber;
                    command.Parameters.Add(new SqlParameter("@LockerRoomType", SqlDbType.NVarChar, 50)).Value = lockerRoom.LockerRoomType;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(LockerRoom lockerRoom)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspUpdateLockerRoom", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@LockerRoomId", SqlDbType.Int)).Value = lockerRoom.LockerRoomId;
                    command.Parameters.Add(new SqlParameter("@LockerRoomNumber", SqlDbType.Int)).Value = lockerRoom.LockerRoomNumber;
                    command.Parameters.Add(new SqlParameter("@LockerRoomType", SqlDbType.NVarChar, 50)).Value = lockerRoom.LockerRoomType;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspDeleteLockerRoom", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@LockerRoomId", SqlDbType.Int)).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }

        

        public IEnumerable<LockerRoom> GetAvailableLockerRooms(DateTime start, DateTime end)
        {
            var lockerRooms = new List<LockerRoom>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspGetAvailableLockerRooms", connection)) // Brug den rigtige stored procedure
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NewBookingStart", start);
                    command.Parameters.AddWithValue("@NewBookingEnd", end);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lockerRooms.Add(new LockerRoom
                            {
                                LockerRoomId = Convert.ToInt32(reader["LockerRoomId"]),
                                LockerRoomNumber = Convert.ToInt32(reader["LockerRoomNumber"]),
                                LockerRoomType = reader["LockerRoomType"].ToString() // Henter LockerRoomType
                            });
                        }
                    }
                }
            }
            return lockerRooms;
        }




    }

}
