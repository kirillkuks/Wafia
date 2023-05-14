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

        internal async Task<List<City>> GetCities(Country country)
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
    }
}
