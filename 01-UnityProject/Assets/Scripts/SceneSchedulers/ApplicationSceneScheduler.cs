using RTS.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationSceneScheduler : MonoBehaviour
{
    #region Unity callbacks
    void Start()
    {
        Schedule();
    }

    #endregion

    #region Implementation

    private void Schedule()
    {
        
        //Load internal data
        AssetBundleManager.instance.Init();
        GameResourceManager.instance.Init();
        GameBuildingManager.instance.Init();

        ////Player manager
        PlayerManager.instance.Init();

        //Network
        LoginClientManager.instance.Init();

        //Subscribe to client events
        LoginClientManager.instance.OnConnected += ClockManager.instance.OnLoginClientConnected;
        LoginClientManager.instance.OnConnected += ApplicationManager.instance.OnLoginClientConnected;

        //Suscribe to GameClient events
        GameClientManager.instance.OnLoggedIn += ApplicationManager.instance.OnGameClientLoggedIn;

        //Try connect
        LoginClientManager.instance.Connect();

    }

    #endregion

}
