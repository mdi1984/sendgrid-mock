using SendGridMock.Models;

namespace SendGridMock.Services;

public interface IMailStorage
{
    Task<string> StoreAsync(SendGridMessage message);
    Task<IEnumerable<StoredMessage>> GetAllAsync();
    Task ClearAsync();
}
