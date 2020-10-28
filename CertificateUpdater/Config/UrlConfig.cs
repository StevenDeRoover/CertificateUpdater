using System.Security.Cryptography.X509Certificates;
using CertificateUpdater.Controller;
using CertificateUpdater.Logging;

namespace CertificateUpdater.Config
{
    class UrlConfig : IConfig, IGetCertificateConfig
    {
        private readonly ILogger _log;
        private readonly UrlController _controller;

        public string Url { get; set; }

        public UrlConfig(ILogger log, UrlController controller) 
        {
            _log = log;
            _controller = controller;
        }

        public X509Certificate Get()
        {
            return _controller.GetCertificate(Url);
            
        }
    }

   
}
