namespace WAFIA.Database {
    public static class ConnParser {
        public static bool Parse(string path, out string? host, out string? name, out string? password, out string? baseName) {
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
                return false;
            }
            return true;
        }
    }
}
