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
using RTS.Models;

#region Delegates
public delegate void ClientEventHandler(LoginClientManager pClient);
#endregion

public class LoginClientManager : MonoBehaviourSingletonPersistent<LoginClientManager>
{

    #region Events
    public event ClientEventHandler OnConnected, OnDisconnected, OnLoggedIn;
    #endregion

    #region Properties

    /// <summary>
    /// Reference to the network client
    /// </summary>
    public UnityClient Client { get; set; }
    
    #endregion

    #region Implementation

    /// <summary>
    /// Initialize the component
    /// </summary>
    public void Init()
    {
        Client = GetComponent<UnityClient>();
        Client.MessageReceived += OnClientMessageReceived;
    }


    /// <summary>
    /// Connects to the DarkRift Server
    /// </summary>
    public void Connect()
    {
        try
        {
            Client.Connect(Client.Address, Client.Port, Client.IPVersion);
            OnConnected?.Invoke(this);
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
    /// <param name="pDeviceId"></param>
    public void LoginWithDeviceRequest(string pDeviceId, string pToken)
    {
        if (Client != null)
        {
            if (Client.ConnectionState == ConnectionState.Connected)
            {
                LoginDeviceRequestMessage loginRequest = new LoginDeviceRequestMessage
                {
                    deviceId = pDeviceId,
                    deviceToken = pToken,
                };

                using (Message loginMessage = Message.Create(CommunicationTag.Connection.DEVICE_IDENTIFICATION_REQUEST, loginRequest))
                {
                    Client.SendMessage(loginMessage, SendMode.Reliable);
                }
            }
        }
    }
    

    /// <summary>
    /// Login response received from the server
    /// </summary>
    /// <param name="e"></param>
    private void OnLoginResponseReceived (MessageReceivedEventArgs e)
    {
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {
            LoginResponseMessage loginResponse = e.GetMessage().Deserialize<LoginResponseMessage>();

            //Update player information
            PlayerManager.instance.Player.id = loginResponse.id;
            PlayerManager.instance.Player.nickname = loginResponse.nickname;
            PlayerManager.instance.Player.CurrentToken = loginResponse.token;
            PlayerManager.instance.Player.resourceBag = new ResourceBagModel();

            switch (loginResponse.loginErrorCode)
            {
                //Connexion sucessfull
                case (int)LoginErrorCode.NO_ERROR:

                    //Initialize the world client manager
                    WorldClientManager.instance.Init(loginResponse.RedirectedServerHost, loginResponse.RedirectedServerPort);
                    WorldClientManager.instance.ConnectAndLogin();

                    OnLoggedIn?.Invoke(this);

                    break;

                case (int)LoginErrorCode.USER_DONT_EXIST:
                    Debug.Log("Login response : User don't exist");
                    SceneManager.LoadScene(Constants.Scenes.PlayerMap.FIRST_CONNECTION, LoadSceneMode.Additive);
                    break;
                default:
                    Debug.Log("Error when login - Disconnection");
                    OnDisconnected?.Invoke(this);
                    break;
            }
        }
    }

    /// <summary>
    /// Send Player information creation message
    /// </summary>
    /// <param name="pNickname"></param>
    /// <param name="pDeviceId"></param>
    public void SendPlayerCreationInformationMessage(string pNickname, string pDeviceId)
    {
        if (Client != null)
        {
            if (Client.ConnectionState == ConnectionState.Connected)
            {
                PlayerCreationInformationMessage playerInfos = new PlayerCreationInformationMessage
                {
                    nickname = pNickname,
                    deviceId = pDeviceId
                };

                using (Message playerInfosMessage = Message.Create(CommunicationTag.Connection.PLAYER_CREATION_INFORMATION_MESSAGE, playerInfos))
                {
                    Client.SendMessage(playerInfosMessage, SendMode.Reliable);
                }
            }
        }
    }

    #endregion
}
