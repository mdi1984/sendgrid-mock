using System.Collections.Concurrent;
using SendGridMock.Models;

namespace SendGridMock.Services;

public class InMemoryMailStorage : IMailStorage
{
    private readonly ConcurrentBag<StoredMessage> _messages = new();

    public Task<string> StoreAsync(SendGridMessage message)
    {
        var storedMessage = new StoredMessage
        {
            Message = message,
            ReceivedAt = DateTime.UtcNow
        };
        _messages.Add(storedMessage);
        return Task.FromResult(storedMessage.Id);
    }

    public Task<IEnumerable<StoredMessage>> GetAllAsync()
    {
        // Return latest first
        return Task.FromResult(_messages.OrderByDescending(m => m.ReceivedAt).AsEnumerable());
    }

    public Task ClearAsync()
    {
        _messages.Clear();
        return Task.CompletedTask;
    }
}
