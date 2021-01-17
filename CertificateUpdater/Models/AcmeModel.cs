using Certes.Acme;
using CertificateUpdater.Config;
using System.Collections.Generic;

namespace CertificateUpdater.Models
{
    public class AcmeModel
    {
        public IEnumerable<IChallengeContext> Authorizations { get; internal set; }
        public AcmeConfig Config { get; internal set; }
        public List<INotifyConfig> Notifications { get; internal set; }
        public string Certificate { get; internal set; }
    }
}
