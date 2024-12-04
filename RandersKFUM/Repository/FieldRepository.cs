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
    public class FieldRepository : IRepository<Field>
    {
        private readonly string _connectionString;

        public FieldRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Field> GetAll()
        {
            var fields = new List<Field>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspGetAllFields", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fields.Add(new Field
                            {
                                FieldId = Convert.ToInt32(reader["FieldId"]),
                                FieldType = reader["FieldType"]?.ToString(),
                                FieldNumber = reader["FieldNumber"] != DBNull.Value ? Convert.ToInt32(reader["FieldNumber"]) : 0
                            });

                        }
                    }
                }
            }

            return fields;
        }

        public Field GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspGetFieldById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@FieldId", SqlDbType.Int)).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Field
                            {
                                FieldId = Convert.ToInt32(reader["FieldId"]),
                                FieldType = reader["FieldType"]?.ToString(),
                                FieldNumber = reader["FieldNumber"] != DBNull.Value ? Convert.ToInt32(reader["FieldNumber"]) : 0
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Add(Field field)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspAddField", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@FieldType", SqlDbType.NVarChar, 50)).Value = field.FieldType;
                    command.Parameters.Add(new SqlParameter("@FieldNumber", SqlDbType.Int)).Value = field.FieldNumber;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Field field)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspUpdateField", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@FieldId", SqlDbType.Int)).Value = field.FieldId;
                    command.Parameters.Add(new SqlParameter("@FieldType", SqlDbType.NVarChar, 50)).Value = field.FieldType;
                    command.Parameters.Add(new SqlParameter("@FieldNumber", SqlDbType.Int)).Value = field.FieldNumber;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspDeleteField", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@FieldId", SqlDbType.Int)).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Field> GetAvailableFields(DateTime start, DateTime end)
        {
            var fields = new List<Field>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("uspGetAvailableFields", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NewBookingStart", start);
                    command.Parameters.AddWithValue("@NewBookingEnd", end);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fields.Add(new Field
                            {
                                FieldId = Convert.ToInt32(reader["FieldId"]),
                                FieldNumber = Convert.ToInt32(reader["FieldNumber"])
                            });
                        }
                    }
                }
            }
            return fields;
        }


    }

}
