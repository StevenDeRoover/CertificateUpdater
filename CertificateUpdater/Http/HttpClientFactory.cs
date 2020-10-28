using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Http
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient();
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient()
        {
            return new HttpClient();
        }
    }
}
