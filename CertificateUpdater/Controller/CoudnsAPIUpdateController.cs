using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
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

        public void NotifyDnsChanges(Config.CloudnsAPIConfig config, string domain, Dictionary<string, string> dnsValidations)
        {
            //login
            Cloudns.Key = config.Key;
            Cloudns.Password = config.Password;
            foreach (var dnsValidation in dnsValidations)
            {
                var records = Cloudns.DNS.Zone[domain].Records.Request().GetAsync().Result;
				var acme = records.FirstOrDefault(v => v.Value.Host == "_acme-challenge");
				acme.Value.Record = dnsValidation.Value;
				Cloudns.DNS.Zone[domain].Records[acme.Key].Request().PostAsync(acme.Value).Wait();
			}
        }
    }
}
