using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;

namespace ASBQueueSender.Service
{
    public class QueueSenderService<T> : IQueueSenderService<T> where T : class
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string _queueName= string.Empty;
        public QueueSenderService(ServiceBusClient serviceBusClient, IConfiguration configuration)
        {
            _serviceBusClient = serviceBusClient;
            _queueName = configuration.GetSection("ServiceBus").GetValue<string>("QueueName");
        }
        public async Task<string> Send(T command, string correlationId, Action<IDictionary<string, object>>? setAppProperties = null)
        {
            var messageId = Guid.NewGuid().ToString();
            var sender = _serviceBusClient.CreateSender(_queueName);
            var body = JsonConvert.SerializeObject(command);
            var message = new ServiceBusMessage(body)
            {
                CorrelationId = correlationId,
                MessageId = messageId
            };

            setAppProperties?.Invoke(message.ApplicationProperties);

            await sender.SendMessagesAsync(new[] { message });

            return messageId;
        }
    }
}
