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
            _log.LogInfo("Logging in to Acme");
            AcmeContext acme = default(AcmeContext);
#if DEBUG
            config.AccountKey = string.Empty;
#endif
            if (string.IsNullOrWhiteSpace(config.AccountKey) || !IsKeyValid(config.AccountKey))
            {
#if DEBUG
				acme = new AcmeContext(WellKnownServers.LetsEncryptStagingV2);
#else
                acme = new AcmeContext(WellKnownServers.LetsEncryptV2);
#endif
				await acme.NewAccount(config.Email, true);
                var accountKey = acme.AccountKey.ToPem();
                _log.LogWarning($"New accountkey for '{config.Email}':");
                _log.LogWarning(accountKey);
            }
            else
            {
                string accountKey = File.ReadAllText(config.AccountKey);
                var key = KeyFactory.FromPem(accountKey);
#if DEBUG
				acme = new AcmeContext(WellKnownServers.LetsEncryptStagingV2, key);
#else
                acme = new AcmeContext(WellKnownServers.LetsEncryptV2, key);
#endif
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
