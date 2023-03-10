
using Infra.CrossCutting.Log.Interfaces;

namespace Infra.CrossCutting.Http
{
    public abstract class HttpCore<T>
    {
        private readonly ILoggerAdapter<T> _logger;
        private readonly IHttpClientFactory _httpClient;

        public HttpCore(ILoggerAdapter<T> logger,
                        IHttpClientFactory httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public virtual async Task<string> GetAsync(string url, string name, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = new();
            try
            {
                using var client = _httpClient.CreateClient(name);

                response = await client.GetAsync(url, cancellationToken);
                string result = await response.Content.ReadAsStringAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}