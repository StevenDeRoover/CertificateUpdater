using System;

namespace CertificateUpdater.Config
{
    internal class HttpConfigConverter : Newtonsoft.Json.Converters.CustomCreationConverter<AcmeHttpConfig>
    {
        public override AcmeHttpConfig Create(Type objectType)
        {
            return new AcmeHttpConfig();
        }
    }
}