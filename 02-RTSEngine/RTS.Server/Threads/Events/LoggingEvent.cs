using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public class LoggingEvent : ThreadEvent
    {
        #region Properties

        public LogLevel logLevel;

        public string message;

        public LoggingEvent(LogLevel pLogLevel, string pMessage, ThreadBase pThreadSouce) : base(1, pThreadSouce,null)
        {
            logLevel = pLogLevel;
            message = pMessage;
            ThreadSource = pThreadSouce;
        }

        #endregion

        #region Implementation
        #endregion


    }
}
