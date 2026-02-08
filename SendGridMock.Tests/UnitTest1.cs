using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGridMock.Services;
using System.Net;
using System.Reflection.Metadata;

namespace SendGridMock.Tests;

public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public UnitTest1(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IMailStorage>();
                services.AddSingleton<IMailStorage, InMemoryMailStorage>();
            });

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"SendGridApiKey", "test_api_key"},
                });
            });
        });
        _client = _factory.CreateClient();
    }

    private SendGridClient SendGridClient
    {
        get
        {
            var options = new SendGridClientOptions
            {
                ApiKey = "test_api_key",
            };
            return new SendGridClient(_client, options);
        }
    }

    [Fact]
    public async Task TestPlainText_WorksAsync()
    {
        var sender = new EmailAddress("sender@example.com", "Sender McSendFace");
        var recipient = new EmailAddress("recipient@example.com", "Recipient McReceiveFace");
        var recipient2 = new EmailAddress("recipient2@example.com", "Recipient2 McReceiveFace");
        var replyTo = new EmailAddress("noc-reply@example.com", "No Reply");
        var subject = "Test Email";
        var txtContent = "This is a test email.";
        var email = MailHelper.CreateSingleEmailToMultipleRecipients(
            sender,
            new List<EmailAddress> { recipient, recipient2 },
            subject,
            txtContent,
            null);

        var response = await SendGridClient.SendEmailAsync(email, CancellationToken.None);
        Assert.True(response.IsSuccessStatusCode, $"Expected success status code but got {response.StatusCode}");
        Assert.True(response.Headers.Contains("X-Message-ID"), "Response should contain X-Message-ID header");
    }

    [Fact]
    public async Task TestHtml_WorksAsync()
    {
        var sender = new EmailAddress("sender@example.com", "Sender McSendFace");
        var recipient = new EmailAddress("recipient@example.com", "Recipient McReceiveFace");
        var recipient2 = new EmailAddress("recipient2@example.com", "Recipient2 McReceiveFace");
        var subject = "Test Email";
        var htmlContent = "<strong>This is a test email.</strong>";
        var email = MailHelper.CreateSingleEmail(
            sender,
            recipient,
            subject,
            null,
            htmlContent);

        email.AddAttachment(
            "grumpy.jpg",
            "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");

        var response = await SendGridClient.SendEmailAsync(email, CancellationToken.None);
        Assert.True(response.IsSuccessStatusCode, $"Expected success status code but got {response.StatusCode}");
        Assert.True(response.Headers.Contains("X-Message-ID"), "Response should contain X-Message-ID header");
    }
}
