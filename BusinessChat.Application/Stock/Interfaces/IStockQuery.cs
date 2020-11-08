using System;
using System.Threading.Tasks;
using BusinessChat.Application.Stock.DTO;

namespace BusinessChat.Application.Common.Interfaces
{
    public interface IStockQuery : IDisposable
    {
        void Initialize();
        Task Publish(StockQueryDTO stockCode);
        Task Subscribe(Func<StockQueryDTO,Task> action);
    }
}
