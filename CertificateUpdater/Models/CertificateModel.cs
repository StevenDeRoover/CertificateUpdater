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
using CertificateUpdater.Json;

namespace CertificateUpdater.Models
{
    [JsonConverter(typeof(CertificateConverter))]
    public class CertificateModel
    {
        public IConfig[] Configs { get; set; }
    }

    
}
