using DarkRift;
using DarkRift.Client.Unity;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class ApplicationManager : MonoBehaviourSingletonPersistent<ApplicationManager>
{
    #region Properties

    /// <summary>
    /// Object to destroy when client is logged in
    /// </summary>
    [SerializeField]
    public List<GameObject> objectsToDestroy;

    #endregion

    #region Event Suscribed

    /// <summary>
    /// When the player connects to the login server
    /// </summary>
    /// <param name="pClient"></param>
    public void OnLoginClientConnected(LoginClientManager pClient) {

        if (pClient.Client.ConnectionState == DarkRift.ConnectionState.Connected)
        {           
            //Try to login with the unique identifier
            PlayerManager.instance.Player.DeviceId = SystemInfo.deviceUniqueIdentifier;
            LoginClientManager.instance.LoginWithDeviceRequest(PlayerManager.instance.Player.DeviceId, "");
        }
    }

    /// <summary>
    /// When the player is logged on the login server
    /// </summary>
    /// <param name="pClient"></param>
    public void OnGameClientLoggedIn(GameClientManager pClient)
    {
        StartCoroutine(WaitingSceneLoading(Constants.Scenes.PlayerWorld.PLAYER_WORLD));
        GameClientManager.instance.Client.MessageReceived += PlayerManager.instance.OnGameMessageReceived;
        PlayerManager.instance.RequestPlayerResourcesFromServer();
    }

    public IEnumerator WaitingSceneLoading(string Pscene)
    {
        SceneManager.LoadScene(Constants.Scenes.Core.LOADING_SCENE, LoadSceneMode.Additive);
        SceneLoader sceneLoader = null;

        while (!sceneLoader)
        {
            sceneLoader = FindObjectOfType<SceneLoader>();
            yield return new WaitForSeconds(0.1f);
        }

        sceneLoader.Init(Pscene);
    }


   


    

    #endregion
}
