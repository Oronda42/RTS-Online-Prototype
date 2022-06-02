using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public class ThreadEvent
    {
        #region Properties

        /// <summary>
        /// Source of action request
        /// </summary>
        public ThreadBase ThreadSource;

        /// <summary>
        /// If of the action
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Arguments of the action. 
        /// </summary>
        public object[] Arguments { get; set; }


        #endregion

        #region Constructor

        public ThreadEvent(int pId, ThreadBase pThreadSource, object[] pArguments)
        {
            Id = pId;
            ThreadSource = pThreadSource;
            Arguments = pArguments;
        }

        #endregion
    }
}
