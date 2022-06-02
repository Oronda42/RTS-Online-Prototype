using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift.Server;
using RTS.Configuration;
using RTS.Server;

namespace RTS.Server.WorldServer
{
    internal sealed class WorldServerInitializationPlugin : ServerInitializationPluginBase
    {
        #region Properties

        /// <summary>
        /// Instance of the world server
        /// </summary>
        public static WorldServerInitializationPlugin Instance { get; private set; }

        #endregion

        #region Plugin Implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public WorldServerInitializationPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        #endregion

        #region Implementation
        public override void Init(ServerSpawnData pSpawnData, string pEnvironment, string pName,ServerCustomConfig pConfig)
        {
            base.Init(pSpawnData, pEnvironment, pName, pConfig);

            Instance = this;

            ///////////////////////////
            /// Prepare all game data that the server needs
            WorldServerData.Init(this);

            ///////////////////////////
            /// Schedule plugin initialization
            PluginScheduler.PluginsToSchedule.Add(new PluginSchedule(1, PluginManager.GetPluginByType<WorldPlayerCommunicationPlugin>(), new object[] { DBConnection.Connection}));
            PluginScheduler.PluginsToSchedule.Add(new PluginSchedule(2, PluginManager.GetPluginByType<WorldInterServerCommunicationPlugin>(), new object[] { DBConnection.Connection }));

            PluginScheduler.Schedule();

            isInitialized = true;
        }

        public override void Stop()
        {
            base.Stop();
        }
        #endregion

        #region WorldServer Implementation




        #endregion

        #region Commands

        public override Command[] Commands => new Command[]
        {
            new Command("t", "test commande", "t", test),
        };


        public void test(object sender, CommandEventArgs e)
        {
            IClient pClient = ClientManager.GetClient(1);
            WorldPlayerCommunicationPlugin.Instance.SendDisconnectionMessage(pClient, DisconnectionErrorCode.INVALID_TOKEN);
        }
        #endregion

    }
}
