using Npgsql;

namespace WAFIA.Database {
    public class InfrastructureConnector {
        public InfrastructureConnector(NpgsqlConnector connector) {
            cmd = connector.Cmd;
        }

        private readonly NpgsqlCommand cmd;

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
    }
}
