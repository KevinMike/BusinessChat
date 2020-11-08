using System;
using System.Threading.Tasks;
using BusinessChat.Application.Stock.DTO;

namespace BusinessChat.Application.Common.Interfaces
{
    public interface IStockQuery
    {
        Task Publish(StockQueryDTO stockCode);
        Task Subscribe(Action<StockQueryDTO> action);
    }
}
