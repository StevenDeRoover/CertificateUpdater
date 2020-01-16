using System;

namespace CertificateUpdater.Config
{
    internal class DnsConfigConverter : Newtonsoft.Json.Converters.CustomCreationConverter<AcmeDnsConfig>
    {
        public override AcmeDnsConfig Create(Type objectType)
        {
            return new AcmeDnsConfig();
        }
    }
}