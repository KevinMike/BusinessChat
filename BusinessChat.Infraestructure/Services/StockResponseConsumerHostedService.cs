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

        public StockResponseConsumerHostedService(IStockResponse stockResponse)
        {
            _stockResponse = stockResponse;
            _stockResponse.Initialize();
            
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _stockResponse.Subscribe(ProcessStockQuery);
            return Task.CompletedTask;
        }

        private Task ProcessStockQuery(StockResponseDTO stock)
        {
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _stockResponse.Dispose();
            base.Dispose();
        }
    }
}
