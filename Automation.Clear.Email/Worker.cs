using Automation.Clear.Email.Services.Interfaces;

namespace Automation.Clear.Email
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    using var scope = _serviceProvider.CreateScope();
                    var processClearEmailService = scope.ServiceProvider.GetService<IProcessClearEmailService>();

                    processClearEmailService.ProcessEmails();
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
