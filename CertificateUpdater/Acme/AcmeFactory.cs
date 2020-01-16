using Certes;
using Certes.Acme;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Config;
using CertificateUpdater.Logging;

namespace CertificateUpdater.Acme
{
    public class AcmeFactory
    {
        private ILogger _log;

        public AcmeFactory(ILogger log)
        {
            _log = log;
        }
        

        public async Task<AcmeContext> CreateContext(AcmeConfig config)
        {
            try
            {
                return await Login(config);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<AcmeContext> Login(AcmeConfig config)
        {
            AcmeContext acme = default(AcmeContext);
            if (string.IsNullOrWhiteSpace(config.AccountKey) || !IsKeyValid(config.AccountKey))
            {
                acme = new AcmeContext(WellKnownServers.LetsEncryptV2);
                await acme.NewAccount(config.Email, true);
                var accountKey = acme.AccountKey.ToPem();
                _log.LogWarning($"New accountkey for '{config.Email}':");
                _log.LogWarning(accountKey);
            }
            else
            {
                string accountKey = File.ReadAllText(config.AccountKey);
                var key = KeyFactory.FromPem(accountKey);
                acme = new AcmeContext(WellKnownServers.LetsEncryptV2, key);
                await acme.Account();
            }
            return acme;
        }

        private bool IsKeyValid(string file)
        {
            var key = File.ReadAllText(file);
            try
            {
                KeyFactory.FromPem(key);
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
