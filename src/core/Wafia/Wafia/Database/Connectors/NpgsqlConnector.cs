using Npgsql;
using WAFIA.Database.Tools;

namespace WAFIA.Database.Connectors
{
    public class NpgsqlConnector
    {
        public NpgsqlCommand Cmd { get; }
        public NpgsqlConnector(Connection connection)
        {
            string connString = $"Host={connection.Host};Username={connection.Name};" +
                                $"Password={connection.Password};Database={connection.BaseName}";
            NpgsqlConnection nc = new(connString);
            try
            {
                nc.Open();
            }
            catch
            {
                throw;
            }
            Cmd = new()
            {
                Connection = nc
            };
        }
        ~NpgsqlConnector()
        {
            Cmd.Dispose();
        }
    }
}