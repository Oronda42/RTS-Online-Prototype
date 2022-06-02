using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public class DatabaseEvent : ThreadEvent
    {
        /// <summary>
        /// Delegate to the method that the databaseIO thread must execute
        /// </summary>
        public Delegate DatabaseCallback { get; private set; }

        /// <summary>
        /// Method to execute if the DatabaseCallback execution is successfully executed
        /// </summary>
        public ThreadEvent SuccessEvent { get; set; }

        /// <summary>
        /// Method to execute if the DatabaseCallback execution fails
        /// </summary>
        public ThreadEvent ErrorEvent { get; set; }

        public DatabaseEvent(int pId,ThreadBase pSource, Delegate pCallBack, object[] pArgs) : base(pId, pSource, pArgs)
        {
            DatabaseCallback = pCallBack;
        }

        public DatabaseEvent(int pId, ThreadBase pSource, Delegate pCallBack, object[] pArgs, ThreadEvent pSuccessEvent, ThreadEvent pErrorEvent) : base(pId, pSource, pArgs)
        {
            DatabaseCallback = pCallBack;
            SuccessEvent = pSuccessEvent;
            ErrorEvent = pErrorEvent;
        }
    }
}
