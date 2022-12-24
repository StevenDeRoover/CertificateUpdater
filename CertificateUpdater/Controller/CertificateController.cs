using CertificateUpdater.Config;
using CertificateUpdater.Logging;
using CertificateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CertificateUpdater.Controller
{
    public class CertificateController
    {
        private ILogger _logger;

        public Configuration Config { get; set; }

        public CertificateController(ILogger logger)
        {
            _logger = logger;
        }

        public void ProcessCertificates(bool force)
        {
            _logger.LogInfo("Processing certificates from config");
            foreach (var certificate in Config.Certificates)
            {
                ProcessCertificate(certificate, force);
            }
        }

        private void ProcessCertificate(CertificateModel certificate, bool force)
        {
            var acme = certificate.Configs.OfType<AcmeConfig>().FirstOrDefault();
            var getCertificate = certificate.Configs.OfType<IGetCertificateConfig>().FirstOrDefault();

            if (acme != null && getCertificate != null && GetCertificate(getCertificate, out bool shouldRenew))
            {
                if (force) shouldRenew = true;
                var success = 
                    shouldRenew &&
                    acme.Renew(certificate.Configs.OfType<INotifyConfig>().ToList()) &&
                    SaveNewCertificate(acme.Controller.Model.Certificate, certificate.Configs.OfType<ISaveCertificateConfig>().ToList()) &&
                    NotifyNewCertificate(acme.Controller.Model.Certificate, certificate.Configs.OfType<INotifyConfig>().ToList());
            }
        }

        private bool GetCertificate(IGetCertificateConfig getCertificate, out bool shouldRenew)
        {
            var certificate = getCertificate.Get();
            shouldRenew = false;
            if (certificate != null)
            {
                _logger.LogInfo("Certificate:");
                _logger.LogInfo($"- Issuer: {certificate.Issuer}");
                _logger.LogInfo($"- Expiration: {certificate.GetExpirationDateString()}");
                var maxDate = DateTime.Parse(certificate.GetExpirationDateString()).AddDays(-10);
                shouldRenew = (DateTime.Now.Date >= maxDate.Date);
                _logger.LogInfo($"- should renew: {shouldRenew}");
            }
            else
            {
                _logger.LogInfo("Certificate was null");
                shouldRenew = true;
                _logger.LogInfo($"- should renew: {shouldRenew}");
            }
            return true;
        }

        private bool SaveNewCertificate(string newCertificate, List<ISaveCertificateConfig> saveList)
        {
            saveList.ForEach(s => s.Save(newCertificate));
            return true;
        }

        private bool NotifyNewCertificate(string certificate, List<INotifyConfig> list)
        {
            list.ForEach(l => l.NotifyNewCertificate(certificate));
            return true;
        }

    }
}
