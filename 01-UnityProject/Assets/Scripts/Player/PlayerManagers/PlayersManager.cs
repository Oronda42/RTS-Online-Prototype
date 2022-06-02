using DarkRift;
using DarkRift.Client;
using RTS.Server.Messages;
using RTS.Models;
using System;
using System.Collections.Generic;
using Utilities;
using UnityEngine;

public class PlayersManager : MonoBehaviourSingletonNonPersistent<PlayersManager>
{
    #region Properties

    public List<PlayerModel> players;

    public List<GameObject> avatars;

    #endregion

    #region Implementation

    private void Start()
    {
        if (GameClientManager.instance.Client)
        {
            //Listen for message
            GameClientManager.instance.Client.MessageReceived += OnGameMessageReceived;

            //Instantiate the Local Player
            InstantiatePlayer(PlayerManager.instance.Player);

            //Instantiate nearby Players
            RequestAllPlayers();
        }
    }
    public void RequestAllPlayers()
    {
        if (GameClientManager.instance.Client)
        {
            if (GameClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
            {
                // Message request data
                AllPlayerRequestMessage allPlayerRequest = new AllPlayerRequestMessage
                {
                    position = StringToVector.Vec3ToStr(Vector3.zero),
                    playerId = PlayerManager.instance.Player.id,
                    token = PlayerManager.instance.Player.CurrentToken
                    
                };

                //Send message
                using (Message allPlayerMessage = Message.Create(CommunicationTag.Player.ALL_PLAYERS_REQUEST, allPlayerRequest))
                {
                    Debug.Log("INFO : Request All Players");
                    GameClientManager.instance.Client.SendMessage(allPlayerMessage, SendMode.Reliable);
                }
            }
        }
    }
    public void HandleGetAllPlayersResponse(MessageReceivedEventArgs e)
    {
        Debug.Log("Handle GetAll PlayersResponse");
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {
            AllPlayerResponseMessage allPlayersResponseMessage = e.GetMessage().Deserialize<AllPlayerResponseMessage>();
           
            for (int i = 0; i < allPlayersResponseMessage.players.Count; i++)
            {
                InstantiatePlayer(allPlayersResponseMessage.players[i]);
            }
        }
    }
    public void InstantiatePlayer(PlayerModel pModel)
    {
        //GameObject playerGO = Instantiate(avatars[pModel.avatarId], pModel.localisation.position);
        GameObject playerGO = Instantiate(avatars[0],Vector3.zero,Quaternion.identity);
        playerGO.GetComponent<Player>().model = pModel;
        playerGO.GetComponent<PlayerUI>().Init(pModel);


    }
    public void OnGameMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        switch (e.Tag)
        {
            case CommunicationTag.Player.ALL_PLAYERS_RESPONSE:
                HandleGetAllPlayersResponse(e);
                break;
            default:
                break;
        }
    }

    #endregion



}

