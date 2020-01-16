using Certes;
using Certes.Acme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateUpdater.Config;

namespace CertificateUpdater.Acme
{
    public interface IValidator
    {
        Task<bool> Validate(AcmeContext acme, AcmeConfig config, List<INotifyConfig> notificationsList, Func<string[], Task<IOrderContext>> getOrder);
    }
}
