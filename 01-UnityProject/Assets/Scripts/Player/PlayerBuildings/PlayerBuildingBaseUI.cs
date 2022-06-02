using RTS.Configuration;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuildingBaseUI : MonoBehaviour
{
    /// <summary>
    /// The reference to the PopupPrefab which is use to create Indicators
    /// </summary>
    protected GameObject indicatorPrefab;

    /// <summary>
    /// The reference to the playerBuildingModel
    /// </summary>
    private PlayerBuildingModel playerBuildingModel;

    /// <summary>
    /// Indicator's array which contain some long-during indicators
    /// </summary>
    public List<UIIndicator> indicators;

    #region Implementation
    public virtual void Init(PlayerBuildingBase pBuilding)
    {
        playerBuildingModel = pBuilding.Model;
        indicators = new List<UIIndicator>();
        indicatorPrefab = (GameObject)AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.UI, Constants.UI.Components.INDICATOR_PREFAB);

        if (playerBuildingModel.State.id == BuildingStateData.OutOfEnergy.id)
        {
            OnOutOfEnergy(playerBuildingModel);
        }
    }

    private void OnOutOfEnergy(PlayerBuildingModel pPlayerBuilding)
    {
        GameObject indicatorGameobject = Instantiate(indicatorPrefab, transform);
        UIIndicator indicatorScript = indicatorGameobject.GetComponent<UIIndicator>();
        indicatorScript.Init();
        indicatorScript.Stay = true;
        indicatorScript.AnimateSprite(GameResourceManager.instance.GetResource(ResourceData.Energy.id).sprite, Constants.UI.Components.GO_TO_LOCATION_1_TRIGGER);
        indicatorScript.TypeOfIndicator = ResourceData.Energy.id;
        indicators.Add(indicatorScript);
    }

    public virtual void OnStateChanged(PlayerBuildingModel pPlayerBuilding)
    {
        for (int i = indicators.Count - 1; i >= 0; i--)
        {
            if (indicators[i] != null)
            {
                bool removeIndicator = false;
                if (pPlayerBuilding.State.id != (int)StateOfBuilding.OUT_OF_ENERGY)
                {
                    if (indicators[i].TypeOfIndicator == ResourceData.Energy.id)
                    {
                        indicators[i].Stay = false;
                        removeIndicator = true;
                    }
                }
                if (removeIndicator)
                    indicators[i] = null;
            }
        }
    }

    private void OnDisable()
    {
        UnSubscribeEvent();
    }
    #endregion

    #region Subscribe Events

    /// <summary>
    /// Suscribe to all necessary events
    /// </summary>
    protected virtual void SubscribeEvent()
    {
        playerBuildingModel.OnStateChanged += OnStateChanged;
        playerBuildingModel.OnOutOfEnergy += OnOutOfEnergy;
    }

    /// <summary>
    /// Suscribe to all necessary events
    /// </summary>
    protected virtual void UnSubscribeEvent()
    {
        playerBuildingModel.OnStateChanged -= OnStateChanged;
        playerBuildingModel.OnOutOfEnergy -= OnOutOfEnergy;
    }

    #endregion
}