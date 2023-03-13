using Application.Services.Interfaces;
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

        public async Task<string> GetStockQuoteCSV(string uri, string stock)
        {
            var uriFormatted = string.Format("{0}?s={1}&f=sd2t2ohlcv&h&e=csv", uri, stock);

            var content = await GetAsync(uriFormatted, "csv");
            var quote = await GetStockPrice(content);

            if(quote.Close == "N/D")
                return $"{quote.Symbol} not found";

            return $"{quote.Symbol} quote is ${quote.Close:N2} per share";

        }

        private async Task<CsvStockQuote> GetStockPrice(string content)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using var reader = new StringReader(content);
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<CsvStockQuote>().ToList();
                if (records.Any())
                    return records.First();
            }
            return null;
        }
    }
}
