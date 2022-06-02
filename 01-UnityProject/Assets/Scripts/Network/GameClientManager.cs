using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using RTS.Server.Messages;
using RTS.Configuration;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using System.Net;

#region Delegates
public delegate void GameClientManagerHandler(GameClientManager pClient);
#endregion

public class GameClientManager : MonoBehaviourSingletonPersistent<GameClientManager>
{
    #region Events
    public event GameClientManagerHandler OnConnected, OnDisconnected, OnLoggedIn ;
    #endregion

    #region Properties

    /// <summary>
    /// Reference to the DarkRift network client
    /// </summary>
    public UnityClient Client { get; set; }

    /// <summary>
    /// Name of the game server where the player is connected
    /// </summary>
    public string GameServerName { get; set; }

    #endregion

    #region Implementation

    /// <summary>
    /// Initialize the component
    /// </summary>
    public void Init(string pAdress, int pPort, string pServerName)
    {
        Client = GetComponent<UnityClient>();
        
        //Update information
        Client.Address = IPAddress.Parse(pAdress);
        ushort.TryParse(pPort.ToString(), out ushort parsedPort);
        Client.Port = parsedPort;
        GameServerName = pServerName;

        Client.MessageReceived += OnClientMessageReceived;
    }


    /// <summary>
    /// Connects to the DarkRift Server and login automatically
    /// </summary>
    public void ConnectAndLogin()
    {
        try
        {
            Client.Connect(Client.Address, Client.Port, Client.IPVersion);
            OnConnected?.Invoke(this);

            LoginRequest(PlayerManager.instance.Player.id, PlayerManager.instance.Player.CurrentToken);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
        

    /// <summary>
    /// Handle The messages receive by the client from the server
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        switch (e.Tag)
        {
            case CommunicationTag.Connection.LOGIN_RESPONSE:
                OnLoginResponseReceived(e);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// Login to the server with a device id
    /// </summary>
    /// <param name="pPlayerId"></param>
    public void LoginRequest(int pPlayerId, string pToken)
    {
        if (Client != null)
        {
            if (Client.ConnectionState == ConnectionState.Connected)
            {
                CredentialMessage loginRequest = new CredentialMessage
                {
                    SerializeCredentials = true,
                    playerId = pPlayerId,
                    token = pToken
                };

                using (Message loginMessage = Message.Create(CommunicationTag.Connection.DEVICE_IDENTIFICATION_REQUEST, loginRequest))
                {
                    Client.SendMessage(loginMessage, SendMode.Reliable);
                }
            }
        }
    }
       

    /// <summary>
    /// Login After server confirmation
    /// </summary>
    /// <param name="e"></param>
    private void OnLoginResponseReceived(MessageReceivedEventArgs e)
    {
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {

            LoginResponseMessage loginResponse = e.GetMessage().Deserialize<LoginResponseMessage>();

            switch (loginResponse.loginErrorCode)
            {
                //Connexion sucessfull
                case (int)LoginErrorCode.NO_ERROR:
                    Debug.LogFormat("Succesfuly Connected To GameServer : {0}", GameServerName);
                    OnLoggedIn?.Invoke(this);
                    break;

                default:
                    Debug.LogFormat("Error when log in. Disconnection");
                    OnDisconnected?.Invoke(this);
                    break;
            }
        }
    }

    

    #endregion
}
