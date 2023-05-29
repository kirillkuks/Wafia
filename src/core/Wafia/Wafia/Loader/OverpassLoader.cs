using NpgsqlTypes;
using System.Globalization;
using WAFIA.Database.Types;

namespace WAFIA.Loader
{
    using OQBuilder = OverpassQueryBuilder;
    static public class OverpassLoader {

        static readonly HttpClient? httpClient;

        static OverpassLoader() {
            var socketsHandler = new SocketsHttpHandler {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2)
            };
            httpClient = new HttpClient(socketsHandler);
        }

        static public async Task<List<InfrastructureObject>> LoadInfrObjects(City city, InfrastructureElement InfrElement) {
            List<InfrastructureObject> objs = new();

            if (httpClient == null) {
                return objs;
            }

            var response = await httpClient.GetAsync(OQBuilder.GetQueryName(city, InfrElement));
            List<string?> names = new();
            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                var namesStr = ParseStringTable(content);
                if (namesStr.Count == 0) {
                    return objs;
                }
                else {
                    for (int i = 0; i < namesStr.Count; ++i) {
                        names.Add(namesStr[i]);
                    }
                }
            }
            response.Dispose();

            response = await httpClient.GetAsync(OQBuilder.GetQueryWebsite(city, InfrElement));
            List<string?> websites = new();
            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                var websitesStr = ParseStringTable(content);
                for (int i = 0; i < websitesStr.Count; ++i) {
                    websites.Add(websitesStr[i]);
                }
            }
            response.Dispose();

            response = await httpClient.GetAsync(OQBuilder.GetQueryOpeningHours(city, InfrElement));
            List<string?> ohs = new();
            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                var ohsStr = ParseStringTable(content);
                for (int i = 0; i < ohsStr.Count; ++i) {
                    ohs.Add(ohsStr[i]);
                }
            }
            response.Dispose();

            response = await httpClient.GetAsync(OQBuilder.GetQueryPhone(city, InfrElement));
            List<string?> phones = new();
            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                var phonesStr = ParseStringTable(content);
                for (int i = 0; i < phonesStr.Count; ++i) {
                    phones.Add(phonesStr[i]);
                }
            }
            response.Dispose();

            response = await httpClient.GetAsync(OQBuilder.GetQueryStreet(city, InfrElement));
            List<string?> streets = new();
            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                var streetsStr = ParseStringTable(content);
                for (int i = 0; i < streetsStr.Count; ++i) {
                    streets.Add(streetsStr[i]);
                }
            }
            response.Dispose();

            response = await httpClient.GetAsync(OQBuilder.GetQueryNumber(city, InfrElement));
            List<string?> houses = new();
            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                var housesStr = ParseStringTable(content);
                for (int i = 0; i < housesStr.Count; ++i) {
                    houses.Add(housesStr[i]);
                }
            }
            response.Dispose();

            List<double?> lats = new(objs.Count);
            List<double?> lons = new(objs.Count);

            response = await httpClient.GetAsync(OQBuilder.GetQueryLat(city, InfrElement));

            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                lats = ParseDoubleTable(content);
            }
            response.Dispose();

            response = await httpClient.GetAsync(OQBuilder.GetQueryLon(city, InfrElement));

            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                lons = ParseDoubleTable(content);
            }
            response.Dispose();

            List<Point> coords = new();
            for (int i = 0; i < names.Count; ++i) {
                if (lats[i] == null || lons[i] == null) {
                    continue;
                }
                coords.Add(new(lats[i].Value, lons[i].Value));
            }

            for (int i = 0; i < names.Count; ++i) {
                if (names[i] == null) {
                    continue;
                }
                objs.Add(new(0, coords[i], InfrElement, names[i], city.Id)); /////
                objs[^1].House = houses[i];
                objs[^1].Street = streets[i];
                objs[^1].Website = websites[i];
                objs[^1].OpeningHours = ohs[i];
                objs[^1].Phone = phones[i]; 
            }
             
            return objs;
        }

        static public async Task<List<Country>> LoadCountries() {
            List<Country> countries = new();

            if (httpClient == null) {
                return countries;
            }

            var response = await httpClient.GetAsync(OQBuilder.GetCountryNameQuery());
            List<string?> names = new();
            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                var namesStr = ParseStringTable(content);
                if (namesStr.Count == 0) {
                    return countries;
                }
                else {
                    for (int i = 0; i < namesStr.Count; ++i) {
                        names.Add(namesStr[i]);
                    }
                }
            }
            response.Dispose();

            List<double?> lats = new(names.Count);
            List<double?> lons = new(names.Count);

            response = await httpClient.GetAsync(OQBuilder.GetCountryLatQuery());

            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                lats = ParseDoubleTable(content);
            }
            response.Dispose();

            response = await httpClient.GetAsync(OQBuilder.GetCountryLonQuery());

            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                lons = ParseDoubleTable(content);
            }
            response.Dispose();

            List<Point> coords = new();
            for (int i = 0; i < names.Count; ++i) {
                if (lats[i] == null || lons[i] == null) {
                    continue;
                }
                coords.Add(new((double)lats[i], (double)lons[i]));
            }

            for (int i = 0; i < names.Count; ++i) {
                if (names[i] == null) {
                    continue;
                }
                countries.Add(new(0, names[i], coords[i]));
            }

            return countries;
        }

        static public async Task<List<City>> LoadCities(Country country) {
            List<City> cities = new();

            if (httpClient == null) {
                return cities;
            }

            var response = await httpClient.GetAsync(OQBuilder.GetCityNameQuery(country));
            List<string?> names = new();
            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                var namesStr = ParseStringTable(content);
                if (namesStr.Count == 0) {
                    return cities;
                }
                else {
                    for (int i = 0; i < namesStr.Count; ++i) {
                        names.Add(namesStr[i]);
                    }
                }
            }
            response.Dispose();

            List<double?> lats = new(names.Count);
            List<double?> lons = new(names.Count);

            response = await httpClient.GetAsync(OQBuilder.GetCityLatQuery(country));

            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                lats = ParseDoubleTable(content);
            }
            response.Dispose();

            response = await httpClient.GetAsync(OQBuilder.GetCityLonQuery(country));

            if (!response.IsSuccessStatusCode) {
                Console.WriteLine(response.StatusCode);
            }
            else {
                string content = await response.Content.ReadAsStringAsync();
                lons = ParseDoubleTable(content);
            }
            response.Dispose();

            List<Point> coords = new();
            for (int i = 0; i < names.Count; ++i) {
                if (lats[i] == null || lons[i] == null) {
                    continue;
                }
                coords.Add(new((double)lats[i], (double)lons[i]));
            }

            for (int i = 0; i < names.Count; ++i) {
                if (names[i] == null) {
                    continue;
                }
                cities.Add(new(0, names[i], country.Id ,coords[i]));
            }

            return cities;
        }
        static private List<string?> ParseStringTable(string str) { 
            List<string?> result = new();

            using StringReader sr = new(str);
            sr.ReadLine();

            string? line = sr.ReadLine();
            while (line != null) {
                if (string.IsNullOrWhiteSpace(line)) {
                    result.Add(null);
                }
                else {
                    result.Add(line);
                }
                line = sr.ReadLine();
            }
            return result;
        }
        static private List<double?> ParseDoubleTable(string str) {
            List<double?> result = new();

            using StringReader sr = new(str);
            sr.ReadLine();

            string? line = sr.ReadLine();
            while (line != null) {
                if (double.TryParse(line, NumberStyles.Any, CultureInfo.InvariantCulture, out double fl)) {
                    result.Add(fl);
                }
                else {
                    result.Add(null);
                }
                line = sr.ReadLine();
            }
            return result;
        }
    }
}
