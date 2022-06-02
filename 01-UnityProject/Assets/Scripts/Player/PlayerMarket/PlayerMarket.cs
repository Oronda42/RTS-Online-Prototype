using DarkRift;
using DarkRift.Client;
using RTS.Server.Messages;
using RTS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

#region Delegates

#endregion

public class PlayerMarket : MonoBehaviourSingletonNonPersistent<PlayerMarket>
{

    #region Properties

    public PlayerMarketModel Model { private set; get; } = null;

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

    #region Network Messages

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        switch (e.Tag)
        {
            case CommunicationTag.PlayerMarket.PLAYER_MARKET_RESPONSE:
                HandlePlayerMarketResponse(e);
                break;
            default:
                break;
        }
    }

    #endregion

    #region Implementation

    public void Init()
    {
        //Request server for data
        if (GameClientManager.instance.Client)
        {
            //Listen for message
            GameClientManager.instance.Client.MessageReceived += OnMessageReceived;

            //Ask player market
            SendPlayerMarketRequest();
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
        if (Model != null)
            return true;

        return false;
    }

    /// <summary>
    /// Send a message to the server and request the player market
    /// </summary>
    private void SendPlayerMarketRequest()
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

                using (Message playerMapMessage = Message.Create(CommunicationTag.PlayerMarket.PLAYER_MARKET_REQUEST, playerMapRequest))
                {
                    GameClientManager.instance.Client.SendMessage(playerMapMessage, SendMode.Reliable);
                }
            }
        }
    }

    /// <summary>
    /// Get the player market
    /// </summary>
    /// <param name="e"></param>
    private void HandlePlayerMarketResponse(MessageReceivedEventArgs e)
    {
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {
            PlayerMarketMessage playerMarket = e.GetMessage().Deserialize<PlayerMarketMessage>();

            //Map successfully loaded
            if (playerMarket.PlayerMarket != null)
            {
                Model = playerMarket.PlayerMarket;
                //Set level data because we receive only the ID from the server
                Model.Level = MarketData.GetMarket().Levels[Model.Level.Id - 1];
                Model.Player = PlayerManager.instance.Player;
            }
            else
            {
                Debug.Log("No player market loaded");
            }
        }
    }

    #endregion
}
