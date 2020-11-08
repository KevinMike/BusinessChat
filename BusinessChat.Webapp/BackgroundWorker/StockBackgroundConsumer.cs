using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace BusinessChat.Webapp.BackgroundWorker
{
    public class StockBackgroundConsumer : BackgroundService
    {
        private readonly IStockResponse _stockResponse;
        private readonly IHubContext<ChatHub> _chatHubContext;

        public StockBackgroundConsumer(IStockResponse stockResponse, IHubContext<ChatHub> hubContext)
        {
            _stockResponse = stockResponse;
            _chatHubContext = hubContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
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
            return base.StartAsync(stoppingToken);
        }
    }
}
