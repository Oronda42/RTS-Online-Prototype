using System.Collections;
using UnityEngine;
using Utilities;

public class PlayerMapSceneScheduler : MonoBehaviourSingletonNonPersistent<PlayerMapSceneScheduler>
{

    #region Properties

  
    public PlayerMapManager PlayerMapManager;
    public PlayerBuildingManager PlayerBuildingsManager;
    public PlayerMarket PlayerMarket;


    /// <summary>
    /// script for The UI
    /// </summary>
    UIPlayerManager UIPlayerManager { get; set; }

    #endregion

    #region Unity callbacks

    public override void Awake()
    {
        base.Awake();
        Init();
    }

    #endregion

    #region Implementation

    /// <summary>
    /// Use this component to initialize the scheduler
    /// </summary>
    private void Init()
    {        
        //////////////////////////
        /// Get References
        PlayerMapManager = GetComponent<PlayerMapManager>();
        PlayerBuildingsManager = GetComponent<PlayerBuildingManager>();
        PlayerMarket = GetComponent<PlayerMarket>();

        UIPlayerManager = FindObjectOfType<UIPlayerManager>();
        UIPlayerManager.Init(PlayerManager.instance);

        //////////////////////////
        /// Start schedulation
        StartCoroutine(Schedule());
    }

    /// <summary>
    /// Coroutine to schedule each manager
    /// </summary>
    /// <returns></returns>
    IEnumerator Schedule()
    {
        //Player market
        PlayerMarket.Init();
        while (!PlayerMarket.IsInitialized())
        {
            yield return new WaitForSeconds(0.1f);
        }

        //Map manager
        PlayerMapManager.Init(PlayerManager.instance);
        while (!PlayerMapManager.IsInitialized())
        {
            yield return new WaitForSeconds(0.1f);
        }

        //Map manager
        PlayerBuildingsManager.Init();
        while (!PlayerBuildingsManager.IsInitialized())
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    #endregion


}
