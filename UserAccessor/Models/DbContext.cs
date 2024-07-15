using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace UserAccessor.Models
{
    public class DbContext
    {
        private readonly string connectionString;
        private readonly MySqlConnection connection;

        public DbContext(IOptions<DbSettings> options) {
            connectionString = options.Value.ConnectionString;
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        public async void RunCommand(string command)
        {
            var cmd = new MySqlCommand(command, connection);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<DbDataReader> RunQuery(string query)
        {
            var cmd = new MySqlCommand(query, connection);
            return await cmd.ExecuteReaderAsync();
        }
    }
}
