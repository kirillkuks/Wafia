using Npgsql;
using NpgsqlTypes;

namespace WAFIA.Database {
    public class NpgsqlConnector {
        public NpgsqlCommand Cmd { get; }
        public NpgsqlConnector(string host, string username, string password, string database) {
            string connString = $"Host={host};Username={username};Password={password};Database={database}";
            NpgsqlConnection nc = new(connString);
            try {
                nc.Open();
            }
            catch {
                throw;
            }
            Cmd = new() {
                Connection = nc
            };
        }
        ~NpgsqlConnector() {
            Cmd.Dispose();
        }
    }
}