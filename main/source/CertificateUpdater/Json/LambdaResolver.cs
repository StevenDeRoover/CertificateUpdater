using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Json
{
    public class LambdaResolver : DefaultContractResolver
    {
        private readonly Func<object> _getter;

        public LambdaResolver(Func<object> get)
        {
            _getter = get;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            JsonObjectContract contract = base.CreateObjectContract(objectType);

            contract.DefaultCreator = _getter;

            return contract;
        }
    }
}
