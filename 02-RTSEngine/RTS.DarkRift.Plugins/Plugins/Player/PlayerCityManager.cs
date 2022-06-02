using DarkRift;
using DarkRift.Server;
using RTS.Configuration;
using RTS.Server.Messages;
using RTS.Database;
using RTS.Models;

using System;

namespace RTS.Plugins
{
    class PlayerCityManager : Plugin
    {

        #region Plugin implementation
        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public PlayerCityManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        public void Init()
        {
            //Listen to message
            ClientManager.ClientConnected += OnClientConnected;

            Console.WriteLine("[RTS] INFO : PlayerCityManager initialized successfully");
        }

        #endregion

        #region Connection 

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnClientMessageReceived;
        }

        #endregion

        #region Message handle

        private void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Tag)
            {
                case CommunicationTag.PlayerCity.PLAYER_CITY_REQUEST:
                    UpdatePlayerCity(e);
                    break;
                case CommunicationTag.PlayerCity.NEIGHBOORS_CITY_REQUEST:
                    SendAllNeighboors(e);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Create a resource producer building and send a confirmation message to the client
        /// </summary>
        /// <param name="e"></param>
        public void UpdatePlayerCity(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.PlayerCity.PLAYER_CITY_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    //Get the message
                    CredentialMessage request = e.GetMessage().Deserialize<CredentialMessage>();

                    //If player is authorized
                    if (ConnectionManager.instance.CheckClientToken(request.playerId, request.token))
                    {
                        Client client = ConnectionManager.instance.GetClientByPlayerId(request.playerId);
                        if (client == null)
                            throw new Exception("[RTS Info] : There is no client for the player " + client.Simulation.Player.id);


                        PLayerCityResponseMessage response = new PLayerCityResponseMessage();
                        response.model = PlayerCityFactory.GetCity(request.playerId,client.Simulation.SQLConnexion);

                        if (response.model == null)
                            throw new Exception("There is no player City for player " + request.playerId);

                        //Create and send message
                        using (Message playerCityMessage = Message.Create(CommunicationTag.PlayerCity.PLAYER_CITY_RESPONSE, response))
                        {
                            e.Client.SendMessage(playerCityMessage, SendMode.Reliable);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Send all Cities to the client
        /// </summary>
        /// <param name="e"></param>
        public void SendAllNeighboors(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.PlayerCity.NEIGHBOORS_CITY_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    CredentialMessage request = e.GetMessage().Deserialize<CredentialMessage>();

                    if (ConnectionManager.instance.CheckClientToken(request.playerId, request.token))
                    {
                        Client client = ConnectionManager.instance.GetClientByPlayerId(request.playerId);
                        if (client == null)
                            throw new Exception("[RTS Info] : There is no client for the player " + client.Simulation.Player.id);

                        NeighboorsCityResponseMessage neighboorsCityResponseMessage = new NeighboorsCityResponseMessage 
                        {neighboorsCities = PlayerCityFactory.GetAllNeighboors(request.playerId, client.Simulation.SQLConnexion) };


                        using (Message buildingCenterMessage = Message.Create(CommunicationTag.PlayerCity.NEIGHBOORS_CITY_RESPONSE, neighboorsCityResponseMessage))
                        {
                            e.Client.SendMessage(buildingCenterMessage, SendMode.Reliable);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Destroy a building in the Database
        /// </summary>
        /// <param name="e"></param>
        public void DestroyPlayerBuilding(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.PlayerBuildings.DESTROY_PLAYER_BUILDING_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    PlayerBuildingDestroyRequest request = e.GetMessage().Deserialize<PlayerBuildingDestroyRequest>();

                    Client client = ConnectionManager.instance.GetClientByPlayerId(request.playerId);
                    if (client == null)
                        throw new Exception("[RTS Info] : There is no player simulation for the player " + client.Simulation.Player.id);

                    //Simulate player to update informations
                    client.Simulation.Simulate();
                    client.Simulation.Persist();

                    switch (request.buildingTypeId)
                    {
                        case (int)TypeOfBuilding.BUILDING_PRODUCER:
                            if (PlayerBuildingProducerFactory.Delete(client.Simulation.SQLConnexion, request.playerId, request.positionOnMap, request.buildingNumber))
                            {
                                Console.WriteLine(string.Format("[RTS] INFO: Destroy {0}, by player {1}", BuildingData.GetBuildingById(request.buildingId).name, request.playerId));
                                client.Simulation.RemoveBuilding(request.buildingNumber);
                            }
                            break;

                        case (int)TypeOfBuilding.BUILDING_PASSIVE:
                            if (PlayerBuildingPassiveFactory.Delete(client.Simulation.SQLConnexion, request.playerId, request.positionOnMap, request.buildingNumber))
                            {
                                Console.WriteLine(string.Format("[RTS] INFO: Destroy {0}, by player {1}", BuildingData.GetBuildingById(request.buildingId).name, request.playerId));
                                client.Simulation.RemoveBuilding(request.buildingNumber);
                            }
                            break;

                        default:
                            Console.WriteLine("[RTS] INFO: TypeOfBuilding not correct");
                            break;
                    }

                }
            }
        }

       
        #endregion
    }
}
