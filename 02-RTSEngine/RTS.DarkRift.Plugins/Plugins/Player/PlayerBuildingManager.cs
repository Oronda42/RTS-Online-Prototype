using DarkRift;
using DarkRift.Server;
using RTS.Configuration;
using RTS.Server.Messages;
using RTS.Database;
using RTS.Models;
using RTS.Simulator;
using System;

namespace RTS.Plugins
{
    class PlayerBuildingManager : Plugin
    {
        #region Plugin implementation
        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public PlayerBuildingManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        public void Init()
        {
            //Listen to message
            ClientManager.ClientConnected += OnClientConnected;

            Console.WriteLine("[RTS] INFO : PlayerBuildingManager initialized successfully");
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
                case CommunicationTag.PlayerBuildings.CREATE_PLAYER_BUILDING_REQUEST:
                    HandleCreatePlayerBuildingRequest(e);
                    break;
                case CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_REQUEST:
                    HandleBuildingRequest(e);
                    break;
                case CommunicationTag.PlayerBuildings.DESTROY_PLAYER_BUILDING_REQUEST:
                    DestroyPlayerBuilding(e);
                    break;
                case CommunicationTag.PlayerBuildings.INCREMENT_LEVEL_PLAYER_BUILDING_REQUEST:
                    IncrementPlayerBuildingLevel(e);
                    break;
                case CommunicationTag.PlayerBuildings.ACTIVATE_PLAYER_BUILDING_REQUEST:
                    ActivatePlayerBuilding(e);
                    break;
                case CommunicationTag.PlayerBuildings.PAUSE_PLAYER_BUILDING_REQUEST:
                    PausePlayerBuilding(e);
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
        public void HandleCreatePlayerBuildingRequest(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.PlayerBuildings.CREATE_PLAYER_BUILDING_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    //Get the message
                    PlayerBuildingCreationRequestMessage request = e.GetMessage().Deserialize<PlayerBuildingCreationRequestMessage>();

                    //If player is authorized
                    if (ConnectionManager.instance.CheckClientToken(request.playerId, request.token))
                    {
                        Client client = ConnectionManager.instance.GetClientByPlayerId(request.playerId);
                        if (client == null)
                            throw new Exception("[RTS Info] : There is no client for the player " + client.Simulation.Player.id);

                        bool canCreateBuilding = true;

                        //Response to be sent to the player
                        PlayerBuildingCreationResponseMessage responseData = null;

                        //There is a building already on this map element
                        PlayerBuildingModel playerBuilding = client.Simulation.BuildingManager.GetPlayerBuildingByElementInstanceId(request.mapExtentId, request.mapElementInstanceId);

                        if (playerBuilding != null)
                        {
                            canCreateBuilding = false;

                            responseData = new PlayerBuildingCreationResponseMessage
                            {
                                isBuidlingValid = false,
                                buildingNumber = 0,
                                mapExtentId = request.mapExtentId,
                                mapElementInstanceId = request.mapElementInstanceId,
                            };
                        }


                        client.Simulation.Simulate();

                        BuildingModel buildingToCreate = BuildingData.GetBuildingById(request.buildingId);
                        if (buildingToCreate == null)
                            throw new Exception("[RTS Info] : There is no building to create");


                        //Check for resource
                        if (canCreateBuilding && client.Simulation.Player.resourceBag.HasEnoughResource(buildingToCreate.cost))
                        {
                            switch (buildingToCreate.buildingType.id)
                            {
                                case (int)TypeOfBuilding.BUILDING_PRODUCER:

                                    //Create the building
                                    PlayerBuildingProducerModel playerBuildingProducerToCreate = new PlayerBuildingProducerModel((BuildingProducerModel)buildingToCreate);
                                    playerBuildingProducerToCreate.Init(DateTime.Now, client.Simulation.Player);

                                    //Get building number
                                    playerBuildingProducerToCreate.buildingNumber = client.Simulation.BuildingManager.GetNextPlayerBuildingNumber();
                                    playerBuildingProducerToCreate.mapExtentId = request.mapExtentId;
                                    playerBuildingProducerToCreate.mapElementInstanceId = request.mapElementInstanceId;

                                    //Building type
                                    playerBuildingProducerToCreate.Building.buildingType = new BuildingTypeModel();
                                    playerBuildingProducerToCreate.Building.buildingType.id = (int)TypeOfBuilding.BUILDING_PRODUCER;

                                    //State normal
                                    playerBuildingProducerToCreate.StartConstruction(DateTime.Now);

                                    //Add the building to the simulation & remove resource
                                    client.Simulation.Player.resourceBag.Consume(playerBuildingProducerToCreate.Building.cost);
                                    client.Simulation.AddBuilding(playerBuildingProducerToCreate);

                                    //Send the response to the player
                                    responseData = new PlayerBuildingCreationResponseMessage
                                    {
                                        isBuidlingValid = true,
                                        buildingNumber = playerBuildingProducerToCreate.buildingNumber,
                                        mapExtentId = playerBuildingProducerToCreate.mapExtentId,
                                        mapElementInstanceId = playerBuildingProducerToCreate.mapElementInstanceId,
                                    };

                                    break;

                                case (int)TypeOfBuilding.BUILDING_PASSIVE:

                                    //Create the building
                                    PlayerBuildingPassiveModel playerBuildingPassiveToCreate = new PlayerBuildingPassiveModel((BuildingPassiveModel)buildingToCreate);
                                    playerBuildingPassiveToCreate.Init(DateTime.Now, client.Simulation.Player);
                                    

                                    //Get building number
                                    playerBuildingPassiveToCreate.buildingNumber = client.Simulation.BuildingManager.GetNextPlayerBuildingNumber();
                                    playerBuildingPassiveToCreate.mapExtentId = request.mapExtentId;
                                    playerBuildingPassiveToCreate.mapElementInstanceId = request.mapElementInstanceId;

                                    //Type of building 
                                    playerBuildingPassiveToCreate.Building.buildingType = new BuildingTypeModel();
                                    playerBuildingPassiveToCreate.Building.buildingType.id = (int)TypeOfBuilding.BUILDING_PASSIVE;

                                    //State normal
                                    playerBuildingPassiveToCreate.StartConstruction(DateTime.Now);

                                    //Add the building to the simulation
                                    client.Simulation.Player.resourceBag.Consume(playerBuildingPassiveToCreate.Building.cost);
                                    client.Simulation.AddBuilding(playerBuildingPassiveToCreate);

                                    responseData = new PlayerBuildingCreationResponseMessage
                                    {
                                        isBuidlingValid = true,
                                        buildingNumber = playerBuildingPassiveToCreate.buildingNumber,
                                        mapExtentId = playerBuildingPassiveToCreate.mapExtentId,
                                        mapElementInstanceId = playerBuildingPassiveToCreate.mapElementInstanceId,

                                    };

                                    break;

                                default:
                                    canCreateBuilding = false;
                                    break;
                            }
                        }
                        else
                        {
                            canCreateBuilding = false;

                            responseData = new PlayerBuildingCreationResponseMessage
                            {
                                isBuidlingValid = false,
                                buildingNumber = 0,
                                mapExtentId = request.mapExtentId,
                                mapElementInstanceId = request.mapElementInstanceId,
                            };
                        }


                        //Send the response if there is one to send
                        if (responseData != null)
                        {
                            using (Message response = Message.Create(CommunicationTag.PlayerBuildings.CREATE_PLAYER_RESOURCE_BUILDING_RESPONSE, responseData))
                            {
                                e.Client.SendMessage(response, SendMode.Reliable);
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Send all buildings to the client
        /// </summary>
        /// <param name="e"></param>
        public void HandleBuildingRequest(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    PlayerBuildingListRequestMessage request = e.GetMessage().Deserialize<PlayerBuildingListRequestMessage>();

                    if (ConnectionManager.instance.CheckClientToken(request.playerId, request.token))
                    {
                        Client client = ConnectionManager.instance.GetClientByPlayerId(request.playerId);
                        if (client == null)
                            throw new Exception("[RTS Info] : There is no client for the player " + client.Simulation.Player.id);

                        ////////////////////////////
                        // Building center

                        PlayerBuildingCenterMessage buildingCenterMessageData = new PlayerBuildingCenterMessage();
                        buildingCenterMessageData.PlayerBuilding = PlayerBuildingCenterFactory.Get(client.Simulation.SQLConnexion, request.playerId);

                        if (buildingCenterMessageData.PlayerBuilding == null)
                            throw new Exception("There is no player center for player " + request.playerId);

                        //Create and send message
                        using (Message buildingCenterMessage = Message.Create(CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_CENTER_RESPONSE, buildingCenterMessageData))
                        {
                            e.Client.SendMessage(buildingCenterMessage, SendMode.Reliable);
                        }


                        ////////////////////////////
                        // Buildings producer

                        //Create the message 
                        PlayerBuildingProducerListResponseMessage producerBuildingsMessageData = new PlayerBuildingProducerListResponseMessage();

                        //Set data
                        producerBuildingsMessageData.buildings = PlayerBuildingProducerFactory.GetAllBuildings(client.Simulation.SQLConnexion, request.playerIdBuildings);

                        //Create and send message
                        using (Message BuildingsResourceProducerMessage = Message.Create(CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_PRODUCER_RESPONSE, producerBuildingsMessageData))
                        {
                            e.Client.SendMessage(BuildingsResourceProducerMessage, SendMode.Reliable);
                        }

                        ////////////////////////////
                        // Buildings passive

                        //Create the message 
                        PlayerBuildingPassiveListResponseMessage passiveBuildingsMessageData = new PlayerBuildingPassiveListResponseMessage();

                        //Set data
                        passiveBuildingsMessageData.playerBuildings = PlayerBuildingPassiveFactory.GetAllBuildings(client.Simulation.SQLConnexion, request.playerIdBuildings);

                        //Create and send message
                        using (Message BuildingsResourcePassiveMessage = Message.Create(CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_PASSIVE_RESPONSE, passiveBuildingsMessageData))
                        {
                            e.Client.SendMessage(BuildingsResourcePassiveMessage, SendMode.Reliable);
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

        /// <summary>
        /// Update the Building Level in DB
        /// </summary>
        /// <param name="e"></param>
        public void IncrementPlayerBuildingLevel(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.PlayerBuildings.INCREMENT_LEVEL_PLAYER_BUILDING_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    PlayerBuildingUpdatelLevelRequest request = e.GetMessage().Deserialize<PlayerBuildingUpdatelLevelRequest>();

                    //If player is authorized
                    if (ConnectionManager.instance.CheckClientToken(request.playerId, request.token))
                    {
                        Client client = ConnectionManager.instance.GetClientByPlayerId(request.playerId);
                        if (client == null)
                            throw new Exception("[RTS Info] : There is no player simulation for the player " + client.Simulation.Player.id);

                        //Simulate player to update informations
                        client.Simulation.Simulate();

                        PlayerBuildingModel playerBuilding = client.Simulation.BuildingManager.GetPlayerBuildingByNumber(request.buildingNumber);
                        if (playerBuilding == null)
                            throw new Exception("The player has no building with number " + request.buildingNumber);

                        if (playerBuilding.CanLevelUp())
                        {
                            client.Simulation.BuildingManager.GetPlayerBuildingByNumber(request.buildingNumber).IncrementLevel(DateTime.Now);
                            Console.WriteLine(string.Format("[RTS] INFO : Building number {0}, buildingtype : BuildingProducer,  Level Up by player {1}",
                                    request.buildingNumber, request.playerId));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update player building producer message
        /// </summary>
        /// <param name="e"></param>
        public void ActivatePlayerBuilding(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.PlayerBuildings.ACTIVATE_PLAYER_BUILDING_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    PlayerBuildingMessage Message = e.GetMessage().Deserialize<PlayerBuildingMessage>();

                    //If player is authorized
                    if (ConnectionManager.instance.CheckClientToken(Message.playerId, Message.token))
                    {
                        Client client = ConnectionManager.instance.GetClientByPlayerId(Message.playerId);
                        if (client == null)
                            throw new Exception("[RTS Info] : There is no player simulation for the player " + client.Simulation.Player.id);

                        //Simulate player to update informations
                        client.Simulation.Simulate();

                        PlayerBuildingModel playerBuildingProducer = client.Simulation.BuildingManager.GetPlayerBuildingByNumber(Message.PlayerBuilding.buildingNumber);

                        //Check if the player has enough resource
                        if (playerBuildingProducer.CanActivate())
                        {
                            playerBuildingProducer.Activate(DateTime.Now);
                            Console.WriteLine("[RTS] INFO : Player building producer N° " + playerBuildingProducer.buildingNumber + "activated successfully");
                        }
                        else
                            Console.WriteLine("[RTS] INFO : Player building producer N° " + playerBuildingProducer.buildingNumber + "NOT activated successfully");

                        

                    }
                }
            }
        }

        /// <summary>
        /// Pause the player building. Set state to PAUSED
        /// </summary>
        /// <param name="e"></param>
        public void PausePlayerBuilding(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.PlayerBuildings.PAUSE_PLAYER_BUILDING_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    PlayerBuildingMessage Message = e.GetMessage().Deserialize<PlayerBuildingMessage>();

                    //If player is authorized
                    if (ConnectionManager.instance.CheckClientToken(Message.playerId, Message.token))
                    {
                        Client client = ConnectionManager.instance.GetClientByPlayerId(Message.playerId);
                        if (client == null)
                            throw new Exception("[RTS Info] : There is no player simulation for the player " + client.Simulation.Player.id);

                        //Simulate player to update informations
                        client.Simulation.Simulate();

                        PlayerBuildingModel playerBuildingProducer = client.Simulation.BuildingManager.GetPlayerBuildingByNumber(Message.PlayerBuilding.buildingNumber);

                        //Check if the player has enough resource
                        if (playerBuildingProducer.CanPause())
                        {
                            playerBuildingProducer.Pause(DateTime.Now);
                        }

                        Console.WriteLine("[RTS] INFO : Player building producer N° " + playerBuildingProducer.buildingNumber + "paused successfully");
                    }
                }
            }
        }
        #endregion

        #region Commands

        public override Command[] Commands => new Command[]
        {
            new Command("LoadBuildingData", "Retreives building data from the database.", "loadbuildingdata", HighScoresCommandHandler)
        };

        void HighScoresCommandHandler(object sender, CommandEventArgs e)
        {
            //GameBuildingManager.Init();
        }

        #endregion
    }
}
