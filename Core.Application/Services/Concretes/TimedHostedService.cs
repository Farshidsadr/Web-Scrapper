using Core.Application.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Application.Services.Concretes
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        #region Properties

        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer = null!;
        private readonly ICurrencyRateService _currencyRateService;

        #endregion

        #region Ctor

        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _currencyRateService = factory.CreateScope().ServiceProvider.GetRequiredService<ICurrencyRateService>();
        }

        #endregion

        #region StartAsync

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        #endregion

        #region DoWork

        private void DoWork(object? state)
        {
            _ = _currencyRateService.ExtractRateAsync().Result;
        }

        #endregion

        #region StopAsync

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            _timer?.Dispose();
        }

        #endregion
    }
}
