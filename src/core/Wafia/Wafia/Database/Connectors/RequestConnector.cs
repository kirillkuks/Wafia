using Npgsql;
using NpgsqlTypes;
using WAFIA.Database.Types;

namespace WAFIA.Database.Connectors {
    public class RequestConnector {
        public RequestConnector(NpgsqlConnector connector) {
            cmd = connector.Cmd;
        }

        private readonly NpgsqlCommand cmd;

        public async Task<bool> Add(Request request) {
            if (request.Border == null && request.City == null) {
                cmd.CommandText = $"INSERT INTO request (account, date, country) VALUES (@Account, @Date, @Country)";
            }
            else if (request.Border == null) {
                cmd.CommandText = $"INSERT INTO request (account, date, country, city) " +
                    $"VALUES (@Account, @Date, @Country, @City)";
                cmd.Parameters.AddWithValue("City", request.City);
            }
            else if (request.City == null) {
                cmd.CommandText = $"INSERT INTO request (account, date, border, country) " +
                    $"VALUES (@Account, @Date, @Border, @Country)";
                cmd.Parameters.AddWithValue("Border", request.Border);
            }
            else {
                cmd.CommandText = $"INSERT INTO request (account, date, border, country, city) " +
                                    $"VALUES (@Account, @Date, @Border, @Country, @City)";
                cmd.Parameters.AddWithValue("City", request.City);
                cmd.Parameters.AddWithValue("Border", request.Border);
            }

            cmd.Parameters.AddWithValue("Date", request.Date);
            cmd.Parameters.AddWithValue("Account", request.Account);
            cmd.Parameters.AddWithValue("Country", request.Country);

            try {
                await cmd.ExecuteNonQueryAsync();
            }
            catch {
                cmd.Parameters.Clear();
                return false;
            }
            cmd.Parameters.Clear();

            cmd.CommandText = $"SELECT id, date FROM request WHERE account = '{request.Account}' ORDER BY date DESC";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            long requestId;

            try {
                await reader.ReadAsync();
                requestId = (long)reader["id"];
            }
            catch {
                reader.Close();
                cmd.Parameters.Clear();
                return false;
            }
            reader.Close();

            cmd.Parameters.AddWithValue("Request", requestId);

            foreach (var parameter in request.Parameters) {
                cmd.CommandText = $"INSERT INTO parameter (request, ie_value) " +
                    $"SELECT @Request, infrastructure_element_value.id FROM infrastructure_element_value " +
                    $"WHERE value = '{(long)parameter.Value}' " +
                    $"AND infrastructure_element = '{(long)parameter.InfrElement}'";

                try {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch {
                    cmd.Parameters.Clear();
                    return false;
                }
            }

            cmd.Parameters.Clear();
            return true;
        }
        public async Task<List<Request>> Get(long accountId) {
            cmd.CommandText = $"SELECT id, border, date, country, city FROM request WHERE account = '{accountId}'";

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            List<Request> requests = new();

            List<long> ids = new();
            List<DateTime> dates = new();
            List<long> countries = new();
            List<long?> cities = new();
            List<List<Point>?> borders = new();
            try {
                while (await reader.ReadAsync()) {
                    ids.Add((long)reader["id"]);
                    dates.Add((DateTime)reader["date"]);
                    var npgsqlPolygon = reader["border"] as NpgsqlPolygon?;
                    if (npgsqlPolygon == null)
                    {
                        borders.Add(null);
                    }
                    else
                    {
                        List<Point> polygon = new();
                        foreach (var point in npgsqlPolygon)
                        {
                            polygon.Add(new(point.X, point.Y));
                        }
                        borders.Add(polygon);
                    }
                    countries.Add((long)reader["country"]);
                    cities.Add(reader["city"] as long?);
                }
            }
            catch {
                reader.Close();
                return requests;
            }
            reader.Close();

            List<List<Parameter>> par = new();

            foreach (long id in ids) {
                cmd.CommandText = $"SELECT value, infrastructure_element FROM infrastructure_element_value" +
                    $" WHERE id IN (SELECT ie_value FROM parameter WHERE request = '{id}')";

                reader = await cmd.ExecuteReaderAsync();

                List<Parameter> parameters = new();
                try {
                    while (await reader.ReadAsync()) {
                        parameters.Add(
                            new((InfrastructureElement)(long)reader["infrastructure_element"],
                                (Value)(long)reader["value"])
                            );
                    }
                }
                catch {
                    reader.Close();
                    return requests;
                }
                par.Add(parameters);
                reader.Close();
            }

            for (int i = 0; i < ids.Count; ++i) {
                requests.Add(new(ids[i], accountId, dates[i], countries[i], par[i]));
                requests[i].City = cities[i];
                requests[i].Border = borders[i];
            }
            return requests;
        }
        public async Task<bool> Delete(long id) {
            cmd.CommandText = $"DELETE  FROM request WHERE id='{id}'";
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