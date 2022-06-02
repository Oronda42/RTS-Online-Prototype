using DarkRift;
using DarkRift.Client;
using RTS.Server.Messages;
using RTS.Models;
using System;
using System.Collections.Generic;
using Utilities;
using UnityEngine;

public class PlayerManager : MonoBehaviourSingletonPersistent<PlayerManager>
{
    #region Properties

    /// <summary>
    /// Representation of the player
    /// </summary>
    public PlayerModel Player { get; set; }



    #endregion
      
    #region Implementation

    public void Init()
    {
        // Construct the player model
        Player = new PlayerModel
        {
            resourceBag = new ResourceBagModel(),
        };
    }

    /// <summary>
    /// Returns true if the component is initialized
    /// </summary>
    /// <returns></returns>
    public bool IsInitialized()
    {
        if (Player.resourceBag.resources.Count > 0)
            return true;

        return false;
    }

    /// <summary>
    /// Request resources from the server
    /// </summary>
    public void RequestPlayerResourcesFromServer()
    {
        if (GameClientManager.instance.Client)
        {
            if (GameClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
            {
                // Message request data
                PlayerResourceRequestMessage resourceRequest = new PlayerResourceRequestMessage
                {
                    playerIdResourceRequest = PlayerManager.instance.Player.id,
                    playerId = PlayerManager.instance.Player.id,
                    token = PlayerManager.instance.Player.CurrentToken
                };

                //Send message
                using (Message resourceMessage = Message.Create(CommunicationTag.PlayerConsumableResources.ALL_RESOURCE_REQUEST, resourceRequest))
                {
                    Debug.Log("INFO : Request Player Resources data");
                    GameClientManager.instance.Client.SendMessage(resourceMessage, SendMode.Reliable);
                }
            }
        }
    }

    /// <summary>
    /// Request resources from the server
    /// </summary>
    //private void RequestPlayerCenterFromServer()
    //{
    //    if (GameClientManager.instance.Client)
    //    {
    //        if (GameClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
    //        {
    //            // Message request data
    //            CredentialMessage centrerRequest = new CredentialMessage
    //            {
    //                playerId = Player.id,
    //                token = PlayerManager.instance.Player.CurrentToken
    //            };

    //            //Send message
    //            using (Message resourceMessage = Message.Create(CommunicationTag.PlayerCenter.PLAYER_CENTER_REQUEST, centrerRequest))
    //            {
    //                Debug.Log("INFO : Request Player Center data");
    //                GameClientManager.instance.Client.SendMessage(resourceMessage, SendMode.Reliable);
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// Receive resource from the server
    /// </summary>
    /// <param name="e"></param>
    private void ReceiveResourcesFromServer(MessageReceivedEventArgs e)
    {
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {
            PlayerResourcesResponseMessage playerResourceBagMessage = e.GetMessage().Deserialize<PlayerResourcesResponseMessage>();
            Player.resourceBag.resources = new List<ResourceBagSlotModel>();
            Player.resourceBag.AddBag(playerResourceBagMessage.ResourceBag);

            ////Resource bag successfully received
            //if (Player.resourceBag.resources != null)
            //{
            //    RequestPlayerCenterFromServer();
            //}
            //else
            //    throw new Exception("[RTS] ERROR : No resource bag loaded");

        }
    }

    #endregion

    #region Network Messages

    public void OnGameMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        switch (e.Tag)
        {
            case CommunicationTag.PlayerConsumableResources.ALL_RESOURCE_RESPONSE:
                ReceiveResourcesFromServer(e);
                break;
            default:
                break;
        }
    }

    #endregion

}
