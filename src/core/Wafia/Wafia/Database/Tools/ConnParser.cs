namespace WAFIA.Database.Tools {
    internal static class ConnParser {
        internal static Connection? Parse(string path) {
            string? host;
            string? name;
            string? password;
            string? baseName;
            try {
                StreamReader sr = new(path);
                host = sr.ReadLine();
                name = sr.ReadLine();
                password = sr.ReadLine();
                baseName = sr.ReadLine();
            }
            catch {
                host = null;
                name = null;
                password = null;
                baseName = null;
            }

            if (host == null || name == null || password == null || baseName == null) {
                return null;
            }
            Connection conn = new(host, name, password, baseName);
            return conn;
        }
    }
}
