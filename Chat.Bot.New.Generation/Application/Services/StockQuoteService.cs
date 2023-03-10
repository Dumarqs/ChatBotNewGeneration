using CsvHelper;
using CsvHelper.Configuration;
using Domain.Files;
using Infra.CrossCutting.Http;
using Infra.CrossCutting.Log.Interfaces;
using System.Globalization;

namespace Application.Services
{
    public class StockQuoteService : HttpCore<StockQuoteService>, IStockQuoteService
    {
        private readonly ILoggerAdapter<StockQuoteService> _logger;
        public StockQuoteService(ILoggerAdapter<StockQuoteService> logger, IHttpClientFactory httpClientFactory) :
            base(logger, httpClientFactory)
        {
            _logger = logger;
        }

        public async Task<string> GetStockQuoteCSV(string uri)
        {
            var content = await GetAsync(uri, "csv");
            var quote = await GetStockPrice(content);

            return $"{quote.Symbol} quote is ${quote.Close:N2} per share";
        }

        private async Task<CsvStockQuote> GetStockPrice(string content)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            using var reader = new StreamReader(content);
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<CsvStockQuote>();
                if (records.Any())
                    return records.First();
            }

            return null;
        }
    }
}
