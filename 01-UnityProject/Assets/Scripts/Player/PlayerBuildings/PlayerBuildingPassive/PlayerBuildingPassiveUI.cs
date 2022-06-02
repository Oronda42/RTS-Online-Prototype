using RTS.Configuration;
using UnityEngine;
using UnityEngine.UI;
using RTS.Models;

public class PlayerBuildingPassiveUI : PlayerBuildingBaseUI
{
    #region Properties
    /// <summary>
    /// The Reference to the building
    /// </summary>
    PlayerBuildingPassive playerBuildingPassive;

    private Sprite SpriteResourceNeeded;

    #endregion

    #region Unity Callbacks

    #endregion

    #region Implementation
    /// <summary>
    /// Initialize the UI component
    /// </summary>
    /// <param name="pBuilding"></param>
    public override void Init(PlayerBuildingBase pBuilding)
    {
        base.Init(pBuilding);
        playerBuildingPassive = (PlayerBuildingPassive)pBuilding;
        SubscribeEvent();
    }

    private void OnBuildingPassiveCycleFinished(PlayerBuildingPassiveModel pPlayerBuilding)
    {
        GameObject indicatorGameobject = Instantiate(indicatorPrefab, transform);
        UIIndicator indicator = indicatorGameobject.GetComponent<UIIndicator>();
        indicator.Init();
        indicator.AnimateSprite(GameResourceManager.instance.GetResource(pPlayerBuilding.Building.Levels[0].consumptionBag.resources[0].resourceId).sprite, Constants.UI.Components.GO_UP_TRIGGER);
        indicator.SetText(playerBuildingPassive.Model.Building.Levels[0].consumptionBag.resources[0].amount * -1 + "", Color.red);
    }
    #endregion

    #region Subscribe Events

    /// <summary>
    /// Suscribe to all necessary events
    /// </summary>
    protected override void SubscribeEvent()
    {
        base.SubscribeEvent();
        playerBuildingPassive.Model.OnBuildingPassiveCycleFinished += OnBuildingPassiveCycleFinished;
    }

    /// <summary>
    /// Suscribe to all necessary events
    /// </summary>
    protected override void UnSubscribeEvent()
    {
        base.UnSubscribeEvent();
        playerBuildingPassive.Model.OnBuildingPassiveCycleFinished -= OnBuildingPassiveCycleFinished;
    }

    #endregion
}

