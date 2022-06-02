using DarkRift;
using RTS.Server.Messages;
using RTS.Models;
using System.Collections.Generic;
using UnityEngine;

#region delegate
public delegate void PlayerBuildingResourceDelegate(PlayerBuildingProducer pbuilding);
#endregion

public class PlayerBuildingProducer : PlayerBuildingBase
{
    #region Events
    public event PlayerBuildingResourceDelegate OnbuildingTick, OnBuildingProduce;
    #endregion

    #region Properties

    #region Building

    /// <summary>
    /// Model of the player building producer. 
    /// </summary>
    public new PlayerBuildingProducerModel Model
    {
        get { return GetModel() as PlayerBuildingProducerModel; }
        set { SetModel(value); }
    }
    [SerializeField]
    private PlayerBuildingProducerModel modelProducer;

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building property
    /// </summary>
    /// <returns></returns>
    protected override PlayerBuildingModel GetModel()
    {
        return modelProducer;
    }

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building properties
    /// </summary>
    /// <returns></returns>
    protected override void SetModel(PlayerBuildingModel pPlayerBuilding)
    {
        modelProducer = pPlayerBuilding as PlayerBuildingProducerModel;
    }
    #endregion

    /// <summary>
    /// Reference to the scriptable object
    /// </summary>
    [SerializeField]
    BuildingProducer building;

    #endregion

    #region Unity callbacks

    public override void Start()
    {
        base.Start();
    }

    #endregion

    #region Implementation

    /// <summary>
    /// Initialization of the building
    /// </summary>
    public override void Init(PlayerBuildingModel pPlayerBuildingModel = null)
    {
        if (pPlayerBuildingModel != null) // when we specify a buildingModel
        {
            Model = (PlayerBuildingProducerModel)pPlayerBuildingModel;
        }
        else // when there is no buildingModel, we take it from prefab
        {
            Model = new PlayerBuildingProducerModel(building.Model);
            Model.Init(ClockManager.instance.time, PlayerManager.instance.Player);
        }


        GetComponent<PlayerBuildingProducerUI>().Init(this);
    }

    /// <summary>
    /// Called on tick event
    /// </summary>
    public override void OnClockTick()
    {
        base.OnClockTick();

        //If there is an action to perform, the model will do it automatically and will update the player
        Model.Tick(ClockManager.instance.time);

        OnbuildingTick?.Invoke(this);
    }

    #endregion
}
