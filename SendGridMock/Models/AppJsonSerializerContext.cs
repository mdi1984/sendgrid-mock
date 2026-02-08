using System.Text.Json.Serialization;

namespace SendGridMock.Models;

[JsonSerializable(typeof(StoredMessage))]
[JsonSerializable(typeof(IEnumerable<StoredMessage>))]
[JsonSerializable(typeof(SendGridMessage))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}
