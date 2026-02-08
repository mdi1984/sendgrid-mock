using Microsoft.AspNetCore.Mvc;
using SendGridMock.Models;
using SendGridMock.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (string.IsNullOrWhiteSpace(builder.Configuration["MailStoragePath"]))
{
    builder.Services.AddSingleton<IMailStorage, InMemoryMailStorage>();
}
else
{
    builder.Services.AddSingleton<IMailStorage, FileMailStorage>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseFileServer();

app.MapPost("/v3/mail/send", async (HttpContext context, [FromBody] SendGridMessage message, IMailStorage storage, IConfiguration config) =>
{
    var apiKey = config.GetValue<string>("SendGridApiKey");
    if (!string.IsNullOrEmpty(apiKey))
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ") || authHeader.Substring(7).Trim() != apiKey)
        {
            return Results.Unauthorized();
        }
    }

    if (message == null)
    {
        return Results.BadRequest("Invalid message format.");
    }
    
    // Basic validation (optional, can be expanded)
    if (message.From == null)
    {
        message.From = new EmailAddress("no-reply@example.com", "No Reply"); // Fallback or error? SendGrid requires From.
        // real SendGrid returns 400 if From is missing.
        // return Results.BadRequest(new { errors = new[] { new { message = "The from email is required.", field = "from.email", help = "http://sendgrid.com/docs/API_Reference/Web_API_v3/Mail/errors.html#-From-Email-Required" } } });
    }

    var messageId = await storage.StoreAsync(message);
    context.Response.Headers.Append("x-message-id", messageId);
    return Results.Accepted();
});

app.MapGet("/api/mails", async (IMailStorage storage) =>
{
    var mails = await storage.GetAllAsync();
    return Results.Ok(mails);
});

app.MapDelete("/api/mails", async (IMailStorage storage) =>
{
    await storage.ClearAsync();
    return Results.Ok();
});

app.Run();

public partial class Program { }
