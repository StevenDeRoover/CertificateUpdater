using CertificateUpdater.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CertificateUpdater.Service.Api
{
    [RoutePrefix("api")]
    public class CertificateApiController : ApiController
    {
        private readonly Func<Application> _applicationFactory;
        private readonly ILogger _logger;

        public CertificateApiController(Func<Application> applicationFactory, ILogger logger) {
            _applicationFactory = applicationFactory;
            _logger = logger;
        }

        [HttpGet()]
        [Route("certificate/renew")]
        public IHttpActionResult RenewCertificate()
        {
            return RenewCertificate(false);
        }


        [HttpGet()]
        [Route("certificate/renew/{force}")]
        public IHttpActionResult RenewCertificate(bool force) {
            try
            {
                _logger.LogInfo("HTTP GET  /api/certificate/renew");
                _applicationFactory().Run(force);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return base.InternalServerError(ex);
            }
        }
    }
}
