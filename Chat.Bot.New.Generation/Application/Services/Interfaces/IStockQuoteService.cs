namespace Application.Services.Interfaces
{
    public interface IStockQuoteService
    {
        Task<string> GetStockQuoteCSV(string uri);
    }
}
