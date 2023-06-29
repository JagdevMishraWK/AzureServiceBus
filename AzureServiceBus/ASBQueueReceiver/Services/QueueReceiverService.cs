using ASBQueueReceiver.Models;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace ASBQueueReceiver.Services
{
    public class QueueReceiverService: IQueueReceiverService
    {
        private readonly ILogger<QueueReceiverService> _logger;
        private readonly ServiceBusClient _serviceBusClient;
        public QueueReceiverService(ILogger<QueueReceiverService> logger, ServiceBusClient serviceBusClient)
        {
            _logger = logger;
            _serviceBusClient = serviceBusClient;
        }
        
        public async Task SubscribeAsync(string queueName, CancellationToken stoppingToken) 
        {
            var processor = _serviceBusClient.CreateProcessor(queueName,
                new ServiceBusProcessorOptions { AutoCompleteMessages = false });
            
            // configure the message and error handler to use
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ProcessErrorAsync;
            
            await processor.StartProcessingAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }

            _logger.LogInformation("Closing message pump");
            await processor.CloseAsync(stoppingToken);
            _logger.LogInformation("Message pump closed : {Time}", DateTimeOffset.UtcNow);
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            var data= JsonConvert.DeserializeObject<QueueMessage>(body);
            Console.WriteLine(body);

            _logger.LogInformation("Handler completed successfull. Acknowledging the message was processed.");
            // we can evaluate application logic and use that to determine how to settle the message.
            await args.CompleteMessageAsync(args.Message);

        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Message handler encountered an exception");
            _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
            _logger.LogDebug($"- Entity Path: {arg.EntityPath}");
            _logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");

            return Task.CompletedTask;
        }

    }
}
