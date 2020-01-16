using Autofac.Features.Indexed;
using Certes;
using Certes.Acme;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CertificateUpdater.Acme;
using CertificateUpdater.Config;
using CertificateUpdater.Logging;
using CertificateUpdater.Models;

namespace CertificateUpdater.Controller
{
    public class AcmeController
    {
        private readonly ILogger _log;
        private readonly Func<AcmeConfig, AcmeContext> _contextFactory;
        private readonly IIndex<AcmeConfig.ValidationType, IValidator> _validators;

        public AcmeModel Model { get; set; }

        public AcmeController(ILogger log, Func<AcmeConfig, AcmeContext> contextFactory, IIndex<AcmeConfig.ValidationType, Acme.IValidator> validators)
        {
            _log = log;
            _contextFactory = contextFactory;
            _validators = validators;
        }

        internal async Task<bool> Renew(AcmeConfig config, List<INotifyConfig> notificationsList)
        {
            Model.Config = config;
            Model.Notifications = notificationsList;
            var acme = _contextFactory(config);

            IOrderContext order = default(IOrderContext);

            bool validated = await _validators[config.Validation].Validate(acme, config, notificationsList, async(domainNames) => {
                order = await acme.NewOrder(domainNames);
                return order;
            });

            if (validated)
            {
                var privateKey = KeyFactory.NewKey(KeyAlgorithm.ES256);
                var cert = await order.Generate(new CsrInfo
                {
                    CountryName = "BE",
                    State = "Antwerp",
                    Locality = "Belgium",
                    Organization = "stovem",
                    OrganizationUnit = "stovem",
                    CommonName = config.DNS.DomainNames.First(),
                }, privateKey);

                var certPem = cert.ToPem();
                var privatePem = privateKey.ToPem();

                Model.Certificate = privatePem + certPem;
            }

            return validated;
        }
    }
}
