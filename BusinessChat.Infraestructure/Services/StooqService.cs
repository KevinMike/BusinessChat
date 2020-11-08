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
        private static string URL = @"https://stooq.com";

        public async Task<Stock> GetStock(string stockSymbol)
        {
            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, URL + $@"/q/l/?s={ stockSymbol }&f=sd2t2ohlcv&h&e=csv");
                var response = await client.SendAsync(request);
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var streamReader = new StreamReader(stream))
                using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    return csv.GetRecords<Stock>().Single();
                }
            }    
        }
    }
}
