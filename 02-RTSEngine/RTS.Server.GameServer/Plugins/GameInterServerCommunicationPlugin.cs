using DarkRift.Client;
using DarkRift.Server;
using RTS.Database;
using RTS.Models.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTS.Server.GameServer
{
    public class GameInterServerCommunicationPlugin : InterServerCommunicationPluginBase
    {
        #region Properties

        public static GameInterServerCommunicationPlugin Instance { get; private set; }

        #endregion

        #region Plugin Implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public GameInterServerCommunicationPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }
        #endregion

        /// <summary>
        /// Initialize the Game inter server initialization
        /// </summary>
        /// <param name="args"></param>
        public override void Init(object[] args)
        {
            base.Init(args);
            Instance = this;

            ////////////////////////////
            /// Try to get World server adress information 
            ServerInstanceModel WorldServerInstanceModel = ServerFactory.GetServerInstance(
                DatabaseConnection,
                Constants.ServerName.WORLD_SERVER,
                GameServerInitializationPlugin.Instance.InstanceInformation.ServerInstanceModel.Environment);

            //While no information, sleep 2s and try again
            while (WorldServerInstanceModel == null)
            {
                DispatcherThread.Instance.EnqueueEvent(
                    new LoggingEvent(LogLevel.INFO,"Connection to World Server Fail - Next try in 2 sec", null));
                Thread.Sleep(2000);

                //Get World Server Connection
                WorldServerInstanceModel = ServerFactory.GetServerInstance(
                    DatabaseConnection,
                    Constants.ServerName.WORLD_SERVER,
                    GameServerInitializationPlugin.Instance.InstanceInformation.ServerInstanceModel.Environment);
            }

            ////////////////////////////
            /// Create the connection between the GamaServer and the WorldServer
            DarkRiftClient client = new DarkRiftClient();
            client.Connect(System.Net.IPAddress.Parse(WorldServerInstanceModel.Host), WorldServerInstanceModel.Port, DarkRift.IPVersion.IPv4);

            SendRegistrationServerRequest(client, GameServerInitializationPlugin.Instance.InstanceInformation.ServerInstanceModel, WorldServerInstanceModel);

            //Create and init the WorldServerInstanceInformation
            ServerInstanceInformation WorldServerInstanceInformation = new ServerInstanceInformation();
            WorldServerInstanceInformation.Init(client, WorldServerInstanceModel);

            OtherServers.Add(WorldServerInstanceInformation);

            isInitialized = true;
        }
    }
}
