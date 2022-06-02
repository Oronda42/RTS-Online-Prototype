using DarkRift.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public class PluginSchedule
    {

        #region Properties

        /// <summary>
        /// Order of scheduling
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Plugin to schedule
        /// </summary>
        public PluginBase Plugin { get; set; } 

        /// <summary>
        /// Arguments to pass to the plugin
        /// </summary>
        public object[] Arguments { get; set; }

        #endregion

        #region Implementation

        public PluginSchedule(int pOrder, PluginBase pPlugin, object[] pArgs)
        {
            Order = pOrder;
            Plugin = pPlugin;
            Arguments = pArgs;
        }

        #endregion
    }
}
