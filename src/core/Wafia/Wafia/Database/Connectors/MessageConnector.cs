using Npgsql;
using NpgsqlTypes;
using WAFIA.Database.Types;

namespace WAFIA.Database.Connectors {
    public class MessageConnector {
        public MessageConnector(NpgsqlConnector connector) {
            cmd = connector.Cmd;
        }

        private readonly NpgsqlCommand cmd;
    }
}