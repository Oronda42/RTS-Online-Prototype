using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using UnityEngine;
using Utilities;
using RTS.Server.Messages;
using RTS.Models;
using System.Linq;
using RTS.Configuration;

public class PlayerMapManager: MonoBehaviourSingletonNonPersistent<PlayerMapManager>
{
    #region Properties

    /// <summary>
    /// Model of the player map
    /// </summary>
    public PlayerMapModel Model;

    /// <summary>
    /// Map extent manager
    /// </summary>
    public MapExtentManager MapExtentManager
    {
        get { return mapExtentManager; }
        set { Debug.Break(); mapExtentManager = value; }
    }
    public MapExtentManager mapExtentManager;

    #endregion

    #region Unity Callbaks


    #endregion

    #region Implementation

    /// <summary>
    /// Initialize the component
    /// </summary>
    public void Init(PlayerManager pPlayerManager)
    {
        if (MapExtentManager == null)
            throw new System.Exception("there is no MapExtentManager filled in the inspector");

        MapExtentManager.Init();

        //Initialization of the model
        Model = new PlayerMapModel
        {
            owner = pPlayerManager.Player
        };

        //Request server for data
        if (GameClientManager.instance.Client)
        {
            //Listen for message
            GameClientManager.instance.Client.MessageReceived += OnMessageReceived;

            //Ask map
            SendPlayerMapRequest();
        }
        else
        {
            throw new Exception("There is no client active");
        }
    }


    /// <summary>
    /// Returns true if the component is initialized
    /// </summary>
    public bool IsInitialized()
    {
        if (Model.Extents != null)
            return true;

        return false;
    }

    /// <summary>
    /// Send a message to the server and request the player map
    /// </summary>
    private void SendPlayerMapRequest()
    {
        if (GameClientManager.instance.Client)
        {
            if (GameClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
            {
                CredentialMessage playerMapRequest = new CredentialMessage
                {
                    playerId = PlayerManager.instance.Player.id,
                    token = PlayerManager.instance.Player.CurrentToken
                };

                using (Message playerMapMessage = Message.Create(CommunicationTag.PlayerMap.PLAYER_MAP_REQUEST, playerMapRequest))
                {
                    GameClientManager.instance.Client.SendMessage(playerMapMessage, SendMode.Reliable);
                }
            }
        }

    }

    /// <summary>
    /// Display the player map
    /// </summary>
    /// <param name="e"></param>
    private void HandlePlayerMapResponse(MessageReceivedEventArgs e)
    {
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {
            PlayerMapModel playerMap = e.GetMessage().Deserialize<PlayerMapResponseMessage>().PlayerMap;

            //Map successfully loaded
            if (playerMap.owner != null)
            {
                //Set data into the model
                Model.name = playerMap.name;
                Model.Extents = playerMap.Extents;

                //For each map extents owned by the player              
                for (int i = 0; i < playerMap.Extents.Count; i++)
                {
                    //Browse each elements specific to the player
                    for (int j = 0; j < playerMap.Extents[i].PlayerElements.Count; j++)
                    {
                        //Get the map extent and activate it
                        MapExtent mapExtent = MapExtentManager.Extents.Where(ex => ex.Model.id == playerMap.Extents[i].Extent.id).FirstOrDefault();
                        if (mapExtent)
                        {
                            mapExtent.gameObject.SetActive(true);
                        }
                        else
                        {
                            Debug.LogError("MAP EXTENT IS NULL");
                           // return;
                        }
                       

                        MapElementBase mapElement = mapExtent.Elements.Where(el => el.MapExtentElementModel.InstanceId == playerMap.Extents[i].PlayerElements[j].mapElementInstanceId).FirstOrDefault();

                        switch (mapElement.MapExtentElementModel.Element.Type.Id)
                        {
                            case (int)TypeOfMapElement.RESOURCE:
                                mapElement.SpriteRenderer.sprite = GameResourceManager.instance.GetResource(playerMap.Extents[i].PlayerElements[j].EntityId).sprite;
                                break;
                        }
                    }
                }

            }
            else
            {
                Debug.Log("No map loaded");
            }
        }
    }

    //TODO : A virer c'est moche
    public void OnbuttonClick()
    {
        StartCoroutine(ApplicationManager.instance.WaitingSceneLoading(Constants.Scenes.PlayerWorld.PLAYER_WORLD));
    }

    #endregion

    #region Network Messages

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        switch (e.Tag)
        {
            case CommunicationTag.PlayerMap.PLAYER_MAP_RESPONSE:
                HandlePlayerMapResponse(e);
                break;
            default:
                break;
        }
    }

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
