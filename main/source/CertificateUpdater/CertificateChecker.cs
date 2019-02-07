using Certes;
using Certes.Acme;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CertificateUpdater
{
    public class CertificateChecker
    {
        private readonly string _accountKeyFileName;
        private readonly string _currentPemFileName;
        private readonly string _email;
        private readonly Action<Dictionary<string, byte[]>> _saveFileAction;
        private readonly Action<string> _actionAfterCreation;

        public CertificateChecker(string accountKeyFileName, string currentPemFileName, string email) : this(accountKeyFileName, currentPemFileName, email, null, null)
        { }

        public CertificateChecker(string accountKeyFileName, string currentPemFileName, string email, Action<Dictionary<string, byte[]>> saveFileAction, Action<string> actionAfterCreation)
        {
            _accountKeyFileName = accountKeyFileName;
            _currentPemFileName = currentPemFileName;
            _email = email;
            _saveFileAction = saveFileAction;
            _actionAfterCreation = actionAfterCreation;
        }

        public async Task<bool> CheckShouldRenewCertificate()
        {
            return await Task<bool>.Run(() =>
            {
                bool shouldRenew = default(bool);
                FileStream fs = null;
                try
                {
                    X509CertificateParser certParser = new X509CertificateParser();
                    fs = new FileStream(_currentPemFileName, FileMode.Open);
                    X509Certificate cert = certParser.ReadCertificate(fs);
                    var maxDate = cert.NotAfter.AddDays(-10);
                    shouldRenew = (DateTime.Now.Date >= maxDate.Date);
                }
                catch
                {
                }
                finally
                {
                    fs?.Close();
                }
                fs.Close();
                return shouldRenew;
            });
        }
        public async Task<bool> RenewCertificate(string[] domainNames, string mainDomain)
        {
            return await Task<bool>.Run(async () =>
            {
                bool isRenewed = default(bool);

                try
                {
                    AcmeContext acme = await Login();

                    var order = await acme.NewOrder(domainNames);

                    var authz = (await order.Authorizations());
                    var isValid = await HandleChallenges(authz.Select(a => a.Http().Result));

                    if (isValid)
                    {

                        var privateKey = KeyFactory.NewKey(KeyAlgorithm.ES256);
                        var cert = await order.Generate(new CsrInfo
                        {
                            CountryName = "BE",
                            State = "Antwerp",
                            Locality = "Belgium",
                            Organization = "stovem",
                            OrganizationUnit = "stovem",
                            CommonName = mainDomain,
                        }, privateKey);

                        var certPem = cert.ToPem();
                        var privatePem = privateKey.ToPem();

                        File.Move(_currentPemFileName, Path.Combine(Path.GetDirectoryName(_currentPemFileName), Path.GetFileNameWithoutExtension(_currentPemFileName) + "_backup" + DateTime.Now.ToString("yyyyMMddHHmmss") +  Path.GetExtension(_currentPemFileName)));
                        File.WriteAllText(_currentPemFileName, privatePem + certPem);

                        _actionAfterCreation(privatePem = certPem);
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                }

                return isRenewed;
            });
        }

        private async Task<bool> HandleChallenges(IEnumerable<IChallengeContext> challenges)
        {
            var isValid = true;
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
            foreach (var challenge in challenges)
            {
                files.Add(challenge.Token, Encoding.UTF8.GetBytes(challenge.KeyAuthz));
            }
            await SaveFiles(files);
            Task.WaitAll(challenges.Select(c => Task.Run(async () =>
            {
                var validation = await c.Validate();
                do
                {
                    if (validation.Status == Certes.Acme.Resource.ChallengeStatus.Pending)
                    {
                        Thread.Sleep(2000);
                        validation = await c.Resource();
                    }
                } while (validation.Status == Certes.Acme.Resource.ChallengeStatus.Pending);
                isValid = isValid && (validation.Status != Certes.Acme.Resource.ChallengeStatus.Invalid);
            })).ToArray());
            return isValid;
        }

        private async Task SaveFiles(Dictionary<string, byte[]> files)
        {
            await Task.Run(() => { _saveFileAction(files); });
        }

        private async Task<AcmeContext> Login()
        {
            AcmeContext acme = default(AcmeContext);
            if (string.IsNullOrWhiteSpace(File.ReadAllText(_accountKeyFileName)))
            {
                acme = new AcmeContext(WellKnownServers.LetsEncryptV2);
                var account = await acme.NewAccount(_email, true);
                File.WriteAllText(_accountKeyFileName, acme.AccountKey.ToPem());
            }
            else
            {
                var key = KeyFactory.FromPem(File.ReadAllText(_accountKeyFileName));
                acme = new AcmeContext(WellKnownServers.LetsEncryptV2, key);
                var account = await acme.Account();
                return acme;
            }
            return acme;
        }
    }
}
