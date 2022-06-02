using DarkRift;
using DarkRift.Server;
using MySql.Data.MySqlClient;
using RTS.Database;
using RTS.Models.Server;
using RTS.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public abstract class InterServerCommunicationPluginBase : PluginBase
    {

        #region Properties

        /// <summary>
        /// List of ServerInitializationPlugin children
        /// </summary>
        protected List<ServerInstanceInformation> OtherServers;

        /// <summary>
        /// Connection (Reference from Server Initialisation Plugin)
        /// </summary>
        public MySqlConnection DatabaseConnection { get; private set; }

        #endregion

        #region Plugin Implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public InterServerCommunicationPluginBase(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        #endregion

        #region Implementation

        /// <summary>
        /// Initialize the plugin manager
        /// args[0] = MySqlConnection from Initialization Server Plugin
        /// </summary>
        public override void Init(object[] args)
        {
            base.Init(args);

            if (args[0] == null)
                throw new Exception("Args[0] must be a MySqlConnection");

            if (args[0].GetType() != typeof(MySqlConnection))
                throw new Exception("Args[0] must be a MySqlConnection");

            DatabaseConnection = args[0] as MySqlConnection;

            //Initialization
            OtherServers = new List<ServerInstanceInformation>();

        }

        /// <summary>
        /// Register a server into the list OtherServers
        /// </summary>
        /// <param name="e"></param>
        protected virtual void HandleRegistrationServerRequest(MessageReceivedEventArgs e)
        {
            if (e.Tag == ServerCommunicationTags.SERVER_REGISTRATION_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    ServerInstanceMessage serverInstanceMessage = reader.ReadSerializable<ServerInstanceMessage>();

                    if (serverInstanceMessage == null)
                        return;

                    if (OtherServers.Where(os => os.ServerInstanceModel.Name == serverInstanceMessage.ServerInstance.Name).Count() > 0)
                    {
                        DispatcherThread.Instance.EnqueueEvent(
                            new LoggingEvent(LogLevel.ERROR, string.Format("Server {0} is already registered", serverInstanceMessage.ServerInstance.Name), null));
                        return;
                    }

                    ServerInstanceModel serverInstanceInDatabase = ServerFactory.GetServerInstanceByToken(DatabaseConnection, serverInstanceMessage.Token);
                    if (serverInstanceInDatabase == null)
                    {
                        DispatcherThread.Instance.EnqueueEvent(
                            new LoggingEvent(LogLevel.ERROR, string.Format("Server {0} is not registered in database. Ignoring the request", serverInstanceMessage.ServerInstance.Name), null));
                        return;
                    }

                    //Otherwise, register the server and open a connection
                    OtherServers.Add(new ServerInstanceInformation
                    {
                        ServerInstanceModel = serverInstanceInDatabase,
                        ClientConnection = null //The other server would open a connection if necessary
                    });

                    LoggingEvent log = new LoggingEvent(LogLevel.INFO, string.Format("New Server {0} registered", serverInstanceMessage.ServerName), null);
                    DispatcherThread.Instance.EnqueueEvent(log);
                }
            }
        }

        /// <summary>
        /// Send a registration message to the other server using pClient connection
        /// </summary>
        /// <param name="pClient"></param>
        /// <param name="pCurrentServer"></param>
        protected void SendRegistrationServerRequest(DarkRift.Client.DarkRiftClient pClient, ServerInstanceModel pCurrentServer, ServerInstanceModel pDestinationServer)
        {
            ServerInstanceMessage serverInstanceMessage = new ServerInstanceMessage()
            {
                SerializeCredentials = true,
                ServerName = pCurrentServer.Name,
                Token = pCurrentServer.Token,
                ServerInstance = pDestinationServer
            };

            //Create and send message
            using (Message message = Message.Create(ServerCommunicationTags.SERVER_REGISTRATION_REQUEST, serverInstanceMessage))
            {
                pClient.SendMessage(message, SendMode.Reliable);
            }
        }

        /// <summary>
        /// When a message is received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            base.OnClientMessageReceived(sender, e);

            switch (e.Tag)
            {
                case ServerCommunicationTags.SERVER_REGISTRATION_REQUEST:
                    HandleRegistrationServerRequest(e);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Returns the Server attached to this one by name
        /// </summary>
        /// <param name="pName"></param>
        /// <returns></returns>
        public ServerInstanceInformation GetServerByName(string pName)
        {
            return OtherServers.Where(c => c.ServerInstanceModel.Name == pName).FirstOrDefault();
        }

        /// <summary>
        /// Add new server to list
        /// </summary>
        /// <param name="pServer"></param>
        public void AddServer(ServerInstanceInformation pServer)
        {
            OtherServers.Add(pServer);
        }

        /// <summary>
        /// Revmove the server
        /// </summary>
        /// <param name="pName"></param>
        public void RemoveServerByName(string pName)
        {
            ServerInstanceInformation server = GetServerByName(pName);

            if (server != null)
                OtherServers.Remove(server);
        }

        #endregion

    }
}
