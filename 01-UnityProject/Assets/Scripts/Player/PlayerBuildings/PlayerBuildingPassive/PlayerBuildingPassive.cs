using DarkRift;
using RTS.Configuration;
using RTS.Server.Messages;
using RTS.Models;
using System.Collections.Generic;
using UnityEngine;

#region delegate
public delegate void PlayerBuildingPassiveDelegate(PlayerBuildingPassive pbuilding);
#endregion

public class PlayerBuildingPassive : PlayerBuildingBase
{

    #region Events
    public event PlayerBuildingPassiveDelegate OnbuildingTick;
    #endregion

    #region Properties

    public new PlayerBuildingPassiveModel Model
    {
        get { return GetModel() as PlayerBuildingPassiveModel; }
        set { SetModel(value); }
    }
    /// <summary>
    /// (Don't use, Use accessor instead) Model of the player building passive
    /// </summary>
    [SerializeField]
    private PlayerBuildingPassiveModel modelPassive;

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building property
    /// </summary>
    /// <returns></returns>
    protected override PlayerBuildingModel GetModel()
    {
        return modelPassive;
    }

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building properties
    /// </summary>
    /// <returns></returns>
    protected override void SetModel(PlayerBuildingModel pPlayerBuilding)
    {
        modelPassive = pPlayerBuilding as PlayerBuildingPassiveModel;
    }


    /// <summary>
    /// Reference to the scriptable object
    /// </summary>
    [SerializeField]
    BuildingPassive building;

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
            Model = (PlayerBuildingPassiveModel)pPlayerBuildingModel;
            //Model.Building = (BuildingPassiveModel)BuildingData.GetBuildingById(pPlayerBuildingModel.Building.id);
        }
        else // when there is no buildingModel, we take it from prefab
        {
            Model = new PlayerBuildingPassiveModel(building.Model);
            Model.Init(ClockManager.instance.time, PlayerManager.instance.Player);
        }

        //Init the UI
        GetComponent<PlayerBuildingPassiveUI>().Init(this);
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

    #region Messages

    #endregion

    


}
