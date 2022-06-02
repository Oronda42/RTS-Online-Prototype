using DarkRift.Server;
using MySql.Data.MySqlClient;
using RTS.Database;
using RTS.Models.Server;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS.Server;
using DarkRift.Client;

namespace RTS.Server.LoginServer
{
    public class LoginServerInitializationPlugin : ServerInitializationPluginBase
    {
        #region Properties

        public static LoginServerInitializationPlugin Instance { get; set; }

        #endregion

        #region Plugin Implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public LoginServerInitializationPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        #endregion

        #region Implementation
        public override void Init(ServerSpawnData pSpawnData, string pEnvironment, string pName, ServerCustomConfig pConfigFile)
        {
            base.Init(pSpawnData, pEnvironment, pName, pConfigFile);

            Instance = this;

            try
            {
                PluginScheduler.PluginsToSchedule.Add(new PluginSchedule(1, PluginManager.GetPluginByType<LoginPlayerCommunicationPlugin>(), new object[] { DBConnection.Connection }));
                PluginScheduler.PluginsToSchedule.Add(new PluginSchedule(2, PluginManager.GetPluginByType<LoginInterServerCommunicationPlugin>(), new object[] { DBConnection.Connection }));
                
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
