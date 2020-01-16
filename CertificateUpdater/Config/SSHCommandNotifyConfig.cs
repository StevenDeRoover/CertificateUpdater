using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Controller;

namespace CertificateUpdater.Config
{
    public class SSHCommandNotifyConfig : IConfig, INotifyConfig
    {
        public SSHCommandNotifyController Controller { get; set; }

        public string Host { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string NewCertificateCommand { get; set; }

        public void NotifyDnsChanges(Dictionary<string, string> dnsValidations)
        {

        }

        public void NotifyNewCertificate(string certificate) => Controller.Execute(this, this.NewCertificateCommand);
    }
}
