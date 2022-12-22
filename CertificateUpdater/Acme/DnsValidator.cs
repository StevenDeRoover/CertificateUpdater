using Certes;
using Certes.Acme;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CertificateUpdater.Config;
using CertificateUpdater.Logging;

namespace CertificateUpdater.Acme
{
    public class DnsValidator : IValidator
    {
        private ILogger _log;

        public DnsValidator(ILogger log) {
            _log = log;
        }

        public async Task<bool> Validate(AcmeContext acme, AcmeConfig config, List<INotifyConfig> notificationsList, Func<string[], Task<IOrderContext>> getOrder)
        {
            _log.LogInfo("Acme: Validating DNS order");
            var order = await getOrder(config.DNS.DomainNames);

            _log.LogInfo("Acme: getting authorizations");
            var authz = await order.Authorizations();

            var authorizations = authz.Select(a => a.Dns().Result);

            return await HandleDns(acme, config, authorizations, notificationsList);
        }

        private async Task<bool> HandleDns(AcmeContext acme, AcmeConfig config, IEnumerable<IChallengeContext> authorizations, List<INotifyConfig> notificationsList)
        {
            bool isValid = true;
            Dictionary<string, string> dnsValidation = new Dictionary<string, string>();
            var index = -1;
            foreach (var challenge in authorizations)
            {
                index++;
                var domainName = config.DNS.DomainNames[index].Replace("*.", "");
                var acmeDomain = "_acme-challenge." + domainName;
                var dnsText = acme.AccountKey.DnsTxt(challenge.Token);
                dnsValidation.Add(acmeDomain, dnsText);
                _log.LogInfo($"Add TXT dns for {acmeDomain} to '{dnsText}'");
				await AwaitDnsChanges(domainName, dnsValidation, notificationsList);
			}

            Task.WaitAll(authorizations.Select(c => Task.Run(async () =>
            {
                var validation = await c.Validate();
                do
                {
                    if (validation.Status == Certes.Acme.Resource.ChallengeStatus.Pending)
                    {
                        System.Threading.Thread.Sleep(2000);
                        validation = await c.Resource();
                    }
                } while (validation.Status == Certes.Acme.Resource.ChallengeStatus.Pending);
                isValid = isValid && (validation.Status != Certes.Acme.Resource.ChallengeStatus.Invalid);
            })).ToArray());
            return isValid;
        }

        private async Task AwaitDnsChanges(string domain, Dictionary<string, string> dnsValidations, List<INotifyConfig> notificationsList)
        {
            await Task.Run(() =>
            {
                bool isValid = false;
                while (!isValid)
                {
                    _log.LogInfo("Checking DNS Validations");
                    isValid = dnsValidations.All(v => IsDnsValid(v.Key, v.Value));
                    if (!isValid) {
                        notificationsList.ForEach(n => n.NotifyDnsChanges(domain, dnsValidations));
                        System.Threading.Thread.Sleep(TimeSpan.FromMinutes(30));
                    }
                }
            });
        }

        private bool IsDnsValid(string host, string token)
        {
            return GetTxtRecords(host).Contains(token);
        }

        private static IList<string> GetTxtRecords(string hostname)
        {
            IList<string> txtRecords = new List<string>();
            string output;
            string pattern = string.Format(@"{0}\s*text =\s*""([\w\-\=]*)""", hostname);

            var startInfo = new ProcessStartInfo("nslookup");
            startInfo.Arguments = string.Format("-type=TXT {0} 8.8.8.8", hostname);
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            using (var cmd = System.Diagnostics.Process.Start(startInfo))
            {
                output = cmd.StandardOutput.ReadToEnd();
            }

            MatchCollection matches = Regex.Matches(output, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                if (match.Success)
                    txtRecords.Add(match.Groups[1].Value);
            }

            return txtRecords;
        }



    }
}
