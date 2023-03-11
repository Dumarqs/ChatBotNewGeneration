using Domain.Core.Http;
using Infra.CrossCutting.Log.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Text;

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

        public virtual async Task<Response> PostAsync(string url, string name, T content, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage responseMessage = new();
            var response = new Response();
            try
            {
                using var client = _httpClient.CreateClient(name);
                var json = JsonConvert.SerializeObject(content, Newtonsoft.Json.Formatting.Indented);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                responseMessage = await client.PostAsync(url, data, cancellationToken);
                
                response.ResponseContent = await responseMessage.Content.ReadAsStringAsync();
                response.Status = responseMessage.StatusCode;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}