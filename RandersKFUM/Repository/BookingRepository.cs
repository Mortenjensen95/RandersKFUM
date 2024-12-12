using RandersKFUM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;


namespace RandersKFUM.Repository
{
    public class BookingRepository
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
                            bookings.Add(new Booking(
                            Convert.ToInt32(reader["BookingNumber"]),
                            reader["DateTimeStart"] != DBNull.Value ? Convert.ToDateTime(reader["DateTimeStart"]) : DateTime.MinValue,
                            reader["DateTimeEnd"] != DBNull.Value ? Convert.ToDateTime(reader["DateTimeEnd"]) : DateTime.MinValue,
                            reader["TeamId"] != DBNull.Value ? Convert.ToInt32(reader["TeamId"]) : 0
                            ));

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
                            return new Booking(
                                Convert.ToInt32(reader["BookingNumber"]),
                                reader["DateTimeStart"] != DBNull.Value ? Convert.ToDateTime(reader["DateTimeStart"]) : DateTime.MinValue,
                                reader["DateTimeEnd"] != DBNull.Value ? Convert.ToDateTime(reader["DateTimeEnd"]) : DateTime.MinValue,
                                reader["TeamId"] != DBNull.Value ? Convert.ToInt32(reader["TeamId"]) : 0
                            );
                        }
                    }
                }
            }

            return null;
        }

        public void Add(Booking booking, IEnumerable<int> fieldIds, IEnumerable<int> lockerRoomIds)
        {
            using (var connection = new SqlConnection(_connectionString)) // Opretter en forbindelse til databasen
            {
                connection.Open(); // Åbner forbindelsen
                using (var command = new SqlCommand("uspAddBooking", connection)) // Opretter en SQL-kommando til at kalde en stored procedure
                {
                    command.CommandType = CommandType.StoredProcedure; // Angiver at kommandotypen er en stored procedure

                    // Tilføjer parametre til kommandoen
                    command.Parameters.Add(new SqlParameter("@DateTimeStart", booking.DateTimeStart));
                    command.Parameters.Add(new SqlParameter("@DateTimeEnd", booking.DateTimeEnd));
                    command.Parameters.Add(new SqlParameter("@TeamId", booking.TeamId));

                    // Opretter og tilføjer parameter for banernes ID'er
                    var fieldIdsParam = new SqlParameter("@FieldIds", SqlDbType.Structured)
                    {
                        TypeName = "dbo.IntList", // Definerer den brugerdefinerede tabeltype
                        Value = ConvertToDataTable(fieldIds) // Konverterer fieldIds til DataTable
                    };
                    command.Parameters.Add(fieldIdsParam);

                    // Opretter og tilføjer parameter for omklædningsrummenes ID'er
                    var lockerRoomIdsParam = new SqlParameter("@LockerRoomIds", SqlDbType.Structured)
                    {
                        TypeName = "dbo.IntList", // Definerer den brugerdefinerede tabeltype
                        Value = ConvertToDataTable(lockerRoomIds) // Konverterer lockerRoomIds til DataTable
                    };
                    command.Parameters.Add(lockerRoomIdsParam);

                    command.ExecuteNonQuery(); // Udfører SQL-kommandoen
                }
            }
        }


        private DataTable ConvertToDataTable(IEnumerable<int> ids)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            foreach (var id in ids)
            {
                table.Rows.Add(id);
            }
            return table;
        }



        public void Update(Booking updatedBooking, IEnumerable<int> fieldIds, IEnumerable<int> lockerRoomIds)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            // Konverter fieldIds til DataTable
            var fieldIdsTable = new DataTable();
            fieldIdsTable.Columns.Add("Id", typeof(int));
            foreach (var id in fieldIds)
            {
                fieldIdsTable.Rows.Add(id);
            }

            // Konverter lockerRoomIds til DataTable
            var lockerRoomIdsTable = new DataTable();
            lockerRoomIdsTable.Columns.Add("Id", typeof(int));
            foreach (var id in lockerRoomIds)
            {
                lockerRoomIdsTable.Rows.Add(id);
            }

            // Opret parameterobjekter til stored procedure
            var parameters = new[]
            {
        new SqlParameter("@BookingNumber", updatedBooking.BookingNumber),
        new SqlParameter("@DateTimeStart", updatedBooking.DateTimeStart),
        new SqlParameter("@DateTimeEnd", updatedBooking.DateTimeEnd),
        new SqlParameter("@TeamId", updatedBooking.TeamId),
        new SqlParameter("@FieldIds", SqlDbType.Structured)
        {
            TypeName = "dbo.IntList",
            Value = fieldIdsTable
        },
        new SqlParameter("@LockerRoomIds", SqlDbType.Structured)
        {
            TypeName = "dbo.IntList",
            Value = lockerRoomIdsTable
        }
    };

            // Kald stored procedure
            using var command = new SqlCommand("uspUpdateBooking", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddRange(parameters);
            command.ExecuteNonQuery();
        }

        public void Delete(int bookingNumber)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Opretter en SqlCommand for at kalde stored procedure
                using (var command = new SqlCommand("uspDeleteBooking", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@BookingNumber", bookingNumber));

                    command.ExecuteNonQuery();  // Udfører kommandoen
                }
            }
        }




        public IEnumerable<BookingOverview> GetBookingOverviews()
        {
            var bookings = new List<BookingOverview>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("uspShowBookingOverView", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bookings.Add(new BookingOverview
                            {
                                BookingNumber = reader.GetInt32(reader.GetOrdinal("BookingNumber")),
                                DateTimeStart = reader.GetDateTime(reader.GetOrdinal("DateTimeStart")),
                                DateTimeEnd = reader.GetDateTime(reader.GetOrdinal("DateTimeEnd")),
                                TeamName = reader.GetString(reader.GetOrdinal("TeamName")),
                                FieldNumbers = reader.GetString(reader.GetOrdinal("FieldNumbers")),
                                LockerRoomNumbers = reader.GetString(reader.GetOrdinal("LockerRoomNumbers"))
                            });
                        }
                    }
                }
            }

            return bookings;
        }
