namespace SendGridMock.Models;

public class StoredMessage
{
    public string Id { get; set; } = GenerateMessageId();
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public SendGridMessage Message { get; set; } = default!;

    private static string GenerateMessageId()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        // Use Random.Shared - crypto strength isn't needed for mock message IDs
        return string.Create(22, chars, static (span, chars) =>
        {
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = chars[Random.Shared.Next(chars.Length)];
            }
        });
    }
}
