﻿using System;
using System.Collections.Generic;
using System.Linq;
using CertificateUpdater.Config;
using CertificateUpdater.Logging;
using CertificateUpdater.Models;

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

        public void ProcessCertificates()
        {
            foreach (var certificate in Config.Certificates)
            {
                ProcessCertificate(certificate);
            }
        }

        private void ProcessCertificate(CertificateModel certificate)
        {
            var acme = certificate.Configs.OfType<AcmeConfig>().FirstOrDefault();
            var getCertificate = certificate.Configs.OfType<IGetCertificateConfig>().FirstOrDefault();

            if (acme != null && getCertificate != null && GetCertificate(getCertificate, out bool shouldRenew))
            {
#if DEBUG
                shouldRenew = true;
#endif
                _logger.LogInfo($"- should renew: {shouldRenew}");

                if (shouldRenew)
                {
                    if (acme.Renew(certificate.Configs.OfType<INotifyConfig>().ToList()))
                    {
                        SaveNewCertificate(acme.Controller.Model.Certificate, certificate.Configs.OfType<ISaveCertificateConfig>().ToList());
                        NotifyNewCertificate(acme.Controller.Model.Certificate, certificate.Configs.OfType<INotifyConfig>().ToList());
                    }
                }
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
            }
            return certificate != null;
        }

        private void SaveNewCertificate(string newCertificate, List<ISaveCertificateConfig> saveList)
        {
            saveList.ForEach(s => s.Save(newCertificate));
        }

        private void NotifyNewCertificate(string certificate, List<INotifyConfig> list)
        {
            list.ForEach(l => l.NotifyNewCertificate(certificate));
        }

    }
}