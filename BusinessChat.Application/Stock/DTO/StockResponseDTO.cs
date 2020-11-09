using System;
using BusinessChat.Application.Common.Models;

namespace BusinessChat.Application.Stock.DTO
{
    public class StockResponseDTO
    {
        public Common.Models.Stock Stock { get; set; }
        public bool IsSuccesfull { get; set;  }
        public string Message  { get; set; }

        public StockResponseDTO()
        {

        }

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
