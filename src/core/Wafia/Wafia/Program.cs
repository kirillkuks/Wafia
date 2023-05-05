using System.Text.Json;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
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

        if (loginData != null && loginData.Login == "admin" && loginData.Password == "admin")
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

app.Run(HandleRequest);
app.Run();


class RedirectResponse
{
    public string Link { get; set; } = "";
}

class LoginData
{
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
}

