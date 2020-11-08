using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Stock.DTO;
using Microsoft.Extensions.Hosting;

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
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._stockQuery.Subscribe(ProcessStockQuery);
            return base.StartAsync(stoppingToken);
        }

        private async void ProcessStockQuery(StockQueryDTO stock)
        {
            try
            {
                var result = await _stooqService.GetStock(stock.StockCode);
                Console.WriteLine("Stock response ", result.High);
                await _stockResponse.Publish(new StockResponseDTO(result));
            }
            catch
            {
                Console.WriteLine("Stock not found");
                await _stockResponse.Publish(new StockResponseDTO(new Exception("Stock not found")));
            }
        }

        public override void Dispose()
        {
            //this._stockQuery.Dispose();
            //this._stockResponse.Dispose();
            base.Dispose();
        }
    }
}
