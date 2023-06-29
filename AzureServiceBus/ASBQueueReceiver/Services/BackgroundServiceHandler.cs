using System.Threading;

namespace ASBQueueReceiver.Services
{
    public class BackgroundServiceHandler : BackgroundService
    {
        public readonly ILogger<BackgroundServiceHandler> _logger;
        public BackgroundServiceHandler(ILogger<BackgroundServiceHandler> logger)
        {
            _logger = logger;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker Service running at: {time}", DateTime.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
