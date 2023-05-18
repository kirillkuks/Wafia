using Npgsql;
using WAFIA.Database.Types;

namespace WAFIA.Database.Connectors
{
    public class GeographyConnector
    {
        public GeographyConnector(NpgsqlConnector connector)
        {
            cmd = connector.Cmd;
        }

        private readonly NpgsqlCommand cmd;

        public async Task<List<Country>> GetCountries()
        {
            cmd.CommandText = "SELECT id, name FROM country";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            var result = new List<Country>();
            try
            {
                while (await reader.ReadAsync())
                {
                    Country country = new
                    (
                        (long)reader["id"],
                        (string)reader["name"]
                    );
                    result.Add(country);
                }
                reader.Close();
                return result;
            }
            catch
            {
                reader.Close();
                return result;
            }
        }
        public async Task<Country?> GetCountry(long id) {
            cmd.CommandText = $"SELECT name FROM country WHERE id = '{id}'";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            try {
                if (await reader.ReadAsync()) {
                    var country = new Country(
                        id,
                        (string)reader["name"]);
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
        public async Task<bool> AddCountry(string name)
        {
            cmd.CommandText = $"INSERT INTO country (name) VALUES (@Name)";
            cmd.Parameters.AddWithValue("Name", name);
            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
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
        public async Task<List<City>> GetCities(Country country)
        {
            cmd.CommandText = $"SELECT id, name FROM city WHERE country = '{country.Id}'";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            var result = new List<City>();
            try
            {
                while (await reader.ReadAsync())
                {
                    City city = new
                    (
                        (long)reader["id"],
                        (string)reader["name"],
                        country.Id
                    );
                    result.Add(city);
                }
                reader.Close();
                return result;
            }
            catch
            {
                reader.Close();
                return result;
            }
        }
        public async Task<City?> GetCity(long id) {
            cmd.CommandText = $"SELECT name, country FROM city WHERE id = '{id}'";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            try {
                if (await reader.ReadAsync()) {
                    var city = new City(
                        id,
                        (string)reader["name"],
                        (long)reader["country"]);
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
        public async Task<bool> AddCity(Country country, string name)
        {
            cmd.CommandText = $"INSERT INTO city (name, country) VALUES (@Name, @Country)";
            cmd.Parameters.AddWithValue("Name", name);
            cmd.Parameters.AddWithValue("Country", country.Id);
            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
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
