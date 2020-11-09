using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Stock.DTO;
using Microsoft.Extensions.Hosting;

namespace BusinessChat.Infrastructure.Services
{
    public class StockResponseConsumerHostedService : BackgroundService
    {
        private readonly IStockResponse _stockResponse;
        private INotification _notification;

        public StockResponseConsumerHostedService(IStockResponse stockResponse, INotification notification)
        {
            _notification = notification;
            _stockResponse = stockResponse;
            _stockResponse.Initialize();
            
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _stockResponse.Subscribe(ProcessStockQuery);
            return Task.CompletedTask;
        }

        private async Task ProcessStockQuery(StockResponseDTO stock)
        {
            await _notification.Notify(stock);
        }

        public override void Dispose()
        {
            _stockResponse.Dispose();
            base.Dispose();
        }
    }
}
