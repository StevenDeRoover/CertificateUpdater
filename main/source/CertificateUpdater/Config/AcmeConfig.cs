using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Controller;

namespace CertificateUpdater.Config
{
    public class AcmeConfig : IConfig, IRenewCertificateConfig
    {
        public enum ValidationType { 
            DNS,
            HTTP
        }

        [JsonConverter(typeof(DnsConfigConverter))]
        public AcmeDnsConfig DNS { get; set; }

        public AcmeHttpConfig HTTP { get; set; }

        public AcmeController Controller { get; set; }

        public string Email { get; set; }

        public string AccountKey { get; set; }

        public string DomainKey { get; set; }

        public ValidationType Validation { get; set; }

        internal bool Renew(List<INotifyConfig> notificationsList) => Controller.Renew(this, notificationsList).Result;
    }
}
