using System.Text.Json;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


async Task HandleRequest(HttpContext context)
{
    if (context.Request.Path == "/api/signIn")
    {
        context.Response.StatusCode = 200;
        Answer answer = new Answer();

        await context.Response.WriteAsJsonAsync(answer);
    }

    else if (context.Request.Path == "/api/about")
    {
        context.Response.StatusCode = 200;
        Answer answer = new Answer();

        await context.Response.WriteAsJsonAsync(answer);
    }

    else
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.SendFileAsync("html/html/GuestScreen.html");
    }
}


app.UseStaticFiles();

app.Run(HandleRequest);
app.Run();


class Answer
{
    public string Name { get; set; } = "";
}

