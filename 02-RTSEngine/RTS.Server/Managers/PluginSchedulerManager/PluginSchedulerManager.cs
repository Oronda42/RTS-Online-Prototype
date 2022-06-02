using DarkRift.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public class PluginSchedulerManager
    {

        #region Properties

        /// <summary>
        /// Plugins to Schedule
        /// </summary>
        public List<PluginSchedule> PluginsToSchedule { get; set; }

        #endregion

        #region Constructor

        public PluginSchedulerManager()
        {
            PluginsToSchedule = new List<PluginSchedule>();
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Schedule plugins
        /// </summary>
        public virtual void Schedule()
        {

            Console.WriteLine("Scheduler initialization start");

            // Order the list
            PluginsToSchedule = PluginsToSchedule.OrderBy(p => p.Order).ToList();

            for (int i = 0; i < PluginsToSchedule.Count; i++)
            {
                PluginsToSchedule[i].Plugin.Init(PluginsToSchedule[i].Arguments);
                Console.WriteLine("Plugin " + PluginsToSchedule[i].Plugin.Name + " initialized successfully");
            }

            Console.WriteLine("Scheduler initialization finished");

        }

        #endregion

    }
}
