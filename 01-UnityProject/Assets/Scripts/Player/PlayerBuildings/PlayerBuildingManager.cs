using DarkRift;
using DarkRift.Client;
using RTS.Server.Messages;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

public class PlayerBuildingManager : MonoBehaviourSingletonNonPersistent<PlayerBuildingManager>
{
    #region Properties

    /// <summary>
    /// List of buildings gameobject
    /// </summary>
    public List<PlayerBuildingBase> buildings = null;

    /// <summary>
    /// Manager of player buildings
    /// </summary>
    public PlayerBuildingManagerModel model = null;

    #endregion

    #region Implementation Common

    /// <summary>
    /// Use this function to initialize the player building manager
    /// </summary>
    public void Init()
    {
        if (GameClientManager.instance.Client)
        {
            //Listen for message
            GameClientManager.instance.Client.MessageReceived += OnMessageReceived;

            //Request buildings from the server
            RequestAllBuildings();

            //Initialize components
            model = new PlayerBuildingManagerModel(PlayerManager.instance.Player);
            buildings = new List<PlayerBuildingBase>();

            //Suscribe to master manager events
            model.OnBuildingCreated += OnBuildingCreated;
            model.OnBuildingCreationAborted += OnBuildingCreationAborted;
            model.OnBuildingDestroyed += OnBuildingDestroyed;

        }
        else
        {
            throw new Exception("There is no client active");
        }
    }

    private void OnBuildingDestroyed(PlayerBuildingManagerModel pPlayerBuildingManager, PlayerBuildingModel pBuilding)
    {
        Debug.Log("model event OnBuildingDestroyed");
        pBuilding.Destroy();
    }

    private void OnBuildingCreationAborted(PlayerBuildingManagerModel pPlayerBuildingManager, PlayerBuildingModel pBuilding)
    {
        Debug.Log("model event OnBuildingCreationAborted");
    }

    private void OnBuildingCreated(PlayerBuildingManagerModel pPlayerBuildingManager, PlayerBuildingModel pBuilding)
    {
        Debug.Log("model event OnBuildingCreated");
    }

    /// <summary>
    /// Returns true if the component is initialized
    /// </summary>
    /// <returns></returns>
    public bool IsInitialized()
    {
        if (model != null)
            return true;

        return false;
    }

