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

        public StockBackgroundConsumer(IStockResponse stockResponse, IHubContext<ChatHub> hubContext)
        {
            _stockResponse = stockResponse;
            _chatHubContext = hubContext;
            _stockResponse.Initialize();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

             _stockResponse.Subscribe(async stockResponse =>
            {
                if(stockResponse.IsSuccesfull)
                {
                    await _chatHubContext.Clients.All.SendAsync("BotMethod", "Bot", $"{stockResponse.Stock.Symbol} quote is ${stockResponse.Stock.High} per share");
                } else
                {
                    await _chatHubContext.Clients.All.SendAsync("BotMethod", "Bot", stockResponse.Message);
                }
            });
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _stockResponse.Dispose();
            base.Dispose();
        }

    }
}
