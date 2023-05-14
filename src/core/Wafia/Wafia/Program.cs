using WAFIA.Database;

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

    else if (context.Request.Path == "/api/get_user_rights") {
        context.Response.StatusCode = 200;
        var userId = context.Session.GetInt32("userId");

        if (context.Session.Keys.Contains("userId")) {
            await context.Response.WriteAsJsonAsync(new { state = "user" });
        }
        else {
            await context.Response.WriteAsJsonAsync(new { state = "guest" });
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

class LoginData {
    public LoginData(string login, string password) {
        Login = login;
        Password = password;
    }

    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
}

