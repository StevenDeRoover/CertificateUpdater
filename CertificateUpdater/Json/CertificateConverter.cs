using Autofac;
using Autofac.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Config;
using CertificateUpdater.Models;

namespace CertificateUpdater.Json
{
    internal class CertificateConverter : Newtonsoft.Json.Converters.ExpandoObjectConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var autoFac = serializer.ContractResolver as AutofacContractResolver;
            var obj = (ExpandoObject)base.ReadJson(reader, objectType, existingValue, serializer);

            var certificate = new CertificateModel
            {
                Configs =
                    obj.Select(o =>
                    {
                        //autoFac.Named = o.Key;

                        autoFac.Container.ComponentRegistry.TryGetRegistration(new Autofac.Core.KeyedService(o.Key, typeof(IConfig)), out IComponentRegistration reg);
                        var type = reg.Activator.LimitType;

                        var serialized = JsonConvert.SerializeObject(o.Value);
                        //using (TextReader textReader = new StringReader(serialized))
                        //{

                        //    return serializer.Deserialize(new JsonTextReader(textReader)) as IConfig;
                        //}
                        return (IConfig)Newtonsoft.Json.JsonConvert.DeserializeObject(serialized, type, new JsonSerializerSettings
                        {
                            ContractResolver = new LambdaResolver(() => autoFac.Container.ResolveNamed<IConfig>(o.Key))
                        });
                    }).ToArray()
            };
            return certificate;
        }
    }
}
