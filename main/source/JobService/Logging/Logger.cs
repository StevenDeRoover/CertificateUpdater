using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JobService.Logging
{
    public static class Logger
    {
        static TraceSource _trace = new TraceSource("Logging");

        public static TraceSource Trace { get { return _trace; } }

        public static void Log(TraceEventType eventType, string logMessage, int id = 0, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            string message = String.Format("{0}:{1} - {2}", callerMemberName, sourceLineNumber, logMessage);
            _trace.TraceEvent(eventType, id, message);
        }

        public static void LogVerbose(string logMessage, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            string message = String.Format("{0}:{1} - {2}", callerMemberName, sourceLineNumber, logMessage);
            _trace.TraceEvent(TraceEventType.Verbose, 0, message);
        }

        public static void LogInformation(string logMessage, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            string message = String.Format("{0}:{1} - {2}", callerMemberName, sourceLineNumber, logMessage);
            _trace.TraceEvent(TraceEventType.Information, 0, message);
        }

        public static void LogWarning(string logMessage, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            string message = String.Format("{0}:{1} - {2}", callerMemberName, sourceLineNumber, logMessage);
            _trace.TraceEvent(TraceEventType.Warning, 0, message);
        }

        public static void LogError(string logMessage, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            string message = String.Format("{0}:{1} - {2}", callerMemberName, sourceLineNumber, logMessage);
            _trace.TraceEvent(TraceEventType.Error, 0, message);
        }

        public static void LogException(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            string message = String.Format("{0}:{1} - {2}", callerMemberName, sourceLineNumber, ex.ToString());
            _trace.TraceEvent(TraceEventType.Critical, 0, message);
        }

        public static void LogException(string errorMessage, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            string message = String.Format("{0}:{1} - {2} - Exception: {3}", callerMemberName, sourceLineNumber, errorMessage, ex.ToString());
            _trace.TraceEvent(TraceEventType.Critical, 0, message);
        }
    }
}
