using Npgsql;
using NpgsqlTypes;
using WAFIA.Database.Types;

namespace WAFIA.Database.Connectors {
    public class InfrastructureConnector {
        public InfrastructureConnector(NpgsqlConnector connector) {
            cmd = connector.Cmd;
        }

        private readonly NpgsqlCommand cmd;

        public async Task<List<string>> GetInfrElements()
        {
            cmd.CommandText = "SELECT name FROM infrastructure_element";

            cmd.Connection?.Close();
            cmd.Connection?.Open();

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            var result = new List<string>();

            try
            {
                while (await reader.ReadAsync())
                {
                    result.Add((string)reader["name"]);
                }
                reader.Close();
                return result;
            }
            catch
            {
                reader.Close();
                return result; ;
            }
        }

        public async Task<long?> GetInfrElement(string name)
        {
            cmd.CommandText = $"SELECT id FROM infrastructure_element WHERE name = '{name}'";
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            try
            {
                if (await reader.ReadAsync())
                {
                    var id = (long)reader["id"];
                    reader.Close();
                    return id;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch
            {
                reader.Close();
                return null;
            }
        }

        public async Task<bool> AddInfrElement(string name) {
            cmd.CommandText = $"INSERT INTO infrastructure_element (name) VALUES (@Name)";
            cmd.Parameters.AddWithValue("Name", name);

            try {
                await cmd.ExecuteNonQueryAsync();
            }
            catch {
                cmd.Parameters.Clear();
                return false;
            }
            cmd.Parameters.Clear();

            cmd.CommandText = $"INSERT INTO infrastructure_element_value (value, infrastructure_element) " +
                $"SELECT value.id, infrastructure_element.id " +
                $"FROM (value " +
                $"CROSS JOIN " +
                $"(SELECT id FROM infrastructure_element WHERE name = '{name}') infrastructure_element)";

            try {
                await cmd.ExecuteNonQueryAsync();
            }
            catch {
                return false;
            }
            return true;
        }
        public async Task<bool> AddObject(InfrastructureObject obj) {
            string selectStart = "INSERT INTO infrastructure_object (coordinates, name, infrastructure_element, city";
            string selectEnd = " VALUES (@Coordinates, @Name, @InfrElement, @City";
            string bracket = ")";

            cmd.Parameters.AddWithValue("Coordinates", obj.Coord);
            cmd.Parameters.AddWithValue("Name", obj.Name);
            cmd.Parameters.AddWithValue("InfrElement", (long)obj.InfrElem);
            cmd.Parameters.AddWithValue("City", obj.City);

            string houseStart = "";
            string houseEnd = "";
            if (obj.House != null) {
                houseStart = ", house";
                houseEnd = ", @House";
                cmd.Parameters.AddWithValue("House", obj.House);
            }

            string streetStart = "";
            string streetEnd = "";
            if (obj.Street != null) {
                streetStart = ", street";
                streetEnd = ", @Street";
                cmd.Parameters.AddWithValue("Street", obj.Street);
            }

            string opHoursStart = "";
            string opHoursEnd = "";
            if (obj.OpeningHours != null) {
                opHoursStart = ", opening_hours";
                opHoursEnd = ", @OpHours";
                cmd.Parameters.AddWithValue("OpHours", obj.OpeningHours);
            }

            string phoneStart = "";
            string phoneEnd = "";
            if (obj.Phone != null) {
                phoneStart = ", phone";
                phoneEnd = ", @Phone";
                cmd.Parameters.AddWithValue("Phone", obj.Phone);
            }

            string websiteStart = "";
            string websiteEnd = "";
            if (obj.Website != null) {
                websiteStart = ", website";
                websiteEnd = ", @Website";
                cmd.Parameters.AddWithValue("Website", obj.Website);
            }

            cmd.CommandText = selectStart + houseStart + streetStart + websiteStart
                + phoneStart + opHoursStart + bracket + selectEnd
                + houseEnd + streetEnd + websiteEnd + phoneEnd + opHoursEnd
                + bracket;

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
        public async Task<InfrastructureObject?> GetObject(string name, Point coord) {
            NpgsqlPoint npgsqlPoint = new(coord.X, coord.Y);
            cmd.CommandText = "SELECT id, street, website, opening_hours, infrastructure_element, city, " +
                $"house, phone FROM infrastructure_object WHERE name='{name}' and coord='{npgsqlPoint}'";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            try {
                if (await reader.ReadAsync()) {
                    var obj = new InfrastructureObject(
                        (long)reader["id"],
                        coord,
                        (InfrastructureElement)(long)reader["infrastructure_element"],
                        name,
                        (long)reader["city"]) {
                        Phone = (string)reader["phone"],
                        Street = (string)reader["street"],
                        Website = (string)reader["website"],
                        OpeningHours = (string)reader["opening_hours"],
                        House = (string)reader["house"]
                    };
                    reader.Close();
                    return obj;
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
        public async Task<bool> DeleteObjects(City city) {
            cmd.CommandText = $"DELETE  FROM infrastracture_object WHERE city='{city.Id}'";
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
        public async Task<bool> DeleteObjects(Country country) {
            cmd.CommandText = $"DELETE  FROM infrastracture_object WHERE city IN" +
                $"(SELECT id FROM city WHERE country='{country.Id}')";
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
        public async Task<bool> DeleteObject(long id) {
            cmd.CommandText = $"DELETE  FROM infrastracture_object WHERE id='{id}'";
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
        public async Task<bool> UpdateObject(InfrastructureObject obj) {
            if (obj.Id == 0) {
                return false;
            }

            cmd.CommandText = $"UPDATE infrastracture_object" +
                $" SET name='{obj.Name}'," +
                $"street='{obj.Street}', " +
                $"website='{obj.Website}'" +
                $"opening_hours='{obj.OpeningHours}'" +
                $"infrastructure_element='{(long)obj.InfrElem}'" +
                $"house='{obj.House}'" +
                $"phone='{obj.Phone}'" +
                $"WHERE id = '{obj.Id}'";

            try {
                if (await cmd.ExecuteNonQueryAsync() != 0) {
                    return true;
                }
                return false;
            }
            catch {
                return false;
            }
        }
        public async Task<List<InfrastructureObject>> Search(Request request) {

            string cityIdText;
            if (request.City != null) {
                cityIdText = $"city = '{request.City}'";
            }
            else {
                cityIdText = $"city IN (SELECT id FROM city WHERE country = '{request.Country}')";
            }

            if (request.Border != null) {
                cmd.CommandText = $"SELECT id, coordinates, infrastructure_element, name, city FROM infrastructure_object " +
                    $"WHERE {cityIdText} AND st_within(coordinates::geometry, @Border::geometry)";
                cmd.Parameters.AddWithValue("Border", request.Border);
            }
            else {
                cmd.CommandText = $"SELECT id, coordinates, infrastructure_element, name, city FROM infrastructure_object " +
                    $"WHERE {cityIdText}";
            }

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            cmd.Parameters.Clear();

            List<InfrastructureObject> result = new();
            try {
                while (await reader.ReadAsync()) {
                    var npgsqlPoint = (NpgsqlPoint)reader["coordinates"];
                    Point point = new(npgsqlPoint.X, npgsqlPoint.Y);
                    InfrastructureObject ie = new
                    (
                        (long)reader["id"],
                        point,
                        (InfrastructureElement)(long)reader["infrastructure_element"],
                        (string)reader["name"],
                        (long)reader["city"]
                    );
                    result.Add(ie);
                }
                reader.Close();
                return result;
            }
            catch {
                reader.Close();
                return result;
            }
        }
    }
}