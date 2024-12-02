using RandersKFUM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;


namespace RandersKFUM.Repository
{
    public class BookingRepository : IRepository<Booking>
    {
        private readonly string _connectionString;

        public BookingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Booking> GetAll()
        {
            var bookings = new List<Booking>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspGetAllBookings", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bookings.Add(new Booking
                            {
                                BookingNumber = Convert.ToInt32(reader["BookingNumber"]),
                                DateTimeStart = reader["DateTimeStart"] != DBNull.Value ? Convert.ToDateTime(reader["DateTimeStart"]) : DateTime.MinValue,
                                DateTimeEnd = reader["DateTimeEnd"] != DBNull.Value ? Convert.ToDateTime(reader["DateTimeEnd"]) : DateTime.MinValue,
                                TeamId = reader["TeamId"] != DBNull.Value ? Convert.ToInt32(reader["TeamId"]) : 0
                            });
                        }
                    }
                }
            }

            return bookings;
        }

        public Booking GetById(int bookingNumber)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspGetBookingbyId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@BookingNumber", SqlDbType.Int)).Value = bookingNumber;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Booking
                            {
                                BookingNumber = Convert.ToInt32(reader["BookingNumber"]),
                                DateTimeStart = reader["DateTimeStart"] != DBNull.Value ? Convert.ToDateTime(reader["DateTimeStart"]) : DateTime.MinValue,
                                DateTimeEnd = reader["DateTimeEnd"] != DBNull.Value ? Convert.ToDateTime(reader["DateTimeEnd"]) : DateTime.MinValue,
                                TeamId = reader["TeamId"] != DBNull.Value ? Convert.ToInt32(reader["TeamId"]) : 0
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Add(Booking booking, int fieldId, int lockerRoomId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspAddBooking", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@DateTimeStart", booking.DateTimeStart));
                    command.Parameters.Add(new SqlParameter("@DateTimeEnd", booking.DateTimeEnd));
                    command.Parameters.Add(new SqlParameter("@TeamId", booking.TeamId));
                    command.Parameters.Add(new SqlParameter("@FieldId", fieldId));
                    command.Parameters.Add(new SqlParameter("@LockerRoomId", lockerRoomId));

                    command.ExecuteNonQuery();
                }
            }
        }


        public void Update(Booking booking, int fieldId, int lockerRoomId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspUpdateBooking", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@BookingNumber", booking.BookingNumber));
                    command.Parameters.Add(new SqlParameter("@DateTimeStart", booking.DateTimeStart));
                    command.Parameters.Add(new SqlParameter("@DateTimeEnd", booking.DateTimeEnd));
                    command.Parameters.Add(new SqlParameter("@TeamId", booking.TeamId));
                    command.Parameters.Add(new SqlParameter("@FieldId", fieldId));
                    command.Parameters.Add(new SqlParameter("@LockerRoomId", lockerRoomId));

                    command.ExecuteNonQuery();
                }
            }
        }


        public void Delete(int bookingNumber)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspDeleteBooking", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@BookingNumber", bookingNumber));
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
