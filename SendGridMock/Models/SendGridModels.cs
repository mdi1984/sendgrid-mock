using System.Text.Json.Serialization;

namespace SendGridMock.Models;

public class SendGridMessage
{
    [JsonPropertyName("from")]
    public EmailAddress? From { get; set; }

    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    [JsonPropertyName("personalizations")]
    public List<Personalization>? Personalizations { get; set; }

    [JsonPropertyName("content")]
    public List<Content>? Contents { get; set; }

    [JsonPropertyName("attachments")]
    public List<Attachment>? Attachments { get; set; }

    [JsonPropertyName("template_id")]
    public string? TemplateId { get; set; }

    [JsonPropertyName("reply_to")]
    public EmailAddress? ReplyTo { get; set; }

    [JsonPropertyName("send_at")]
    public long? SendAt { get; set; }

    [JsonPropertyName("headers")]
    public Dictionary<string, string>? Headers { get; set; }

    [JsonPropertyName("custom_args")]
    public Dictionary<string, string>? CustomArgs { get; set; }

    [JsonPropertyName("categories")]
    public List<string>? Categories { get; set; }

    [JsonPropertyName("asm")]
    public object? Asm { get; set; }

    [JsonPropertyName("mail_settings")]
    public object? MailSettings { get; set; }

    [JsonPropertyName("tracking_settings")]
    public object? TrackingSettings { get; set; }
}

public class EmailAddress
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    public EmailAddress() { }
    public EmailAddress(string email, string? name = null)
    {
        Email = email;
        Name = name;
    }
}

public class Personalization
{
    [JsonPropertyName("to")]
    public List<EmailAddress>? Tos { get; set; }

    [JsonPropertyName("cc")]
    public List<EmailAddress>? Ccs { get; set; }

    [JsonPropertyName("bcc")]
    public List<EmailAddress>? Bccs { get; set; }

    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    [JsonPropertyName("headers")]
    public Dictionary<string, string>? Headers { get; set; }

    [JsonPropertyName("substitutions")]
    public Dictionary<string, string>? Substitutions { get; set; }

    [JsonPropertyName("dynamic_template_data")]
    public object? DynamicTemplateData { get; set; }
}

public class Content
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}

public class Attachment
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("filename")]
    public string? Filename { get; set; }

    [JsonPropertyName("disposition")]
    public string? Disposition { get; set; }

    [JsonPropertyName("content_id")]
    public string? ContentId { get; set; }
}
