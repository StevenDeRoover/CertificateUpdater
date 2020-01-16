using Certes.Acme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Config;

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
