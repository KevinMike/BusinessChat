using System;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Models;
using BusinessChat.Application.Stock.DTO;

namespace BusinessChat.Application.Common.Interfaces
{
    public interface IStockResponse
    {
        Task Publish(StockResponseDTO stockSymbol);
        Task Subscribe(Action<StockResponseDTO> action);
    }
}
