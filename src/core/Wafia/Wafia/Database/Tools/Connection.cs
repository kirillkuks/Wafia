namespace WAFIA.Database.Tools {
    public class Connection {
        public string Host { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string BaseName { get; set; }

        internal Connection(string host, string name, string password, string baseName) {
            Host = host;
            Name = name;
            Password = password;
            BaseName = baseName;
        }
        public static bool operator ==(Connection? first, Connection? second) {
            if (ReferenceEquals(first, second)) {
                return true;
            }
            if (first is null || second is null) {
                return false;
            }
            if (first.Host == second.Host &&
                first.Name == second.Name &&
                first.Password == second.Password &&
                first.BaseName == second.BaseName) {
                return true;
            }
            return false;
        }
        public static bool operator !=(Connection? first, Connection? second) {
            return !(first == second);
        }
    }
}