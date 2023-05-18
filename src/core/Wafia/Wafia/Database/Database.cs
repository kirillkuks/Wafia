using WAFIA.Database.Connectors;
using WAFIA.Database.Tools;

namespace WAFIA.Database {
    public class Database {
        private static readonly List<Database> currConnections = new();
        public static Database? Create(string path) {
            Connection? connection = ConnParser.Parse(path);
            if (connection == null) {
                return null;
            }

            foreach (var conn in currConnections) {
                if (conn.Connection == connection) {
                    return conn;
                }
            }

            try {
                Database database = new(connection);
                currConnections.Add(database);
                return database;
            }
            catch {
                return null;
            }
        }
        private readonly NpgsqlConnector? nc;
        public AccountConnector AC { get; }
        public GeographyConnector GC { get; }
        public RequestConnector RC { get; }
        public Connection Connection { get; }
        public Database(Connection connection) {
            Connection = connection;
            try {
                nc = new(connection);
                AC = new(nc);
                GC = new(nc);
                RC = new(nc);
            }
            catch {
                throw;
            }
        }
        public static bool operator ==(Database? first, Database? second) {
            if (ReferenceEquals(first, second)) {
                return true;
            }
            if (first is null || second is null) {
                return false;
            }
            return first.Connection == second.Connection;
        }
        public static bool operator !=(Database? first, Database? second) {
            return !(first == second);
        }
    }
}