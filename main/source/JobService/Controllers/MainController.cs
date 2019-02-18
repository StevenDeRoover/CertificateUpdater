using Hangfire;
using JobService.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace JobService.Controllers
{

    [RoutePrefix("api")]
    public class MainController : ApiController
    {
        [HttpGet, Route("checkcertificate")]
        public void CheckCertificate()
        {
            BackgroundJob.Enqueue<CheckCertificateJob>(c => c.Check())
        }
    }
}
