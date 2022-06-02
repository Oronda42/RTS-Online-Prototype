using DarkRift;
using RTS.Configuration;
using RTS.Server.Messages;
using RTS.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#region delegate
public delegate void PlayerBuildingCenterDelegate(PlayerBuildingCenter pbuilding);
#endregion

public class PlayerBuildingCenter : PlayerBuildingBase,IPointerClickHandler
{

    #region Events
    public event PlayerBuildingCenterDelegate OnbuildingTick;
    #endregion

    #region Properties

    public new PlayerBuildingCenterModel Model
    {
        get { return GetModel() as PlayerBuildingCenterModel; }
        set { SetModel(value); }
    }
    /// <summary>
    /// (Don't use, Use accessor instead) Model of the player building passive
    /// </summary>
    [SerializeField]
    private PlayerBuildingCenterModel modelCenter;

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building property
    /// </summary>
    /// <returns></returns>
    protected override PlayerBuildingModel GetModel()
    {
        return modelCenter;
    }

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building properties
    /// </summary>
    /// <returns></returns>
    protected override void SetModel(PlayerBuildingModel pPlayerBuilding)
    {
        modelCenter = pPlayerBuilding as PlayerBuildingCenterModel;
    }


    /// <summary>
    /// Reference to the scriptable object
    /// </summary>
    [SerializeField]
    BuildingCenter building;

    #endregion

    #region Unity callbacks

    public override void Start()
    {
        base.Start();

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        UIManager.instance.CreatePanel(Constants.UI.Panels.COMMAND_CENTER, gameObject);
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
            Model = (PlayerBuildingCenterModel)pPlayerBuildingModel;
        }
        else // when there is no buildingModel, we take it from prefab
        {
            Model = new PlayerBuildingCenterModel(building.Model);
        }

        //Init the UI
        
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
