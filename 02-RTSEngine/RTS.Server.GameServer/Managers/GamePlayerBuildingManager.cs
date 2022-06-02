using DarkRift;
using DarkRift.Server;
using RTS.Configuration;
using RTS.Database;
using RTS.Models;
using RTS.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.GameServer
{
    public static class GamePlayerBuildingManager
    {
        /// <summary>
        /// Create a PlayerBuilding
        /// </summary>
        /// <param name="pThread"></param>
        /// <param name="pClient"></param>
        /// <param name="pMessage"></param>
        public static void CreatePlayerBuilding(ThreadBase pThread, IClient pClient, PlayerBuildingCreationRequestMessage pMessage)
        {
            try
            {
                //If player is authorized
                if (GamePlayerCommunicationPlugin.Instance.CheckClientToken(pMessage.playerId, pMessage.token))
                {
                    ClientInformation client = GamePlayerCommunicationPlugin.Instance.GetClientByPlayerId(pMessage.playerId);
                    if (client == null)
                        throw new Exception("[RTS Info] : There is no client for the player " + client.Simulation.Player.id);

                    bool canCreateBuilding = true;

                    //Response to be sent to the player
                    PlayerBuildingCreationResponseMessage responseData = null;

                    //There is a building already on this map element
                    PlayerBuildingModel playerBuilding = client.Simulation.BuildingManager.GetPlayerBuildingByElementInstanceId(pMessage.mapExtentId, pMessage.mapElementInstanceId);

                    if (playerBuilding != null)
                    {
                        canCreateBuilding = false;

                        responseData = new PlayerBuildingCreationResponseMessage
                        {
                            isBuidlingValid = false,
                            buildingNumber = 0,
                            mapExtentId = pMessage.mapExtentId,
                            mapElementInstanceId = pMessage.mapElementInstanceId,
                        };
                    }


                    client.Simulation.Simulate();

                    BuildingModel buildingToCreate = BuildingData.GetBuildingById(pMessage.buildingId);
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
                                playerBuildingProducerToCreate.mapExtentId = pMessage.mapExtentId;
                                playerBuildingProducerToCreate.mapElementInstanceId = pMessage.mapElementInstanceId;

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
                                playerBuildingPassiveToCreate.mapExtentId = pMessage.mapExtentId;
                                playerBuildingPassiveToCreate.mapElementInstanceId = pMessage.mapElementInstanceId;

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
                            mapExtentId = pMessage.mapExtentId,
                            mapElementInstanceId = pMessage.mapElementInstanceId,
                        };
                    }

                    //Send the response if there is one to send
                    if (responseData != null)
                    {
                        using (Message response = Message.Create(CommunicationTag.PlayerBuildings.CREATE_PLAYER_RESOURCE_BUILDING_RESPONSE, responseData))
                        {
                            pClient.SendMessage(response, SendMode.Reliable);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }

        /// <summary>
        /// Send all buildings to the client
        /// </summary>
        /// <param name="e"></param>
        public static void SendAllBuildings(ThreadBase pThread, IClient pClient, PlayerBuildingListRequestMessage pMessage)
        {
            try
            {
                if (GamePlayerCommunicationPlugin.Instance.CheckClientToken(pMessage.playerId, pMessage.token))
                {
                    ClientInformation client = GamePlayerCommunicationPlugin.Instance.GetClientByPlayerId(pMessage.playerId);
                    if (client == null)
                        throw new Exception("[RTS Info] : There is no client for the player " + client.Simulation.Player.id);

                    ////////////////////////////
                    // Building center

                    PlayerBuildingCenterMessage buildingCenterMessageData = new PlayerBuildingCenterMessage
                    {
                        PlayerBuilding = PlayerBuildingCenterFactory.Get(pThread.DBConnection.Connection, pMessage.playerId),
                        playerId = pMessage.playerId,
                        token = pMessage.token
                    };
                   

                    if (buildingCenterMessageData.PlayerBuilding == null)
                        throw new Exception("There is no player center for player " + pMessage.playerId);

                    //Create and send message
                    using (Message buildingCenterMessage = Message.Create(CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_CENTER_RESPONSE, buildingCenterMessageData))
                    {
                        pClient.SendMessage(buildingCenterMessage, SendMode.Reliable);
                    }


                    ////////////////////////////
                    // Buildings producer

                    //Create the message 
                    PlayerBuildingProducerListResponseMessage producerBuildingsMessageData = new PlayerBuildingProducerListResponseMessage()
                    {
                        buildings = PlayerBuildingProducerFactory.GetAllBuildings(pThread.DBConnection.Connection, pMessage.playerId),
                        playerId = pMessage.playerId,
                        token = pMessage.token,
                    };

                    //Create and send message
                    using (Message BuildingsResourceProducerMessage = Message.Create(CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_PRODUCER_RESPONSE, producerBuildingsMessageData))
                    {
                        pClient.SendMessage(BuildingsResourceProducerMessage, SendMode.Reliable);
                    }

                    ////////////////////////////
                    // Buildings passive

                    //Create the message 
                    PlayerBuildingPassiveListResponseMessage passiveBuildingsMessageData = new PlayerBuildingPassiveListResponseMessage
                    {
                        playerBuildings = PlayerBuildingPassiveFactory.GetAllBuildings(pThread.DBConnection.Connection, pMessage.playerIdBuildings),
                        playerId = pMessage.playerId,
                        token = pMessage.token
                    };

                  

                    //Create and send message
                    using (Message BuildingsResourcePassiveMessage = 
                        Message.Create(CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_PASSIVE_RESPONSE, 
                        passiveBuildingsMessageData))
                    {
                        pClient.SendMessage(BuildingsResourcePassiveMessage, SendMode.Reliable);
                    }

                }
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }

        /// <summary>
        /// Destroy a building in the Database
        /// </summary>
        /// <param name="e"></param>
        public static void DestroyPlayerBuilding(ThreadBase pThread, IClient pClient, PlayerBuildingDestroyRequest pMessage)
        {
            try
            {
                if (GamePlayerCommunicationPlugin.Instance.CheckClientToken(pMessage.playerId, pMessage.token))
                {
                    ClientInformation client = GamePlayerCommunicationPlugin.Instance.GetClientByPlayerId(pMessage.playerId);
                    if (client == null)
                        throw new Exception("[RTS Info] : There is no player simulation for the player " + client.Simulation.Player.id);

                    //Simulate player to update informations
                    client.Simulation.Simulate();
                    client.Simulation.Persist(pThread.DBConnection.Connection);

                    switch (pMessage.buildingTypeId)
                    {
                        case (int)TypeOfBuilding.BUILDING_PRODUCER:
                            if (PlayerBuildingProducerFactory.Delete(pThread.DBConnection.Connection, pMessage.playerId, pMessage.positionOnMap, pMessage.buildingNumber))
                            {
                                Console.WriteLine(string.Format("[RTS] INFO: Destroy {0}, by player {1}", BuildingData.GetBuildingById(pMessage.buildingId).name, pMessage.playerId));
                                client.Simulation.RemoveBuilding(pMessage.buildingNumber);
                            }
                            break;

                        case (int)TypeOfBuilding.BUILDING_PASSIVE:
                            if (PlayerBuildingPassiveFactory.Delete(pThread.DBConnection.Connection, pMessage.playerId, pMessage.positionOnMap, pMessage.buildingNumber))
                            {
                                Console.WriteLine(string.Format("[RTS] INFO: Destroy {0}, by player {1}", BuildingData.GetBuildingById(pMessage.buildingId).name, pMessage.playerId));
                                client.Simulation.RemoveBuilding(pMessage.buildingNumber);
                            }
                            break;

                        default:
                            Console.WriteLine("[RTS] INFO: TypeOfBuilding not correct");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }

        }

        /// <summary>
        /// Update the Building Level in DB
        /// </summary>
        /// <param name="e"></param>
        public static void IncrementPlayerBuildingLevel(ThreadBase pThread, IClient pClient, PlayerBuildingUpdatelLevelRequest pMessage)
        {
            try
            {
                //If player is authorized
                if (GamePlayerCommunicationPlugin.Instance.CheckClientToken(pMessage.playerId, pMessage.token))
                {
                    ClientInformation client = GamePlayerCommunicationPlugin.Instance.GetClientByPlayerId(pMessage.playerId);
                    if (client == null)
                        throw new Exception("[RTS Info] : There is no player simulation for the player " + client.Simulation.Player.id);

                    //Simulate player to update informations
                    client.Simulation.Simulate();

                    PlayerBuildingModel playerBuilding = client.Simulation.BuildingManager.GetPlayerBuildingByNumber(pMessage.buildingNumber);
                    if (playerBuilding == null)
                        throw new Exception("The player has no building with number " + pMessage.buildingNumber);

                    if (playerBuilding.CanLevelUp())
                    {
                        client.Simulation.BuildingManager.GetPlayerBuildingByNumber(pMessage.buildingNumber).IncrementLevel(DateTime.Now);
                        Console.WriteLine(string.Format("[RTS] INFO : Building number {0}, buildingtype : BuildingProducer,  Level Up by player {1}",
                                pMessage.buildingNumber, pMessage.playerId));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }

        /// <summary>
        /// Update player building producer message
        /// </summary>
        /// <param name="e"></param>
        public static void ActivatePlayerBuilding(ThreadBase pThread, IClient pClient, PlayerBuildingMessage pMessage)
        {
            try
            {
                //If player is authorized
                if (GamePlayerCommunicationPlugin.Instance.CheckClientToken(pMessage.playerId, pMessage.token))
                {
                    ClientInformation client = GamePlayerCommunicationPlugin.Instance.GetClientByPlayerId(pMessage.playerId);
                    if (client == null)
                        throw new Exception("[RTS Info] : There is no player simulation for the player " + client.Simulation.Player.id);

                    //Simulate player to update informations
                    client.Simulation.Simulate();

                    PlayerBuildingModel playerBuildingProducer = client.Simulation.BuildingManager.GetPlayerBuildingByNumber(pMessage.PlayerBuilding.buildingNumber);

                    //Check if the player has enough resource
                    if (playerBuildingProducer.CanActivate())
                    {
                        playerBuildingProducer.Activate(DateTime.Now);
                        Console.WriteLine("[RTS] INFO : Player building producer N° " + playerBuildingProducer.buildingNumber + " activated successfully");
                    }
                    else
                        Console.WriteLine("[RTS] INFO : Player building producer N° " + playerBuildingProducer.buildingNumber + " NOT activated successfully");

                }
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }

        /// <summary>
        /// Pause the player building. Set state to PAUSED
        /// </summary>
        /// <param name="e"></param>
        public static void PausePlayerBuilding(ThreadBase pThread, IClient pClient, PlayerBuildingMessage pMessage)
        {
            try
            {
                //If player is authorized
                if (GamePlayerCommunicationPlugin.Instance.CheckClientToken(pMessage.playerId, pMessage.token))
                {
                    ClientInformation client = GamePlayerCommunicationPlugin.Instance.GetClientByPlayerId(pMessage.playerId);
                    if (client == null)
                        throw new Exception("[RTS Info] : There is no player simulation for the player " + client.Simulation.Player.id);

                    //Simulate player to update informations
                    client.Simulation.Simulate();

                    PlayerBuildingModel playerBuildingProducer = client.Simulation.BuildingManager.GetPlayerBuildingByNumber(pMessage.PlayerBuilding.buildingNumber);

                    //Check if the player has enough resource
                    if (playerBuildingProducer.CanPause())
                    {
                        playerBuildingProducer.Pause(DateTime.Now);
                    }

                    Console.WriteLine("[RTS] INFO : Player building producer N° " + playerBuildingProducer.buildingNumber + "paused successfully");
                }
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }
    }
}

