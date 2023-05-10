System.Collections.Generic.SortedDictionary<string, int> activeUsers = new();
List<LoginData> users = new();
users.Add(new LoginData("admin", "admin"));
users.Add(new LoginData("Xst", "Xst"));
users.Add(new LoginData("Tais", "Tais"));
users.Add(new LoginData("kirillkuks", "kirillkuks"));



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
    {
        options.Cookie.Name = "Wafia.Session";
        options.Cookie.IsEssential = true;
    }
);

var app = builder.Build();


async Task HandleRequest(HttpContext context)
{
    if (context.Request.Path == "/api/signIn")
    {
        context.Response.StatusCode = 200;
        RedirectResponse answer = new RedirectResponse();
        answer.Link = "/signIn";

        await context.Response.WriteAsJsonAsync(answer);
    }

    if (context.Request.Path == "/signIn")
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync("wwwroot/PersonalArea.html");
    }

    else if (context.Request.Path == "/api/login")
    {
        var loginData = await context.Request.ReadFromJsonAsync<LoginData>();
        bool userFound = false;

        if (loginData != null)
        {
            for (int i = 0; i < users.Count && !userFound; ++i)
            {
                if (loginData.Login == users[i].Login && loginData.Password == users[i].Password)
                {
                    context.Session.SetInt32("userId", i);
                    userFound = true;
                }
            }
        }

        if (userFound)
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(new { message = "OK" });
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { message = "Invalid Login or password" });
        }
    }

    else if (context.Request.Path == "/api/get_user_rights")
    {
        context.Response.StatusCode = 200;

        if (context.Session.Keys.Contains("userId"))
        {
            await context.Response.WriteAsJsonAsync(new { state = "user" });
        }
        else
        {
            await context.Response.WriteAsJsonAsync(new { state = "guest" });
        }
    }

    else if (context.Request.Path == "/api/about")
    {
        context.Response.StatusCode = 200;
        RedirectResponse answer = new RedirectResponse();
        answer.Link = "/about";

        await context.Response.WriteAsJsonAsync(answer);
    }

    else if (context.Request.Path == "/about")
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync("wwwroot/About.html");
    }

    else if (context.Request.Path == "/api/search")
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync("<h2>QWEQWE</h2>");
    }

    else
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync("wwwroot/GuestScreen.html");
    }
}


app.UseStaticFiles();
app.UseSession();


app.Run(HandleRequest);
app.Run();


class RedirectResponse
{
    public string Link { get; set; } = "";
}

class LoginData
{
    public LoginData(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
}

