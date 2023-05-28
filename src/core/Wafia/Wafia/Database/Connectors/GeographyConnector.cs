using Npgsql;
using NpgsqlTypes;
using WAFIA.Database.Types;

namespace WAFIA.Database.Connectors {
    using Point = NpgsqlPoint;
    public class GeographyConnector {
        public GeographyConnector(NpgsqlConnector connector) {
            cmd = connector.Cmd;
        }

        private readonly NpgsqlCommand cmd;

        public async Task<List<Country>> GetCountries() {
            cmd.CommandText = "SELECT id, name, center FROM country";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            var result = new List<Country>();
            try {
                while (await reader.ReadAsync()) {
                    Country country = new
                    (
                        (long)reader["id"],
                        (string)reader["name"],
                        (Point)reader["center"]
                    );
                    result.Add(country);
                }
                reader.Close();
                return result;
            }
            catch {
                reader.Close();
                return result;
            }
        }
        public async Task<Country?> GetCountry(long id) {
            cmd.CommandText = $"SELECT name, center FROM country WHERE id = '{id}'";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            try {
                if (await reader.ReadAsync()) {
                    var country = new Country(
                        id,
                        (string)reader["name"],
                        (Point)reader["center"]
                        );
                    reader.Close();
                    return country;
                }
                else {
                    reader.Close();
                    return null;
                }
            }
            catch {
                reader.Close();
                return null;
            }
        }
        public async Task<bool> AddCountry(Country country) {
            cmd.CommandText = $"INSERT INTO country (name, center) VALUES (@Name, @Center)";
            cmd.Parameters.AddWithValue("Name", country.Name);
            cmd.Parameters.AddWithValue("Center", country.Center);
            try {
                await cmd.ExecuteNonQueryAsync();
            }
            catch {
                cmd.Parameters.Clear();
                return false;
            }
            cmd.Parameters.Clear();
            return true;
        }
        public async Task<bool> DeleteCountry(string name) {
            cmd.CommandText = $"DELETE  FROM country WHERE name='{name}'";
            try {
                if (await cmd.ExecuteNonQueryAsync() != 0) {
                    return true;
                };
                return false;
            }
            catch {
                return false;
            }
        }
        public async Task<bool> DeleteCountry(long id) {
            cmd.CommandText = $"DELETE  FROM country WHERE id='{id}'";
            try {
                if (await cmd.ExecuteNonQueryAsync() != 0) {
                    return true;
                };
                return false;
            }
            catch {
                return false;
            }
        }
        public async Task<List<City>> GetCities(Country country) {
            cmd.CommandText = $"SELECT id, name, center FROM city WHERE country = '{country.Id}'";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            var result = new List<City>();
            try {
                while (await reader.ReadAsync()) {
                    City city = new
                    (
                        (long)reader["id"],
                        (string)reader["name"],
                        country.Id,
                        (Point)reader["center"]
                    );
                    result.Add(city);
                }
                reader.Close();
                return result;
            }
            catch {
                reader.Close();
                return result;
            }
        }
        public async Task<City?> GetCity(long id) {
            cmd.CommandText = $"SELECT name, country, center FROM city WHERE id = '{id}'";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            try {
                if (await reader.ReadAsync()) {
                    City city = new
                        (
                            id,
                            (string)reader["name"],
                            (long)reader["country"],
                            (Point)reader["center"]
                        );
                    reader.Close();
                    return city;
                }
                else {
                    reader.Close();
                    return null;
                }
            }
            catch {
                reader.Close();
                return null;
            }
        }
        public async Task<bool> AddCity(City city) {
            cmd.CommandText = $"INSERT INTO city (name, country, center) VALUES (@Name, @Country, @Center)";
            cmd.Parameters.AddWithValue("Name", city.Name);
            cmd.Parameters.AddWithValue("Center", city.Center);
            cmd.Parameters.AddWithValue("Country", city.Country);
            try {
                await cmd.ExecuteNonQueryAsync();
            }
            catch {
                cmd.Parameters.Clear();
                return false;
            }
            cmd.Parameters.Clear();
            return true;
        }
        public async Task<bool> DeleteCity(string name, Country country) {
            cmd.CommandText = $"DELETE  FROM city WHERE name='{name}' AND country='{country.Id}'";
            try {
                if (await cmd.ExecuteNonQueryAsync() != 0) {
                    return true;
                };
                return false;
            }
            catch {
                return false;
            }
        }
        public async Task<bool> DeleteCity(long id) {
            cmd.CommandText = $"DELETE  FROM city WHERE id='{id}'";
            try {
                if (await cmd.ExecuteNonQueryAsync() != 0) {
                    return true;
                };
                return false;
            }
            catch {
                return false;
            }
        }
    }
}