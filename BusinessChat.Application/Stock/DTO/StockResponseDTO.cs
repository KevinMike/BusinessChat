using System;
using BusinessChat.Application.Common.Models;

namespace BusinessChat.Application.Stock.DTO
{
    public class StockResponseDTO
    {
        public Common.Models.Stock Stock { get; set; }
        public bool IsSuccesfull { get; }
        public string Message  { get; set; }

        public StockResponseDTO(Common.Models.Stock stock)
        {
            Stock = stock;
            IsSuccesfull = true;
        }

        public StockResponseDTO(Exception ex)
        {
            Message = ex.Message;
            IsSuccesfull = false;
        }
    }
}
