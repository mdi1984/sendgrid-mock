using System.Text.Json;
using SendGridMock.Models;

namespace SendGridMock.Services;

public class FileMailStorage : IMailStorage
{
    private readonly string _storagePath;
    private readonly AppJsonSerializerContext _jsonContext;

    public FileMailStorage(IConfiguration configuration)
    {
        _storagePath = configuration.GetValue<string>("MailStoragePath")!;
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
        
        var jsonOptions = new JsonSerializerOptions 
        { 
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        _jsonContext = new AppJsonSerializerContext(jsonOptions);
    }

    public async Task<string> StoreAsync(SendGridMessage message)
    {
        var storedMessage = new StoredMessage
        {
            Message = message,
            ReceivedAt = DateTime.UtcNow
        };
        
        var filePath = Path.Combine(_storagePath, $"{storedMessage.Id}.json");
        var json = JsonSerializer.Serialize(storedMessage, _jsonContext.StoredMessage);
        await File.WriteAllTextAsync(filePath, json);

        return storedMessage.Id;
    }

    public async Task<IEnumerable<StoredMessage>> GetAllAsync()
    {
        var dir = new DirectoryInfo(_storagePath);
        var files = dir.GetFiles("*.json");
        var messages = new List<StoredMessage>();

        foreach (var file in files)
        {
            try 
            {
                using var stream = file.OpenRead();
                var message = await JsonSerializer.DeserializeAsync(stream, _jsonContext.StoredMessage);
                if (message != null)
                {
                    messages.Add(message);
                }
            }
            catch 
            {
                // Prepare for partial failures or corrupted files
                continue;
            }
        }

        return messages.OrderByDescending(m => m.ReceivedAt);
    }

    public Task ClearAsync()
    {
        var dir = new DirectoryInfo(_storagePath);
        foreach (var file in dir.GetFiles("*.json"))
        {
            try
            {
                file.Delete();
            }
            catch
            {
                // Ignore errors during deletion
            }
        }
        return Task.CompletedTask;
    }
}
