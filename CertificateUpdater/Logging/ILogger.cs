using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateUpdater.Logging
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogWarning(string message);

        void LogError(Exception ex);
    }
}
