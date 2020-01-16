using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Config
{
    public class MailNotifyConfig : IConfig, INotifyConfig
    {
        public void NotifyDnsChanges(Dictionary<string, string> dnsValidations)
        {
        }

        public void NotifyNewCertificate(string certificate)
        {
        }
    }
}
