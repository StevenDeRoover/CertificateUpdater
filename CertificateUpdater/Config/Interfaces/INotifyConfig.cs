using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Config
{
    public interface INotifyConfig
    {
        void NotifyDnsChanges(string domain, Dictionary<string, string> dnsValidations);

        void NotifyNewCertificate(string certificate);
    }
}
