using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Controller;

namespace CertificateUpdater.Config
{
    public class PushBulletNotifyConfig : IConfig, INotifyConfig
    {
        public string AccountKey { get; set;    }

        public PushBulletNotifyController Controller { get; set; }

        public void NotifyDnsChanges(Dictionary<string, string> dnsValidations) => Controller.NotifyDnsChanges(this, dnsValidations);

        public void NotifyNewCertificate(string certificate)
        {
        }
    }
}
