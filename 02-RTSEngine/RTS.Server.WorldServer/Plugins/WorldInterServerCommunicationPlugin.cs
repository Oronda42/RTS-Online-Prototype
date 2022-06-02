using DarkRift.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.WorldServer
{
    internal sealed class WorldInterServerCommunicationPlugin : InterServerCommunicationPluginBase, ISchedulablePlugin
    {

        #region Properties

        /// <summary>
        /// Singleton
        /// </summary>
        public static WorldInterServerCommunicationPlugin Instance { get; private set; }

        #endregion

        #region Plugin Implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public WorldInterServerCommunicationPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        #endregion

        #region ISchedulablePlugin Implementation

        public override void Init(object[] args)
        {
            base.Init(args);

            Instance = this;

            isInitialized = true;
        } 

        #endregion
    }
}
