using System;
using System.Security.Cryptography;
using System.Text;

namespace SendGridMock.Models;

public class StoredMessage
{
    public string Id { get; set; } = GenerateMessageId();
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public SendGridMessage Message { get; set; } = default!;

    private static string GenerateMessageId()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var sb = new StringBuilder(22);
        
        // Use RandomNumberGenerator for cryptographically strong random values
        // or Random.Shared for performance if crypto strength isn't strictly required for mocking.
        // SendGrid IDs are opaque, but usually look random.
        
        for (int i = 0; i < 22; i++)
        {
            sb.Append(chars[RandomNumberGenerator.GetInt32(chars.Length)]);
        }

        return sb.ToString();
    }
}
