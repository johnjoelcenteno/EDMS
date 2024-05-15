using System.Text.Json;
using Azure.Storage.Queues;

namespace DPWH.EDMS.Infrastructure.Storage.Services;

public class NotificationQueueService : INotificationQueueService
{
    private readonly QueueServiceClient _queueServiceClient;
    public NotificationQueueService(string connectionString)
    {
        _queueServiceClient = new QueueServiceClient(connectionString);
    }

    public async Task<string> QueueMessage<T>(string queueName, T data, TimeSpan? visiblityDelay = null)
    {
        var queueClient = GetQueue(queueName);
        var message = Base64Encode(JsonSerializer.Serialize(data));
        var result = await queueClient.SendMessageAsync(message, visiblityDelay, null);

        return result.Value.MessageId;
    }

    private QueueClient GetQueue(string queueName)
    {
        var queueClient = _queueServiceClient.GetQueueClient(queueName);
        queueClient.CreateIfNotExists();
        return queueClient;
    }
    private static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }
}

public interface INotificationQueueService
{
    Task<string> QueueMessage<T>(string queueName, T data, TimeSpan? visibilityDelay = null);
}
