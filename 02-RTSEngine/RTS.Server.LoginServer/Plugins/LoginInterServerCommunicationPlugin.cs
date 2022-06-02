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

namespace RTS.Server.LoginServer
{
    public class LoginInterServerCommunicationPlugin : InterServerCommunicationPluginBase
    {
        #region Properties

        public static LoginInterServerCommunicationPlugin Instance { get; private set; }

        #endregion

        #region Plugin Implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public LoginInterServerCommunicationPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        #endregion

        #region Implementation
        
        /// <summary>
        /// Initialize the login server initialization plugin. Must be called before using it.
        /// </summary>
        /// <param name="args"></param>
        public override void Init(object[] args)
        {
            base.Init(args);
            

            ////////////////////////////
            /// Try to get World server adress information 
            ServerInstanceModel WorldServerInstanceModel = ServerFactory.GetServerInstance(
                LoginServerInitializationPlugin.Instance.DBConnection.Connection,
                Constants.ServerName.WORLD_SERVER,
                LoginServerInitializationPlugin.Instance.InstanceInformation.ServerInstanceModel.Environment);

            //While no information, sleep 2s and try again
            while (WorldServerInstanceModel == null)
            {
                Console.WriteLine("Connection to World Server Fail - Next try in 2 sec");
                Thread.Sleep(2000);

                //Get World Server Connection
                WorldServerInstanceModel = ServerFactory.GetServerInstance(
                    LoginServerInitializationPlugin.Instance.DBConnection.Connection,
                    Constants.ServerName.WORLD_SERVER,
                    LoginServerInitializationPlugin.Instance.InstanceInformation.ServerInstanceModel.Environment);
            }

            ////////////////////////////
            /// Create the connection between the LoginServer and the WorldServer
            DarkRiftClient client = new DarkRiftClient();
            client.Connect(System.Net.IPAddress.Parse(WorldServerInstanceModel.Host), WorldServerInstanceModel.Port, DarkRift.IPVersion.IPv4);
            
            SendRegistrationServerRequest(client, LoginServerInitializationPlugin.Instance.InstanceInformation.ServerInstanceModel, WorldServerInstanceModel);

            //Create and init the WorldServerInstanceInformation
            ServerInstanceInformation WorldServerInstanceInformation = new ServerInstanceInformation();
            WorldServerInstanceInformation.Init(client, WorldServerInstanceModel);

            OtherServers.Add(WorldServerInstanceInformation);

            Instance = this;
            isInitialized = true;
        }
        #endregion
    }
}
