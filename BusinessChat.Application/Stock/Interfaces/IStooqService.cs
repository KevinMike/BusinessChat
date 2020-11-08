using System;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Models;

namespace BusinessChat.Application.Common.Interfaces
{
    public interface IStooqService
    {
        Task<Models.Stock> GetStock(string stockSymbol);
    }
}
