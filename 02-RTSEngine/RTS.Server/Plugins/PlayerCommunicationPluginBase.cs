using DarkRift;
using DarkRift.Server;
using MySql.Data.MySqlClient;
using RTS.Database;
using RTS.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTS.Server
{
    public abstract class PlayerCommunicationPluginBase : PluginBase
    {

        #region Properties

        /// <summary>
        /// List of all connected player 
        /// </summary>
        public List<ClientInformation> connectedClients;

        /// <summary>
        /// Connection (Reference from Server Initialisation Plugin)
        /// </summary>
        public MySqlConnection DatabaseConnection { get; private set; }

        #endregion

        #region Plugin Implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public PlayerCommunicationPluginBase(PluginLoadData pluginLoadData) : base(pluginLoadData)
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
            connectedClients = new List<ClientInformation>();
        }

        /// <summary>
        /// Called when a client is connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            base.OnClientConnected(sender, e);
        }

        protected override void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            base.OnClientDisconnected(sender, e);

            // Remove the player from the connected player list
            RemoveClientByClientId(e.Client.ID);

        }

        /// <summary>
        /// Check if the token match with the stored token fir the player
        /// </summary>
        /// <param name="pPlayerId"></param>
        /// <param name="pToken"></param>
        /// <returns></returns>
        public bool CheckClientToken(int pPlayerId, string pToken)
        {
            ClientInformation client = connectedClients.Where(c => c.PlayerId == pPlayerId && c.Token == pToken).FirstOrDefault();

            if (client == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Returns the client for the specified player Id
        /// </summary>
        /// <param name="pPlayerId"></param>
        /// <returns></returns>
        public ClientInformation GetClientByPlayerId(int pPlayerId)
        {
            return connectedClients.Where(c => c.PlayerId == pPlayerId).FirstOrDefault();
        }

        /// <summary>
        /// Returns the client by the specified client Id
        /// </summary>
        /// <param name="pClientId"></param>
        /// <returns></returns>
        public ClientInformation GetClientByClientId(int pClientId)
        {
            return connectedClients.Where(c => c.ClientId == pClientId).FirstOrDefault();
        }

        /// <summary>
        /// Add a new client to the connected user list
        /// </summary>
        /// <param name="pClient"></param>
        public void AddClient(ClientInformation pClient)
        {
            connectedClients.Add(pClient);
        }

        /// <summary>
        /// Remove a client by a player Id
        /// </summary>
        /// <param name="pClientId"></param>
        public void RemoveClientByPlayerId(int pPlayerId)
        {
            ClientInformation client = GetClientByPlayerId(pPlayerId);

            if (client != null)
                connectedClients.Remove(client);
        }

        /// <summary>
        /// Remove a client by a client Id
        /// </summary>
        /// <param name="pClientId"></param>
        public void RemoveClientByClientId(int pClientId)
        {
            ClientInformation client = GetClientByClientId(pClientId);

            if (client != null)
                connectedClients.Remove(client);
        }

        /// <summary>
        /// Send a disconnection message and disconnect the player
        /// </summary>
        /// <param name="e"></param>
        public virtual void SendDisconnectionMessage(IClient pClient, ushort pErrorCode)
        {
            //Create the message data
            PlayerDisconnectionMessage disconnectionMessage = new PlayerDisconnectionMessage
            {
                DisconnectionMessageId = pErrorCode
            };

            //Create and send message
            using (Message message = Message.Create(CommunicationTag.Connection.PLAYER_DISCONNECTION, disconnectionMessage))
            {
                pClient.SendMessage(message, SendMode.Reliable);
            }

            RemoveClientByClientId(pClient.ID);
            pClient.Disconnect();

        }


        #endregion

    }
}
