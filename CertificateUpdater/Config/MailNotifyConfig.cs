using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Config
{
    public class MailNotifyConfig : IConfig, INotifyConfig
    {
        public void NotifyDnsChanges(string domain, Dictionary<string, string> dnsValidations)
        {
        }

        public void NotifyNewCertificate(string certificate)
        {
        }
    }
}
