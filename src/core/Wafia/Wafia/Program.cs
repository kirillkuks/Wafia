using WAFIA.Database;
using WAFIA.Database.Connectors;

System.Collections.Generic.SortedDictionary<string, int> activeUsers = new();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.Cookie.Name = "Wafia.Session";
    options.Cookie.IsEssential = true;
}
);

var app = builder.Build();

NpgsqlConnector? nc;
AccountConnector? ac = null;
try {
    if (!ConnParser.Parse("./connection_data.txt", out string? host,
        out string? name, out string? password, out string? baseName)) {
        nc = null;
    }
    else {
        nc = new(host, name, password, baseName);
    }
}
catch (Exception ex) {
    Console.WriteLine(ex.ToString());
    nc = null;
}
if (nc != null) {
    ac = new(nc);
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
            if (ac == null) {
                innerFail = true;
            }
            else {
                var res = await ac.GetIdRights(loginData.Login, loginData.Password);
                if (res != null) {
                    context.Session.SetString("userId", res.Value.Key.ToString());
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

