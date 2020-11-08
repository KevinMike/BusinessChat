using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BusinessChat.Webapp.BackgroundWorker
{
    public class StockBackgroundConsumer : BackgroundService
    {
        private readonly IStockResponse _stockResponse;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly ILogger<StockBackgroundConsumer> _logger;

        public StockBackgroundConsumer(IStockResponse stockResponse, IHubContext<ChatHub> hubContext, ILogger<StockBackgroundConsumer> logger)
        {
            _logger = logger;
            _stockResponse = stockResponse;
            _chatHubContext = hubContext;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _stockResponse.Initialize();
            _logger.LogInformation($"Stock Response Consumer started...");
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _stockResponse.Subscribe(async stockResponse =>
            {
                if(stockResponse.IsSuccesfull)
                {
                    await _chatHubContext.Clients.All.SendAsync("BotMethod", "Bot", stockResponse.Stock);
                } else
                {
                    await _chatHubContext.Clients.All.SendAsync("BotMethod", "Bot", stockResponse.Message);
                }
            });
            //return base.StartAsync(stoppingToken);
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _stockResponse.Dispose();
            _logger.LogInformation($"Stock Response Consumer stopped...");
        }

    }
}
