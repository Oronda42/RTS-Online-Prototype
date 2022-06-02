using System;
using System.Collections.Generic;
using RTS.Configuration;
using RTS.Models;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuildingProducerUI : PlayerBuildingBaseUI
{
    #region Properties
    /// <summary>
    /// The Reference to the building
    /// </summary>
    PlayerBuildingProducer playerBuildingProducer;

    /// <summary>
    /// The reference to the PlayerBuildingProducerModel
    /// </summary>
    PlayerBuildingProducerModel pBProducerModel;

    /// <summary>
    /// Use for call the OutOfRawMaterial once when pBProducerModel.Player.resourceBag != null
    /// </summary>
    private bool retrievePlayerResourceBag = false;

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

        playerBuildingProducer = (PlayerBuildingProducer)pBuilding;
        pBProducerModel = playerBuildingProducer.Model;

        if (pBProducerModel.State.id == BuildingStateData.OutOfRawMaterial.id)
        {
            OnOutOfRawMaterial(pBProducerModel);
        }

        SubscribeEvent();
    }

    private void OnOutOfRawMaterial(PlayerBuildingModel pPlayerBuilding)
    {
        if (pBProducerModel.Player.resourceBag != null)
        {
            retrievePlayerResourceBag = false;
            ResourceBagModel resourceBagModel = ResourceBagModel.GetMissingResourceBag(pBProducerModel.Player.resourceBag, pBProducerModel.Level.consumptionBag);
            if (resourceBagModel.resources.Count != 0)
            {
                for (int j = 0; j < resourceBagModel.resources.Count; j++)
                {
                    CreateOutOfRawMaterialIndicator(resourceBagModel.resources[j]);
                }
            }
        }
        else
        {
            retrievePlayerResourceBag = true;
        }
    }

    public override void OnStateChanged(PlayerBuildingModel pPlayerBuilding)
    {
        for (int i = 0; i < indicators.Count; i++)
        {
            bool removeIndicator = false;
            if (pPlayerBuilding.State.id != (int)StateOfBuilding.OUT_OF_RAW_MATERIAL)
            {
                for (int j = 0; j < ResourceData.GetAllResources().Count; j++)
                {
                    if (indicators[i].TypeOfIndicator == ResourceData.GetAllResources()[j].id 
                        && indicators[i].TypeOfIndicator != ResourceData.Energy.id)
                    {
                        indicators[i].Stay = false;
                        removeIndicator = true;
                    }
                    if (removeIndicator)
                        break;
                }
                if (removeIndicator)
                    break;
            }
            if (removeIndicator)
                indicators[i] = null;
        }
        indicators.Remove(null);

        //Necessary call for manage OutOfEnergy indicator
        base.OnStateChanged(pPlayerBuilding);
    }

    private void CreateOutOfRawMaterialIndicator(ResourceBagSlotModel resourceBagSlotModel)
    {
        GameObject indicatorGameobject = Instantiate(indicatorPrefab, transform);
        UIIndicator indicatorScript = indicatorGameobject.GetComponent<UIIndicator>();
        indicatorScript.Init();
        indicatorScript.Stay = true;
        string location = "";

        if (indicators.Count == 0)
            location = Constants.UI.Components.GO_TO_LOCATION_1_TRIGGER;
        else if (indicators.Count == 1)
            location = Constants.UI.Components.GO_TO_LOCATION_2_TRIGGER;
        else if (indicators.Count == 2)
            location = Constants.UI.Components.GO_TO_LOCATION_3_TRIGGER;

        indicatorScript.AnimateSprite(GameResourceManager.instance.GetResource(resourceBagSlotModel.resourceId).sprite, location);
        indicators.Add(indicatorScript);
        indicatorScript.TypeOfIndicator = resourceBagSlotModel.resourceId;
    }

    /// <summary>
    /// OnResourceChanged check if we need to update some Indicators
    /// </summary>
    /// <param name="pSlot"></param>
    private void OnResourceChanged(ResourceBagSlotModel pSlot)
    {
        if (pBProducerModel.State.id == (int)StateOfBuilding.OUT_OF_RAW_MATERIAL)
        {
            ResourceBagModel resourceBagModel = ResourceBagModel.GetMissingResourceBag(pBProducerModel.Player.resourceBag, pBProducerModel.Level.consumptionBag);

            //We check if some indicators have to be destroy
            for (int i = 0; i < indicators.Count; i++)
            {
                bool removeIndicator = false;
                if (resourceBagModel.resources.Count != 0)
                {
                    indicators[i].Stay = false;
                    removeIndicator = true;

                    for (int j = 0; j < resourceBagModel.resources.Count; j++)
                    {
                        if (indicators[i].TypeOfIndicator == resourceBagModel.resources[j].resourceId)
                        {
                            indicators[i].Stay = true;
                            removeIndicator = false;
                        }
                        if (!removeIndicator)
                            break;
                    }
                    if (removeIndicator)
                        indicators[i] = null;
                }
            }
            indicators.Remove(null);

            //Then, check if new indicator have to be create
            for (int i = 0; i < resourceBagModel.resources.Count; i++)
            {
                bool createIndicator = true;
                for (int j = 0; j < indicators.Count; j++)
                {
                    if (resourceBagModel.resources[i].resourceId == indicators[j].TypeOfIndicator)
                        createIndicator = false;
                }
                if (createIndicator)
                    CreateOutOfRawMaterialIndicator(resourceBagModel.resources[i]);
            }
        }
    }

    private void OnBuildingProductionCycleFinished(PlayerBuildingProducerModel pPlayerBuildingProducerModel)
    {
        GameObject indicatorGo = Instantiate(indicatorPrefab, transform);
        UIIndicator indicator = indicatorGo.GetComponent<UIIndicator>();
        indicator.Init();
        indicator.AnimateSprite(GameResourceManager.instance.GetResource(pPlayerBuildingProducerModel.currentResourceIdProduced).sprite, Constants.UI.Components.GO_UP_TRIGGER);
        indicator.SetText("+" + pPlayerBuildingProducerModel.Building.Levels[0].amountProduced, Color.green);
    }

    private void OnDisable()
    {
        UnSubscribeEvent();
    }
    #endregion

    #region Unity Callback

    private void Update()
    {
        if (retrievePlayerResourceBag)
        {
            OnOutOfRawMaterial(pBProducerModel);
            pBProducerModel.Player.resourceBag.OnResourceChanged += OnResourceChanged;
        }
    }

    #endregion

    #region Subscribe Events

    /// <summary>
    /// Suscribe to all necessary events
    /// </summary>
    protected override void SubscribeEvent()
    {
        base.SubscribeEvent();
        pBProducerModel.OnBuildingProductionCycleFinished += OnBuildingProductionCycleFinished;
        pBProducerModel.OnOutOfRawMaterial += OnOutOfRawMaterial;
        pBProducerModel.OnStateChanged += OnStateChanged;

        //ResourceBag not instantiate when PlayerBuildingProducerUI awake
        if (pBProducerModel.Player.resourceBag != null)
            pBProducerModel.Player.resourceBag.OnResourceChanged += OnResourceChanged;
    }

    /// <summary>
    /// Suscribe to all necessary events
    /// </summary>
    protected override void UnSubscribeEvent()
    {
        base.UnSubscribeEvent();
        pBProducerModel.OnBuildingProductionCycleFinished -= OnBuildingProductionCycleFinished;
        pBProducerModel.OnOutOfRawMaterial -= OnOutOfRawMaterial;
        pBProducerModel.OnStateChanged -= OnStateChanged;
        pBProducerModel.Player.resourceBag.OnResourceChanged -= OnResourceChanged;
    }

    #endregion
}

