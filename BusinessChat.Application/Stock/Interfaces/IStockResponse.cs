using System;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Models;
using BusinessChat.Application.Stock.DTO;

namespace BusinessChat.Application.Common.Interfaces
{
    public interface IStockResponse : IDisposable
    {
        void Initialize();
        Task Publish(StockResponseDTO stockSymbol);
        Task Subscribe(Func<StockResponseDTO,Task> action);
    }
}
