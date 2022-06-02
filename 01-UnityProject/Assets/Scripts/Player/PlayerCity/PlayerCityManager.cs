using UnityEngine;
using RTS.Models;
using System.Collections.Generic;
using DarkRift.Client;
using System;
using RTS.Server.Messages;
using DarkRift;
using Utilities;
using System.Linq;

class PlayerCityManager : MonoBehaviourSingletonPersistent<PlayerCityManager>
{
    PlayerCityManagerModel model = null;

    List<PlayerCity> neighboorsCities = null;

    PlayerCity playerCity;

    GameObject playerCityPrefab;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        GetBundle();

        if (WorldClientManager.instance.Client)
        {
            //Listen for message
            WorldClientManager.instance.Client.MessageReceived += OnMessageReceived;

            //Request Cities from server
            RequestPlayerCityFromServer();
            RequestNeighboorsPlayerCitiesFormServer();

            //Initialize components
            model = new PlayerCityManagerModel();
            neighboorsCities = new List<PlayerCity>();

            //Suscribe to master manager events
            //model.OnCityAdded += OnCityAdded;
            //model.OnCityDeleted += OnCityDeleted;
        }
        else
        {
            throw new Exception("There is no client active");
        }
    }
    private void GetBundle()
    {
        playerCityPrefab = (GameObject)AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.CITIES, Constants.Cities.PLAYER_CITY);
    }
    public void RequestNeighboorsPlayerCitiesFormServer()
    {
        if (WorldClientManager.instance.Client)
        {
            if (WorldClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
            {
                CredentialMessage messageData = new CredentialMessage
                {
                    playerId = PlayerManager.instance.Player.id,
                    token = PlayerManager.instance.Player.CurrentToken,
                };

                //Send message
                using (Message message = Message.Create(CommunicationTag.PlayerCity.NEIGHBOORS_CITY_REQUEST, messageData))
                {
                    WorldClientManager.instance.Client.SendMessage(message, SendMode.Reliable);
                }
            }
        }
    }
    public void SetupCitiesFromServer(MessageReceivedEventArgs e)
    {
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {
            NeighboorsCityResponseMessage response = e.GetMessage().Deserialize<NeighboorsCityResponseMessage>();
            
            if (response.neighboorsCities != null)
            {
                for (int i = 0; i < response.neighboorsCities.Count; i++)
                {
                    CreateCityFromServer(response.neighboorsCities[i]);
                }
            }
        }
    }
    private void CreateCityFromServer(PlayerCityModel pPlayerCityModel)
    {
        Vector2 position = StringToVector.StrToVector2(pPlayerCityModel.position);
        GameObject playerCityGo = Instantiate(playerCityPrefab, position, Quaternion.identity);
        PlayerCity playerCity = playerCityGo.GetComponent<PlayerCity>();
        playerCity.playerCityModel = pPlayerCityModel;
        playerCity.Init();
    }
    public void RequestPlayerCityFromServer()
    {
        if (WorldClientManager.instance.Client)
        {
            if (WorldClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
            {
                CredentialMessage messageData = new CredentialMessage
                {
                    playerId = PlayerManager.instance.Player.id,
                    token = PlayerManager.instance.Player.CurrentToken,
                };

                //Send message
                using (Message message = Message.Create(CommunicationTag.PlayerCity.PLAYER_CITY_REQUEST, messageData))
                {
                    WorldClientManager.instance.Client.SendMessage(message, SendMode.Reliable);
                }
            }
        }
    }
    public void UpdatePlayerCity(MessageReceivedEventArgs e)
    {
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {
            PLayerCityResponseMessage response = e.GetMessage().Deserialize<PLayerCityResponseMessage>();
            if (playerCity == null)
                CreateCityFromServer(response.model);
            else
                playerCity.playerCityModel = response.model;
        }
    }
    public void SetupPlayerCity(MessageReceivedEventArgs e)
    {

    }
    public void CreatePlayerCity()
    {

    }
    public void OnCityAdded()
    {
        Debug.Log("model event OnCityAdded");
    }
    public void OnCityDeleted()
    {
        Debug.Log("model event OnBuidlingAdded");
    }
    public bool IsInitialized()
    {
        if (model != null)
            return true;

        return false;
    }
    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        switch (e.Tag)
        {
            case CommunicationTag.PlayerCity.NEIGHBOORS_CITY_RESPONSE:
                SetupCitiesFromServer(e);
                break;
            case CommunicationTag.PlayerCity.PLAYER_CITY_RESPONSE:
                UpdatePlayerCity(e);
                break;

        }
    }
}

