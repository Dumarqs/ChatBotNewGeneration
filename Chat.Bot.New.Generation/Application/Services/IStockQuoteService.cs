namespace Application.Services
{
    public interface IStockQuoteService
    {
        Task<string> GetStockQuoteCSV(string uri);
    }
}
