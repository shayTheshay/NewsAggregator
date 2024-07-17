using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace UserAccessor.Models
{

    public class DbContext : IDisposable
    {
        private readonly string _connectionString;

        public DbContext(IOptions<DbSettings> options)
        {
            _connectionString = options.Value.ConnectionString;
        }
        public async Task RunCommandAsync(string command, params MySqlParameter[] parameters)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var cmd = new MySqlCommand(command, connection);
            cmd.Parameters.AddRange(parameters);
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<T> RunQuerySingleAsync<T>(string query, params MySqlParameter[] parameters)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddRange(parameters);
            var result = await cmd.ExecuteScalarAsync();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public async Task<List<T>> RunQueryAsync<T>(string query, params MySqlParameter[] parameters)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddRange(parameters);
            using var reader = await cmd.ExecuteReaderAsync();
            var results = new List<T>();
            while (await reader.ReadAsync())
            {
                results.Add(reader.GetFieldValue<T>(0));
            }
            return results;
        }
        public void Dispose()
        {
            // No need to dispose of anything here anymore
        }
    }
}
