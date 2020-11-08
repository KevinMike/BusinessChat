using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Stock.DTO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BusinessChat.Infrastructure.Services
{
    public class StockQueryResolverHostedService : BackgroundService
    {
        private readonly IStockQuery _stockQuery;
        private readonly IStockResponse _stockResponse;
        private readonly IStooqService _stooqService;
        private ILogger<StockQueryResolverHostedService> _logger;

        public StockQueryResolverHostedService(IStockQuery stockQuery, IStockResponse stockResponse, IStooqService stooqService, ILogger<StockQueryResolverHostedService> logger)
        {
            _stockQuery = stockQuery;
            _stockResponse = stockResponse;
            _stooqService = stooqService;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _stockQuery.Initialize();
            _stockResponse.Initialize();
            _logger.LogInformation($"Stock Query Resolver started...");
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            this._stockQuery.Subscribe(ProcessStockQuery);
            //return base.StartAsync(stoppingToken);
            return Task.CompletedTask;
        }

        private async Task ProcessStockQuery(StockQueryDTO stock)
        {
            try
            {
                var result = await _stooqService.GetStock(stock.StockCode);
                await _stockResponse.Publish(new StockResponseDTO(result));
            }
            catch
            {
                await _stockResponse.Publish(new StockResponseDTO(new Exception("Stock not found")));
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _stockQuery.Dispose();
            _stockResponse.Dispose();
            _logger.LogInformation($"Stock Query Resolver stopped...");
        }

    }
}
