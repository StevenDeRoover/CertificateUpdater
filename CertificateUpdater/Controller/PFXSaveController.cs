using CertificateUpdater.Config;
using CertificateUpdater.Logging;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CertrificateUpdater.Controller
{
    public class PFXSaveController
    {
        private ILogger _log;

        public PFXSaveController(ILogger log)
        {
            _log = log;
        }

        public void Save(PFXSaveConfig config, string newCertificate)
        {
            TextReader sr = new StringReader(newCertificate);
            PemReader pemReader = new PemReader(sr);


            Pkcs12Store store = new Pkcs12StoreBuilder().Build();
            List<object> pemChains = new List<object>();

            object o;
            while ((o = pemReader.ReadObject()) != null)
            {
                pemChains.Add(o);
            }

            X509CertificateEntry[] chain = pemChains.OfType<X509Certificate>().Select(c => new X509CertificateEntry(c)).ToArray();
            AsymmetricCipherKeyPair privKey = pemChains.OfType<AsymmetricCipherKeyPair>().FirstOrDefault();

            store.SetKeyEntry(config.Alias, new AsymmetricKeyEntry(privKey.Private), chain);
            FileStream p12file = File.Create(config.Path);
            store.Save(p12file, config.Password.ToCharArray(), new SecureRandom());
            p12file.Close();
        }
    }
}
