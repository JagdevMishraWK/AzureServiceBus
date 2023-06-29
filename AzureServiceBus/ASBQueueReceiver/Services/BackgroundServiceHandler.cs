using Microsoft.Extensions.Configuration;
using System.Threading;

namespace ASBQueueReceiver.Services
{
    public class BackgroundServiceHandler: BackgroundService
    {
        public readonly ILogger<BackgroundServiceHandler> _logger;
        public readonly IQueueReceiverService _messageConsumer;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _queueName;

        public BackgroundServiceHandler(ILogger<BackgroundServiceHandler> logger,
            IQueueReceiverService messageConsumer,
            IServiceProvider serviceProvider,IConfiguration configuration)
        {
            _logger = logger;
            _messageConsumer = messageConsumer;
            _serviceProvider = serviceProvider;
            _queueName = configuration.GetSection("ServiceBus").GetValue<string>("QueueName");
        }
        #region Simple run ticker
        //protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        _logger.LogInformation("Worker Service running at: {time}", DateTime.Now);
        //        await Task.Delay(1000, stoppingToken);
        //    }
        //}
        #endregion

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //_logger.LogDebug("Starting the service bus queue consumer and the subscription");
            await _messageConsumer.SubscribeAsync(_queueName, stoppingToken);
        }
    }
}
