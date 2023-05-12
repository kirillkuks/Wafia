using NpgsqlTypes;
using WAFIA.Database;
using WAFIA.Database.Connectors;
using WAFIA.Database.Types;
using WAFIA.Loader;

namespace WAFIA
{
    using Point = NpgsqlPoint;
    using Polygon = NpgsqlPolygon;
    class Program {
        async static Task Main() {
            /*
            bool test_ac = false;
            bool test_gc = false;
            bool test_ic = false;
            bool test_loader = false;
            bool test_rc = true;
            NpgsqlConnector nc;
            try {
                if (!ConnParser.Parse("../../../connection_data.txt", out string? host,
                    out string? name, out string? password, out string? baseName)) {
                    return;
                }
                else {
                    nc = new(host, name, password, baseName);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return;
            }
            if (test_ac) {
                try {
                    AccountConnector ac = new(nc);
                    Account account = new(0, "kirillkuks", "kirillkuks@bruh.bruh", "1234", false);
                    var res1 = await ac.Add(account);

                    var res2 = await ac.GetIdRights("tay", "123");

                    var res3 = await ac.GetIdRights("kuks", "321");

                    var res4 = await ac.GetIdRights("tay", "1234");

                    var res5 = await ac.GetIdMail("kuks");

                    var res6 = await ac.GetIdMail("neKuks");

                    var res7 = await ac.ChangePassword(3, "1234");

                    Account change = new(5, "aleks", "", "mewmew", false);
                    Account badChange = new(0, "aleks", "", "mewmew", false);

                    var res8 = await ac.Edit(change);

                    var res9 = await ac.Edit(badChange);

                    var res10 = await ac.ChangeRights("kuks", true);

                    var res11 = await ac.ChangeRights("kuk", true);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }

            if (test_gc) {
                try {
                    GeographyConnector gc = new(nc);
                    var res1 = await gc.GetCountries();
                    var res2 = await gc.AddCountry("Россия");
                    var res3 = await gc.AddCountry("Япония");
                    var res4 = await gc.AddCountry("Япония");
                    var res5 = await gc.GetCountries();
                    var res6 = await gc.GetCities(res5[0]);
                    var res7 = await gc.AddCity(res5[0], "Санкт-Петербург");
                    var res8 = await gc.AddCity(res5[0], "Уфа");
                    var res9 = await gc.AddCity(res5[1], "Уфа");
                    var res10 = await gc.GetCities(res5[0]);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }

            if (test_ic) {
                try {
                    GeographyConnector gc = new(nc);
                    var countries = await gc.GetCountries();
                    var cities = await gc.GetCities(countries[0]);

                    InfrastructureConnector ic = new(nc);
                    
                    var res1 = await ic.AddInfrElement("Больницы");
                    var res2 = await ic.AddInfrElement("Больницы");
                    var res3 = await ic.AddInfrElement("Университеты");
                    var res4 = await ic.AddInfrElement("Церкви");

                    InfrastructureObject io = new(0, new(0.0, 0.0), InfrastructureElement.University, "something", cities[0].Id);
                    var res5 = await ic.AddObject(io);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }

            if (test_loader) {
                try {
                    GeographyConnector gc = new(nc);
                    InfrastructureConnector ic = new(nc);
                    var countries = await gc.GetCountries();
                    var cities = await gc.GetCities(countries[0]);

                    var ios = await OverpassLoader.LoadInfrObjects(cities[0], InfrastructureElement.University);
                    foreach (var io in ios) {
                        var res = await ic.AddObject(io);
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }

            if (test_rc) {
                try {
                    GeographyConnector gc = new(nc);
                    RequestConnector rc = new(nc);
                    var countries = await gc.GetCountries();
                    var cities = await gc.GetCities(countries[0]);
                    List<Parameter> parameters = new() {
                        new(InfrastructureElement.University, Value.Important),
                        new(InfrastructureElement.Healthcare, Value.VeryImportant)
                    };
                    Request req1 = new(0, 1, DateTime.Now, 1, parameters);
                    //System.Threading.Thread.Sleep(5000);
                    Request req2 = new(0, 1, DateTime.Now, 1, parameters);
                    Polygon border = new(new Point[]{ new Point(59.95417338478922, 30.162604838410676), 
                                                      new Point(59.95578021352399, 30.55075654498469),
                                                      new Point(60.10361442944917, 30.32951695817004)});
                    req2.City = 14; req2.Border = border;

                    var res1 = await rc.AddRequest(req1);
                    var res2 = await rc.AddRequest(req2);
                    var res3 = await rc.GetRequests(1);
                    var res4 = await rc.GetRequests(2);
                    var res5 = await rc.Search(req1);
                    var res6 = await rc.Search(req2);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            
            }
            */

            NpgsqlConnector nc;
            try
            {
                if (!ConnParser.Parse("../../../connection_data.txt", out string? host,
                    out string? name, out string? password, out string? baseName))
                {
                    return;
                }
                else
                {
                    nc = new(host, name, password, baseName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }

        }
    }
}