    /// <summary>
    /// Create a building and send a message to the server
    /// </summary>
    /// <param name="pBuildingId"></param>
    /// <param name="pSpot"></param>
    public void CreatePlayerBuilding(int pBuildingId, MapElementBuildingSpot pSpot)
    {
        if (GameClientManager.instance.Client)
        {
            if (GameClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
            {
                //Get building
                BuildingBase building = GameBuildingManager.instance.GetBuilding(pBuildingId);

                // Check if enough resource(s) and consumme it/them
                if (PlayerManager.instance.Player.resourceBag.HasEnoughResource(building.Model.cost))
                {
                    //Create The Building
                    GameObject buildingGameObject = Instantiate(building.playerBuildingPrefab, pSpot.transform);

                    //Get the player building
                    PlayerBuildingBase playerBuilding = buildingGameObject.GetComponent<PlayerBuildingBase>();
                    playerBuilding.Init();

                    // Initalise the Spot with the building inside
                    pSpot.SetBuilding(playerBuilding);

                    // Calculate the appropriate location string
                    playerBuilding.Model.mapExtentId = pSpot.MapExtentElementModel.Extent.id;
                    playerBuilding.Model.mapElementInstanceId = pSpot.MapExtentElementModel.InstanceId;

                    //Add Building To the List of Buildings                    
                    buildings.Add(playerBuilding);
                    model.Create(playerBuilding.Model, ClockManager.instance.time);

                    //Send server request
                    SendPlayerBuildingCreationRequest(playerBuilding);
                }
                else
                {
                    Debug.Log("Not enough resources for building : " + building.Model.name);
                }


            }
        }
    }

    /// <summary>
    /// this function is called when we receive a message from the server
    /// </summary>
    /// <param name="e"></param>
    private void ValidatePlayerBuilding(MessageReceivedEventArgs e)
    {
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {
            PlayerBuildingCreationResponseMessage response = e.GetMessage().Deserialize<PlayerBuildingCreationResponseMessage>();

            PlayerBuildingModel playerBuildingModel = model.GetPlayerBuildingByElementInstanceId(response.mapExtentId, response.mapElementInstanceId);

            if (playerBuildingModel == null)
                throw new Exception("There is no building model number N°" + playerBuildingModel.buildingNumber + " for the player");

            if (response.isBuidlingValid)
            {
                playerBuildingModel.buildingNumber = response.buildingNumber;
            }
            else
            {
                PlayerBuildingBase playerBuildingBase = buildings.Where(x =>
                x.Model.mapExtentId == response.mapExtentId && x.Model.mapElementInstanceId == response.mapElementInstanceId
                && x.Model.buildingNumber == 0).FirstOrDefault();

                if (playerBuildingBase == null)
                    throw new Exception("There is no building number N°" + playerBuildingModel.buildingNumber + " for the player");

                //Remove data from the manager
                model.RemoveByElementInstanceId(response.mapExtentId, response.mapElementInstanceId);

                //Remove gameobject from the scene
                buildings.Remove(playerBuildingBase);
                GameObject.Destroy(playerBuildingBase.gameObject);
            }
        }
    }

    /// <summary>
    /// Destroy One Building 
    /// </summary>
    /// <param name="pbuilding"></param>
    public void DestroyBuilding(PlayerBuildingBase pBuilding)
    {
        if (buildings.Contains(pBuilding))
        {
            if (GameClientManager.instance.Client)
            {
                if (GameClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
                {
                    PlayerBuildingDestroyRequest destroyRequest = new PlayerBuildingDestroyRequest
                    {
                        playerId = PlayerManager.instance.Player.id,
                        token = PlayerManager.instance.Player.CurrentToken,

                        //Building
                        buildingId = pBuilding.Model.Building.id,
                        positionOnMap = "",
                        buildingNumber = pBuilding.Model.buildingNumber,
                        buildingTypeId = pBuilding.Model.Building.buildingType.id
                    };

                    //Send message
                    using (Message destroyMessage = Message.Create(CommunicationTag.PlayerBuildings.DESTROY_PLAYER_BUILDING_REQUEST, destroyRequest))
                    {
                        GameClientManager.instance.Client.SendMessage(destroyMessage, SendMode.Reliable);
                    }

                    //Destroy building number into the manager
                    model.RemoveByNumber(pBuilding.Model.buildingNumber);

                    //Remove from the user scene
                    buildings.Remove(pBuilding);
                    GameObject.Destroy(pBuilding.gameObject);
                }
            }
            else
                Debug.LogError("Building To Delete Not Found");

        }
    }

    #endregion

    #region Building Center

    /// <summary>
    /// Setup buildings
    /// </summary>
    /// <param name="e"></param>
    public void SetupBuildingCenterFromServer(MessageReceivedEventArgs e)
    {
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {
            PlayerBuildingCenterMessage response = e.GetMessage().Deserialize<PlayerBuildingCenterMessage>();

            if (response.PlayerBuilding != null)
            {
                //Get building scriptable object
                BuildingBase building = GameBuildingManager.instance.GetBuilding(response.PlayerBuilding.Building.id);
                GameObject playerBuildingGameObject = Instantiate((GameObject)building.playerBuildingPrefab);

                MapExtentElementModel buildingSpotElement = PlayerMapManager.instance.MapExtentManager.Model.GetMapElementByInstanceId(response.PlayerBuilding.mapExtentId, response.PlayerBuilding.mapElementInstanceId);
                if (buildingSpotElement == null)
                    throw new Exception("There is no map element with this ID " + response.PlayerBuilding.mapElementInstanceId + " on this extent N° " + response.PlayerBuilding.mapExtentId);
                

                playerBuildingGameObject.transform.position = StringToVector.StrToVector3(buildingSpotElement.Position);


                PlayerBuildingCenter playerBuilding = playerBuildingGameObject.GetComponent<PlayerBuildingCenter>();

                //Update player building model 
                playerBuilding.Init(response.PlayerBuilding);

                //Set building model from game building manager
                playerBuilding.Model.Building = building.Model as BuildingCenterModel;
                //Set player
                playerBuilding.Model.Player = PlayerManager.instance.Player;
                //Update state with full data 
                playerBuilding.Model.State = BuildingStateData.GetStateById(response.PlayerBuilding.State.id);
                //Update level
                playerBuilding.Model.Level = building.Model.Levels[response.PlayerBuilding.Level.id - 1] as BuildingCenterLevelModel;

                //Add Building To the List of Buildings
                buildings.Add(playerBuilding);
                model.Add(playerBuilding.Model);
            }
            else
            {
                throw new Exception("There is no player center received");
            }
        }
    }

    #endregion

    #region Building Passive

    /// <summary>
    /// Setup buildings
    /// </summary>
    /// <param name="e"></param>
    public void SetupBuildingPassiveFromServer(MessageReceivedEventArgs e)
    {
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {
            PlayerBuildingPassiveListResponseMessage response = e.GetMessage().Deserialize<PlayerBuildingPassiveListResponseMessage>();

            if (response.playerBuildings != null)
            {
                MapElementBuildingSpot[] spots = FindObjectsOfType<MapElementBuildingSpot>();

                for (int i = 0; i < response.playerBuildings.Count; i++)
                {
                    MapExtentElementModel buildingSpotElement = PlayerMapManager.instance.MapExtentManager.Model.GetMapElementByInstanceId(response.playerBuildings[i].mapExtentId, response.playerBuildings[i].mapElementInstanceId);
                    if (buildingSpotElement == null)
                        throw new Exception("There is no map element with this ID " + response.playerBuildings[i].mapElementInstanceId + " on this extent N° " + response.playerBuildings[i].mapExtentId);
                    
                    for (int y = 0; y < spots.Length; y++)
                    {
                        if (buildingSpotElement.Position == StringToVector.Vec3ToStr(spots[y].transform.position))
                        {
                            //Get building scriptable object
                            BuildingBase building = GameBuildingManager.instance.GetBuilding(response.playerBuildings[i].Building.id);
                            GameObject playerBuildingGameObject = Instantiate((GameObject)building.playerBuildingPrefab, spots[y].transform);


                            PlayerBuildingPassive playerBuilding = playerBuildingGameObject.GetComponent<PlayerBuildingPassive>();

                            //Update player building model 
                            playerBuilding.Init(response.playerBuildings[i]);
                           
                            ////Set building model from game building manager
                            playerBuilding.Model.Building = building.Model as BuildingPassiveModel;

                            //Set player
                            playerBuilding.Model.Player = PlayerManager.instance.Player;
                            //Update state with full data 
                            playerBuilding.Model.State = BuildingStateData.GetStateById(response.playerBuildings[i].State.id);
                            //Update level
                            playerBuilding.Model.Level = building.Model.Levels[response.playerBuildings[i].Level.id - 1] as BuildingPassiveLevelModel;

                            spots[y].SetBuilding(playerBuilding);

                            //Add Building To the List of Buildings
                            buildings.Add(playerBuilding);
                            model.Add(playerBuilding.Model);
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Building Producer    

    /// <summary>
    /// Setup buildings
    /// </summary>
    /// <param name="e"></param>
    public void SetupBuildingProducerFromServer(MessageReceivedEventArgs e)
    {
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {
            PlayerBuildingProducerListResponseMessage response = e.GetMessage().Deserialize<PlayerBuildingProducerListResponseMessage>();

            if (response.buildings != null)
            {
                //TODO : passer par un manager
                MapElementBuildingSpot[] spots = FindObjectsOfType<MapElementBuildingSpot>();

                for (int i = 0; i < response.buildings.Count; i++)
                {
                    MapExtentElementModel buildingSpotElement = PlayerMapManager.instance.MapExtentManager.Model.GetMapElementByInstanceId(response.buildings[i].mapExtentId, response.buildings[i].mapElementInstanceId);
                    if (buildingSpotElement == null)
                        throw new Exception("There is no map element with this ID " + response.buildings[i].mapElementInstanceId + " on this extent N° " + response.buildings[i].mapExtentId);


                    for (int y = 0; y < spots.Length; y++)
                    {
                        if (buildingSpotElement.Position == StringToVector.Vec3ToStr(spots[y].transform.position))
                        {
                            //Get building scriptable object
                            BuildingBase building = GameBuildingManager.instance.GetBuilding(response.buildings[i].Building.id);
                            GameObject playerBuildingGameObject = Instantiate((GameObject)building.playerBuildingPrefab, spots[y].transform);

                            PlayerBuildingProducer playerBuilding = playerBuildingGameObject.GetComponent<PlayerBuildingProducer>();

                            //Update player building model 
                            playerBuilding.Init(response.buildings[i]);

                            ////Set building model from game building manager
                            playerBuilding.Model.Building = building.Model as BuildingProducerModel;

                            //Set player
                            playerBuilding.Model.Player = PlayerManager.instance.Player;
                            //Update state with full data 
                            playerBuilding.Model.State = BuildingStateData.GetStateById(response.buildings[i].State.id);

                            //Update level
                            playerBuilding.Model.Level = building.Model.Levels[response.buildings[i].Level.id - 1] as BuildingProducerLevelModel;

                            spots[y].SetBuilding(playerBuilding);

                            //Add Building To the List of Buildings
                            buildings.Add(playerBuilding);
                            model.Add(playerBuilding.Model);

                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Network Messages

    /// <summary>
    /// On message received from the server
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        switch (e.Tag)
        {
            case CommunicationTag.PlayerBuildings.CREATE_PLAYER_RESOURCE_BUILDING_RESPONSE:
                ValidatePlayerBuilding(e);
                break;
            case CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_PRODUCER_RESPONSE:
                SetupBuildingProducerFromServer(e);
                break;
            case CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_PASSIVE_RESPONSE:
                SetupBuildingPassiveFromServer(e);
                break;
            case CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_CENTER_RESPONSE:
                SetupBuildingCenterFromServer(e);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Send a message to the server to request all buildings
    /// </summary>
    private void RequestAllBuildings()
    {
        if (GameClientManager.instance.Client)
        {
            if (GameClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
            {
                PlayerBuildingListRequestMessage messageData = new PlayerBuildingListRequestMessage
                {
                    playerId = PlayerManager.instance.Player.id,
                    token = PlayerManager.instance.Player.CurrentToken,
                    playerIdBuildings = PlayerManager.instance.Player.id,
                };

                //Send message
                using (Message message = Message.Create(CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_REQUEST, messageData))
                {
                    GameClientManager.instance.Client.SendMessage(message, SendMode.Reliable);
                }
            }
        }
    }

    /// <summary>
    /// Send a player building creation request to the server
    /// </summary>
    /// <param name="pBuilding"></param>
    private void SendPlayerBuildingCreationRequest(PlayerBuildingBase pBuilding)
    {
        // Message request data
        PlayerBuildingCreationRequestMessage buildRequest = new PlayerBuildingCreationRequestMessage
        {
            //Credentials 
            playerId = PlayerManager.instance.Player.id,
            token = PlayerManager.instance.Player.CurrentToken,

            //Building
            buildingId = pBuilding.Model.Building.id,
            mapExtentId = pBuilding.Model.mapExtentId,
            mapElementInstanceId = pBuilding.Model.mapElementInstanceId,
        };

        //Send message
        using (Message buildMessage = Message.Create(CommunicationTag.PlayerBuildings.CREATE_PLAYER_BUILDING_REQUEST, buildRequest))
        {
            GameClientManager.instance.Client.SendMessage(buildMessage, SendMode.Reliable);
        }
    }

    #endregion

    #region Unity callbacks

    /// <summary>
    /// On destroy, unsuscribe events
    /// </summary>
    public override void OnDestroy()
    {
        //Listen for message
        GameClientManager.instance.Client.MessageReceived -= OnMessageReceived;
    }

    #endregion
}

