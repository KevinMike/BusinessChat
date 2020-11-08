using System;
namespace BusinessChat.Application.Stock.DTO
{
    public class StockQueryDTO
    {
        public string StockCode { get; set; }
        public StockQueryDTO(string stockCode)
        {
            StockCode = stockCode;
        }
    }
}
