using MySql.Data.MySqlClient;
using System.Data;

namespace technicaltest.Config.MySql
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            try
            {
                var connection = new MySqlConnection(_connectionString);
                //await connection.OpenAsync();
                return connection;
            }
            catch
            {
                throw;
            }
        }
    }
}
