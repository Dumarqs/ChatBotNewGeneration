using System.Net;

namespace Domain.Core.Http
{
    public class Response
    {
        public HttpStatusCode Status { get; set; }
        public string ResponseContent { get; set; }
    }
}
