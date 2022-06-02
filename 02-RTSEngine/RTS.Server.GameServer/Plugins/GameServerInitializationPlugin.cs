using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift.Server;
using RTS.Server;

namespace RTS.Server.GameServer
{
    public class GameServerInitializationPlugin : ServerInitializationPluginBase
    {
        #region Implementation

        public static GameServerInitializationPlugin Instance { get; set; }

        #endregion

        #region Plugin Implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public GameServerInitializationPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        #endregion

        #region Implementation
        
        public override void Init(ServerSpawnData pSpawnData, string pEnvironment, string pName, ServerCustomConfig pConfig)
        {
            base.Init(pSpawnData, pEnvironment, pName, pConfig);

            Instance = this;

            ///////////////////////////
            /// Prepare all game data that the server needs
            GameServerData.Init(this);

            try
            {
                PluginScheduler.PluginsToSchedule.Add(new PluginSchedule(1, PluginManager.GetPluginByType<GamePlayerCommunicationPlugin>(), new object[] { DBConnection.Connection }));
                PluginScheduler.PluginsToSchedule.Add(new PluginSchedule(2, PluginManager.GetPluginByType<GameInterServerCommunicationPlugin>(), new object[] { DBConnection.Connection }));

                PluginScheduler.Schedule();
            }
            catch (Exception e)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, e.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }

            isInitialized = true;
        }


        #endregion


    }
}
