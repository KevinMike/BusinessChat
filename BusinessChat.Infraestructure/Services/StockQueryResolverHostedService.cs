using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Exceptions;
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

        public StockQueryResolverHostedService(IStockQuery stockQuery, IStockResponse stockResponse, IStooqService stooqService)
        {
            _stockQuery = stockQuery;
            _stockResponse = stockResponse;
            _stooqService = stooqService;
            _stockQuery.Initialize();
            _stockResponse.Initialize();
        }

        protected override  Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _stockQuery.Subscribe(ProcessStockQuery);
            return Task.CompletedTask;
        }

        private async Task ProcessStockQuery(StockQueryDTO stock)
        {
            try
            {
                var result = await _stooqService.GetStock(stock.StockCode);
                if(!result.Succeeded)
                {
                    throw new NotFoundException("The requested stock could not be retrieved from the Stooq service");
                }
                _stockResponse.Publish(new StockResponseDTO(result.Content));
            }
            catch (Exception ex)
            {
                _stockResponse.Publish(new StockResponseDTO(ex));
            }
        }

        public override void Dispose()
        {
            _stockQuery.Dispose();
            _stockResponse.Dispose();
            base.Dispose();
        }

    }
}
