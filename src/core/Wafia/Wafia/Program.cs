using WAFIA.Algorithms;
using WAFIA.Database;
using WAFIA.Database.Types;
using WAFIA.Loader;

System.Collections.Generic.SortedDictionary<string, int> activeUsers = new();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.Cookie.Name = "Wafia.Session";
    options.Cookie.IsEssential = true;
}
);

var app = builder.Build();

Database? db = Database.Create("./connection_data.txt");
if (db == null) {
    Console.WriteLine("Incorrect connection data, database anactive");
}

//var countries = await OverpassLoader.LoadCountries();
//foreach(var country in countries) {
//    await db.GC.AddCountry(country);
//}
//countries = await db.GC.GetCountries();

//foreach (var country in countries) {
//    var cities = await OverpassLoader.LoadCities(country);
//    foreach (var city in cities) {
//        await db.GC.AddCity(city);
//    }
//}

//var res = await db.IC.AddInfrElement(InfrastructureElement.FireStation.ToString());
//res = await db.IC.AddInfrElement(InfrastructureElement.School.ToString());
//res = await db.IC.AddInfrElement(InfrastructureElement.Police.ToString());
//res = await db.IC.AddInfrElement(InfrastructureElement.Mall.ToString());
//res = await db.IC.AddInfrElement(InfrastructureElement.Subway.ToString());
//res = await db.IC.AddInfrElement(InfrastructureElement.Healthcare.ToString());
//res = await db.IC.AddInfrElement(InfrastructureElement.University.ToString());
//res = await db.IC.AddInfrElement(InfrastructureElement.PlaceOfWorship.ToString());

//var spb = await db.GC.GetCity(507);
//var objs = await OverpassLoader.LoadInfrObjects(spb, InfrastructureElement.Healthcare);
//foreach (var obj in objs) {
//    await db.IC.AddObject(obj);
//}

