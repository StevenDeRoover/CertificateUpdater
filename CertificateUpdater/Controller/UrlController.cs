using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Logging;

namespace CertificateUpdater.Controller
{
    class UrlController
    {
        private ILogger _log;

        public UrlController(ILogger log)
        {
            _log = log;
        }

        internal X509Certificate GetCertificate(string url)
        {
            try
            {
                _log?.LogInfo($"Getting certificate from url '{url}'");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Close();

                return request.ServicePoint.Certificate;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex);
                return null;
            }
        }
    }
}
