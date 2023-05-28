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
        var loginData = await context.Request.ReadFromJsonAsync<LoginData>();
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
        var loginData = await context.Request.ReadFromJsonAsync<LoginData>();
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

    else if (context.Request.Path == "/api/search") {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync("<h2>QWEQWE</h2>");
    }

    //else if (context.Request.Path == "/api/get_cities") {
    //    var countries = await db.GC.GetCountries();
    //    List<CountryJs> countryJsList = new();
    //    foreach (var country in countries) {
    //        var cities = await db.GC.GetCities(country);
    //        List<CityJs> cityJsList = new();
    //        foreach (var city in cities) {
    //            cityJsList.Add(new(city.Name, city.Center.X, city.Center.Y));
    //        }
    //        countryJsList.Add(new(country.Name, country.Center.X, country.Center.Y, cityJsList));
    //    }
    //    await context.Response.WriteAsJsonAsync(new { countries = countryJsList });
    //}

    else if (context.Request.Path == "/api/get_cities") {
        var countries = await db.GC.GetCountries();
        List<CountryJs> countryJsList = new();
        foreach (var country in countries) {
            var cities = await db.GC.GetCities(country);
            List<CityJs> cityJsList = new();
            foreach (var city in cities) {
                cityJsList.Add(new(city.Name, city.Center.X, city.Center.Y));
            }
            countryJsList.Add(new(country.Name, country.Center.X, country.Center.Y, cityJsList));
        }
        await context.Response.WriteAsJsonAsync(new { countries = countryJsList });
    }

    else if (context.Request.Path == "/api/get_elements") {
        var elems = new string[]{ InfrastructureElement.Healthcare.ToString(),
            InfrastructureElement.PlaceOfWorship.ToString(),
            InfrastructureElement.University.ToString(),
            InfrastructureElement.Subway.ToString(),
            InfrastructureElement.School.ToString(),
            InfrastructureElement.FireStation.ToString(),
            InfrastructureElement.Police.ToString(),
            InfrastructureElement.Mall.ToString()};
        await context.Response.WriteAsJsonAsync(new {
            elements = elems
        });
    }
        
    else {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync("wwwroot/GuestScreen5.html");
    }
}



app.UseStaticFiles();
app.UseSession();


app.Run(HandleRequest);
app.Run();


class RedirectResponse {
    public string Link { get; set; } = "";
}
class LoginData {
    public LoginData(string login, string password, string mail) {
        Login = login;
        Mail = mail;
        Password = password;
    }

    public string Login { get; set; } = "";
    public string Mail { get; set; } = "";
    public string Password { get; set; } = "";
}

class CityJs {
    public CityJs(string name, double lat, double lon) {
        Name = name;
        Lat = lat;
        Lon = lon;
    }
    public string Name { get; set; } = "";
    public double Lat { get; set; }
    public double Lon { get; set; }
}

class CountryJs {
    public CountryJs(string name, double lat, double lon, List<CityJs> cities) {
        Name = name;
        Lat = lat;
        Lon = lon;
        Cities = cities;
    }
    public List<CityJs> Cities { get; set; }
    public string Name { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
}