async Task HandleRequest(HttpContext context) {
    if (context.Request.Path == "/api/signIn") {
        context.Response.StatusCode = 200;
        RedirectResponse answer = new() {
            Link = "/signIn"
        };

        await context.Response.WriteAsJsonAsync(answer);
    }

    if (context.Request.Path == "/signIn") {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync("wwwroot/PersonalArea.html");
    }

    else if (context.Request.Path == "/api/login") {
        var loginData = await context.Request.ReadFromJsonAsync<AccountJs>();
        bool userFound = false;
        bool innerFail = false;

        if (loginData != null) {
            if (db == null) {
                innerFail = true;
            }
            else {
                var res = await db.AC.Get(loginData.Login);
                if (res != null && res.Password == loginData.Password) {
                    context.Session.SetString("userId", res.Id.ToString());
                    userFound = true;
                }
            }
        }

        if (userFound) {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(new { message = "OK" });
        }
        else if (innerFail) {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { message = "Internal Server Error" });
        }
        else {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { message = "Invalid Login or password" });
        }
    }

    else if (context.Request.Path == "/api/signup") {
        var loginData = await context.Request.ReadFromJsonAsync<AccountJs>();
        bool userFound = false;

        if (loginData != null) {
            var acc = await db.AC.Get(loginData.Login);
            if (acc != null) {
                userFound = true;
            }

            if (!userFound) {
                await db.AC.Add(new(0, loginData.Login, loginData.Login, loginData.Password, false)); //
                context.Response.StatusCode = 200;
                await context.Response.WriteAsJsonAsync(new { message = "ok" });
                acc = await db.AC.Get(loginData.Login);
                context.Session.SetString("userId", acc.Id.ToString());
            }
            else {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { message = "user is already exist" });
            }
        }
        else {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { message = "invalid login or password" });
        }
    }

    else if (context.Request.Path == "/api/logout") {
        context.Response.StatusCode = 200;
        var userIdString = context.Session.GetString("userId");

        if (userIdString != null) {
            context.Session.Remove("userId");
        }

        await context.Response.WriteAsJsonAsync(new { message = "ok" });
    }

    else if (context.Request.Path == "/api/get_session_info") {
        context.Response.StatusCode = 200;
        var userIdString = context.Session.GetString("userId");

        var isOk = long.TryParse(userIdString, out long id);

        if (isOk) {

            Account? ac = await db.AC.Get(id);

            if (ac != null && context.Session.Keys.Contains("userId")) {
                await context.Response.WriteAsJsonAsync(new {
                    user_rights = ac.IsAdmin ? "admin" : "user",
                    user_login = ac.Login
                });
            }
            else {
                await context.Response.WriteAsJsonAsync(new {
                    user_rights = "guest",
                    user_login = "error"
                });
            }
        }
    }

    else if (context.Request.Path == "/api/about") {
        context.Response.StatusCode = 200;
        RedirectResponse answer = new() {
            Link = "/about"
        };

        await context.Response.WriteAsJsonAsync(answer);
    }

    else if (context.Request.Path == "/about") {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync("wwwroot/About.html");
    }

    else if (context.Request.Path == "/api/perform_request") {
        var reqJson = await context.Request.ReadFromJsonAsync<RequestJS>();
        if (reqJson != null) {
            List<Parameter> parameters = new();
            foreach (var paramJs in reqJson.Parameters) {
                var element = await db.IC.GetInfrElement(paramJs.Element);
                if (element == null) {
                    continue;
                }
                parameters.Add(new((InfrastructureElement)element, (Value)paramJs.Value));
            }
            var country = await db.GC.GetCountry(reqJson.Country);
            var acc = await db.AC.Get(reqJson.Account);
            if (country != null && acc != null) {
                Request req = new(0, acc.Id, DateTime.Now, country.Id, parameters);

                if (reqJson.City != null) {
                    var city = await db.GC.GetCity(reqJson.City, country);
                    if (city != null) {
                        req.City = city.Id;
                    }
                }

                if (reqJson.Border != null) {
                    req.Border = reqJson.Border;
                }

                var objs = await db.IC.Search(req);

                var res = GeoAlgorithms.FindZones(req, objs, out List<InfrastructureObject> outObj);

                List<ObjectJS> objectJsList = new();

                foreach (var obj in outObj)
                {
                    objectJsList.Add(new(obj.Name, obj.Coord));
                }

                context.Response.StatusCode = 200;
                await context.Response.WriteAsJsonAsync(new { result = res, objects = outObj });
            }
            else {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new { message = "cant find country with name" });
            }
        }
        else {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { message = "invalid request data" });
        }
    }

    else if (context.Request.Path == "/api/save_request") {
        var reqJson = await context.Request.ReadFromJsonAsync<RequestJS>();
        if (reqJson != null) {
            List<Parameter> parameters = new();
            foreach (var paramJs in reqJson.Parameters) {
                var element = await db.IC.GetInfrElement(paramJs.Element);
                if (element == null) {
                    continue;
                }
                parameters.Add(new((InfrastructureElement)element, (Value)paramJs.Value));
            }
            var country = await db.GC.GetCountry(reqJson.Country);
            var acc = await db.AC.Get(reqJson.Account);
            if (country != null && acc != null) {
                Request req = new(0, acc.Id, DateTime.Now, country.Id, parameters);

                if (reqJson.City != null) {
                    var city = await db.GC.GetCity(reqJson.City, country);
                    if (city != null) {
                        req.City = city.Id;
                    }
                }

                if (reqJson.Border != null) {
                    req.Border = reqJson.Border;
                }

                var res = await db.RC.Add(req);

                if (!res) {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new { message = "cant save request" });
                }
                else {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsJsonAsync(new { message = "ok" });
                }
            }
            else {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new { message = "cant find country with name" });
            }
        }
        else {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { message = "invalid request data" });
        }
    }

    else if (context.Request.Path == "/api/get_cities") {
        var countries = await db.GC.GetCountries();
        List<CountryJs> countryJsList = new();
        foreach (var country in countries) {
            var cities = await db.GC.GetCities(country);
            List<CityJs> cityJsList = new();
            foreach (var city in cities) {
                cityJsList.Add(new(city.Name, city.Center));
            }
            countryJsList.Add(new(country.Name, country.Center, cityJsList));
        }
        await context.Response.WriteAsJsonAsync(new { countries = countryJsList });
    }

    else if (context.Request.Path == "/api/get_elements") {
        var elems = await db.IC.GetInfrElements();
        await context.Response.WriteAsJsonAsync(new { elements = elems });
    }
        
    else {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync("wwwroot/GuestScreen.html");
    }
}


app.UseStaticFiles();
app.UseSession();


app.Run(HandleRequest);
app.Run();


class RedirectResponse {
    public string Link { get; set; } = "";
}
class AccountJs {
    public AccountJs(string login, string password, string mail) {
        Login = login;
        Mail = mail;
        Password = password;
    }

    public string Login { get; set; } = "";
    public string Mail { get; set; } = "";
    public string Password { get; set; } = "";
}
class CityJs {
    public CityJs(string name, Point center) {
        Name = name;
        Center = center;
    }
    public string Name { get; set; }
    public Point Center { get; set; }
}
class CountryJs {
    public CountryJs(string name, Point center, List<CityJs> cities) {
        Name = name;
        Center = center;
        Cities = cities;
    }
    public List<CityJs> Cities { get; set; }
    public string Name { get; set; }
    public Point Center { get; set; }
}

class ParameterJs {
    public string Element { get; set; }
    public long Value { get; set; }
    public ParameterJs(string element, long value) {
        Element = element;
        Value = value;
    }
}
class RequestJS {
    public string Account { get; set; }
    public List<ParameterJs> Parameters { get; set; }
    public List<Point>? Border { get; set; }
    public string Country { get; set; }
    public string? City { get; set; }
    public RequestJS(
        string account, 
        List<ParameterJs> parameters, 
        List<Point>? border, 
        string country, 
        string? city) {
        Account = account;
        Parameters = parameters;
        Border = border;
        Country = country;
        City = city;
    }
}

class ObjectJS {
    public string Name { get; set; }
    public Point Coord { get; set; }

    public ObjectJS(string name, Point coord) {
        Name = name; 
        Coord = coord;
    }
}