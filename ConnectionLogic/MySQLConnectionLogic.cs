using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Project.ConnectionLogic
{
    public class MySQLConnectionLogic
    {
        private readonly string _connectionString;

        public MySQLConnectionLogic(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<DataTable> DQLSingleLineQuery(string sqlQuery, MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddRange(parameters);
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }


        public async Task<bool> DMLSingleLineQuery(string sqlQuery, params MySqlParameter[] parameters)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(sqlQuery, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                throw;
            }
        }


        public async Task<string> DQLSingleLineQueryForString(string sqlQuery)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(sqlQuery, connection))
                {
                    var result = await command.ExecuteScalarAsync();
                    return result?.ToString();
                }
            }
        }

    }
}
