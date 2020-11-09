using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Common.Models;
using CsvHelper;

namespace BusinessChat.Infrastructure.Services
{
    public class StooqService : IStooqService
    {
        private readonly string BASE_URL = @"https://stooq.com";

        public async Task<Result<Stock>> GetStock(string stockCode)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, BASE_URL + $@"/q/l/?s={ stockCode }&f=sd2t2ohlcv&h&e=csv");
                    var response = await client.SendAsync(request);
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var streamReader = new StreamReader(stream))
                    using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                    {
                        Stock stock = csv.GetRecords<Stock>().Single();
                        return Result<Stock>.Success(stock);
                    }
                }
                catch (Exception ex)
                {
                    return Result<Stock>.Failure(new String[] { ex.Message } );
                }
                
            }    
        }
    }
}
