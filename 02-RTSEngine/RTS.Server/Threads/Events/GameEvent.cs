using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public class GameEvent : ThreadEvent
    {
        /// <summary>
        /// Contain the Method To Execute 
        /// </summary>
        public Delegate CallBack;

        public GameEvent(int pId, ThreadBase pSource, Delegate pCallBack, object[] pArgs) : base(pId, pSource, pArgs)
        {
            CallBack = pCallBack;
        }
    }
}
