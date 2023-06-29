using ASBQueueReceiver.Models;

namespace ASBQueueReceiver.Services
{
    public interface IQueueReceiverService
    {
        Task SubscribeAsync(string queueName, CancellationToken stoppingToken);
    }
}
