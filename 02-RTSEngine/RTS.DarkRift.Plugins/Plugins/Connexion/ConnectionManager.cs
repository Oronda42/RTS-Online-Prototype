using DarkRift;
using DarkRift.Server;
using MySql.Data.MySqlClient;
using RTS.Server.Messages;
using RTS.Database;
using RTS.Models;
using RTS.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Plugins
{
    class ConnectionManager : Plugin
    {
        #region Properties

        /// <summary>
        /// List of all connected clients 
        /// </summary>
        public List<Client> connectedClients;

        /// <summary>
        /// Singleton pattern
        /// </summary>
        public static ConnectionManager instance;

        #endregion

        #region Plugin implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public ConnectionManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        /// <summary>
        /// Initialize the plugin manager
        /// </summary>
        public void Init()
        {
            instance = this;

            //Initialization
            connectedClients = new List<Client>();

            //Listen to message
            ClientManager.ClientConnected += OnClientConnected;
            ClientManager.ClientDisconnected += OnClientDisconnected;

            Console.WriteLine("[RTS] INFO : ConnectionManager initialized successfully");
        }



        #endregion

        #region Connection / Disconnection

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnClientMessageReceived;
        }

        private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            Client client = GetClientByClientId(e.Client.ID);
            if (client != null)
            {
                Console.WriteLine("[RTS] INFO : The client {0} has disconnect, simulation started and persisted", client.PlayerId);

                //Simulate and persist
                client.Simulation.Simulate();
                client.Simulation.Persist();
                client.Simulation.SQLConnexion.Close();

                // Remove the player from the connected player list
                RemoveClientByPlayerId(client.PlayerId);
            }
        }

        #endregion

        #region Message handle

        private void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Tag)
            {
                case CommunicationTag.Connection.DEVICE_IDENTIFICATION_REQUEST:
                    LoginWithDevice(e);
                    break;
                case CommunicationTag.Connection.PLAYER_CREATION_INFORMATION_MESSAGE:
                    CreatePlayerWithInformations(e);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Check if the token match with the stored token fir the player
        /// </summary>
        /// <param name="pPlayerId"></param>
        /// <param name="pToken"></param>
        /// <returns></returns>
        public bool CheckClientToken(int pPlayerId, string pToken)
        {
            //pToken = -1;
            //Client client = connectedClients.Where(c => c.PlayerId == pPlayerId && c.Token == pToken).FirstOrDefault();

            //if (client == null)
            //    return false;
            //else
                return true;
        }

        /// <summary>
        /// Returns the client for the specified player Id
        /// </summary>
        /// <param name="pPlayerId"></param>
        /// <returns></returns>
        public Client GetClientByPlayerId(int pPlayerId)
        {
            return connectedClients.Where(c => c.PlayerId == pPlayerId).FirstOrDefault();
        }

        /// <summary>
        /// Returns the client by the specified client Id
        /// </summary>
        /// <param name="pClientId"></param>
        /// <returns></returns>
        public Client GetClientByClientId(int pClientId)
        {
            return connectedClients.Where(c => c.ClientId == pClientId).FirstOrDefault();
        }

        /// <summary>
        /// Add a new client to the connected user list
        /// </summary>
        /// <param name="pClient"></param>
        public void AddClient(Client pClient)
        {
            connectedClients.Add(pClient);
        }

        /// <summary>
        /// Remove a client by a player Id
        /// </summary>
        /// <param name="pPlayerId"></param>
        public void RemoveClientByPlayerId(int pPlayerId)
        {
            Client client = GetClientByPlayerId(pPlayerId);

            if (client != null)
                connectedClients.Remove(client);
        }

        /// <summary>
        /// Generate a new id for the player as token
        /// </summary>
        /// <returns></returns>
        public int GenerateToken()
        {
            //return Guid.NewGuid().GetHashCode();
            return -1;
        }

        /// <summary>
        /// Try to login the client from a message
        /// </summary>
        /// <param name="e"></param>
        private void LoginWithDevice(MessageReceivedEventArgs e)
        {
        //    if (e.Tag == CommunicationTag.Connection.LOGIN_DEVICE_REQUEST)
        //    {
        //        using (DarkRiftReader reader = e.GetMessage().GetReader())
        //        {
        //            //Request received by the player
        //            LoginDeviceRequestMessage loginRequest = e.GetMessage().Deserialize<LoginDeviceRequestMessage>();

        //            //Response to send
        //            LoginResponseMessage loginResponse = new LoginResponseMessage();

        //            MySqlConnection connection = DatabaseConnector.GetNewConnection();

        //            //Check if player exists in database
        //            PlayerModel player = PlayerFactory.GetWithDeviceId(connection, loginRequest.deviceId);

        //            //Create the player
        //            if (player == null)
        //            {
        //                Console.WriteLine(string.Format("[RTS] INFO : Player with device not found {0}. The player will be created", loginRequest.deviceId));

        //                //Create serializable object
        //                loginResponse.loginErrorCode = (int)Configuration.LoginErrorCode.USER_DONT_EXIST;
        //                loginResponse.nickname = "";
        //                loginResponse.token = "";
        //                loginResponse.id = -1;
        //            }

        //            //If the player has been retrieved
        //            if (player != null)
        //            {
        //                //If the client is already connected
        //                Client client = GetClientByPlayerId(player.id);
        //                if (client != null)
        //                {
        //                    Console.WriteLine(string.Format("[RTS] INFO : Player is already connected, remove the last session for player id {0}.", player.id));
        //                    RemoveClientByPlayerId(player.id);
        //                }

        //                //Add the client
        //                client = new Client
        //                {
        //                    PlayerId = player.id,
        //                    Token = "", //TODO
        //                    ClientId = e.Client.ID,
        //                    DeviceId = loginRequest.deviceId,
        //                    Simulation = new Simulation(player.id)
        //                };
        //                connectedClients.Add(client);

        //                Console.WriteLine(string.Format("[RTS] INFO : Player {0} connected with device id {1}", player.id, loginRequest.deviceId));

        //                //Simulate all data and persist them into database
        //                client.Simulation.Simulate();
        //                client.Simulation.Persist();

        //                //Create serializable object
        //                loginResponse.loginErrorCode = (int)Configuration.LoginErrorCode.NO_ERROR;
        //                loginResponse.nickname = player.nickname;
        //                loginResponse.token = client.Token;
        //                loginResponse.id = player.id;
        //            }

        //            connection.Close();

        //            //Create and send message
        //            using (Message loginMessage = Message.Create(CommunicationTag.Connection.LOGIN_RESPONSE, loginResponse))
        //            {
        //                e.Client.SendMessage(loginMessage, SendMode.Reliable);
        //            }
        //        }
        //    }
        }

        /// <summary>
        /// Try to login the client from a message
        /// </summary>
        /// <param name="e"></param>
        private void CreatePlayerWithInformations(MessageReceivedEventArgs e)
        {
            //if (e.Tag == CommunicationTag.Connection.PLAYER_CREATION_INFORMATION_MESSAGE)
            //{
            //    using (DarkRiftReader reader = e.GetMessage().GetReader())
            //    {
            //        //Request received by the player
            //        PlayerCreationInformationMessage playerInfosMessage = e.GetMessage().Deserialize<PlayerCreationInformationMessage>();

            //        //Response to send
            //        LoginResponseMessage loginResponse = new LoginResponseMessage();

            //        MySqlConnection connection = DatabaseConnector.GetNewConnection();

            //        //Create the player
            //        PlayerModel player = PlayerFactory.CreateWithDeviceId(connection, playerInfosMessage.deviceId);

            //        //Update player's informations
            //        player.nickname = playerInfosMessage.nickname;

            //        Console.WriteLine(string.Format("[RTS] INFO : The player {0} will be created with device id {1} as player {2}", playerInfosMessage.nickname, playerInfosMessage.deviceId, player.id));

            //        //Add the client
            //        Client client = new Client
            //        {
            //            PlayerId = player.id,
            //            Token = "",
            //            ClientId = e.Client.ID,
            //            DeviceId = playerInfosMessage.deviceId,
            //            Simulation = new Simulation(player.id)
            //        };
            //        connectedClients.Add(client);

            //        //Simulate all data and persist them into database
            //        client.Simulation.Simulate();
            //        client.Simulation.Persist();

            //        //Create serializable object
            //        loginResponse.loginErrorCode = (int)Configuration.LoginErrorCode.NO_ERROR;
            //        loginResponse.nickname = player.nickname;
            //        loginResponse.token = client.Token;
            //        loginResponse.id = player.id;

            //        try
            //        {
            //            //Update player's informations
            //            PlayerFactory.UpdatePlayerInformations(player, connection);
            //            //Commit the entire transaction
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }

            //        connection.Close();

            //        //Create and send message
            //        using (Message loginMessage = Message.Create(CommunicationTag.Connection.LOGIN_RESPONSE, loginResponse))
            //        {
            //            e.Client.SendMessage(loginMessage, SendMode.Reliable);
            //        }
            //    }
            //}
        }

        #endregion
    }
}
