using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Controller;

namespace CertificateUpdater.Config
{
	public class CloudnsAPIConfig : IConfig, INotifyConfig
	{
		public string Key { get; set; }
		public string Password { get; set; }

		public CoudnsAPIUpdateController Controller { get; set; }

		public void NotifyDnsChanges(string domain, Dictionary<string, string> dnsValidations) => Controller.NotifyDnsChanges(this, domain, dnsValidations);

		public void NotifyNewCertificate(string certificate)
		{
		}
	}
}
