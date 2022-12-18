using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Http;
using CertificateUpdater.Models;
using CloudnsAPI.Client;

namespace CertificateUpdater.Controller
{
    public class CoudnsAPIUpdateController
	{
        public Client Cloudns { get; set; }

        public void NotifyDnsChanges(Config.CloudnsAPIConfig config, Dictionary<string, string> dnsValidations)
        {
            //login
            
            foreach (var dnsValidation in dnsValidations)
            {
                _ = Cloudns.DNS.Request().GetAsync().Result;
            }
        }
    }
}
