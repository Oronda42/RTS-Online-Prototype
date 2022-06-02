using RTS.Configuration;
using RTS.Models;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIPlayerBuildingInfoPanel : UIPanelBase
{
    #region Properties

    /// <summary>
    /// Reference UI elements
    /// </summary>
    public TextMeshProUGUI levelNumberText, buildingStateText, LevelUpAmount;

    /// <summary>
    /// Reference UI buttons
    /// </summary>
    public Button stateActionButton, destroyButton, levelUpButton;

    /// <summary>
    /// Reference to the building
    /// </summary>
    [HideInInspector]
    public PlayerBuildingBase PlayerBuilding;

    /// <summary>
    /// Container of the UI of a building
    /// </summary>
    public GameObject UIBuildingInfoContainer;

    /// <summary>
    /// Level up button color
    /// </summary>
    public Image levelUpButtonState;

    /// <summary>
    /// Reference to the building info UI
    /// </summary>
    UIBuildingInfo buildingInfo;

    /// <summary>
    /// Indicates if we are showing the next level
    /// </summary>
    public bool isShowingNextLevel = false;

    #endregion

    #region Implementation

    public override void Init(GameObject pGameObject)
    {
        base.Init(pGameObject);

        //Init variables
        PlayerBuilding = pGameObject.GetComponent<PlayerBuildingBase>();

        switch (PlayerBuilding.Model.Building)
        {
            case BuildingProducerModel pbProducer:
                GameObject producerInfoPanel = Instantiate((GameObject)AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.UI, Constants.UI.Panels.BUILDING_PRODUCER_INFO), UIBuildingInfoContainer.transform);
                UIBuildingProducerInfo producerPanel = producerInfoPanel.GetComponent<UIBuildingProducerInfo>();
                GameBuildingManager.instance.GetBuilding(PlayerBuilding.Model.Building.id);
                producerPanel.Init((BuildingProducer)GameBuildingManager.instance.GetBuilding(PlayerBuilding.Model.Building.id), PlayerBuilding.Model.Level.id);
                buildingInfo = producerPanel;
                break;

            case BuildingPassiveModel pbPassive:
                GameObject passiveInfoPanel = Instantiate((GameObject)AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.UI, Constants.UI.Panels.BUILDING_PASSIVE_INFO), UIBuildingInfoContainer.transform);
                UIBuildingPassiveInfo passivePanel = passiveInfoPanel.GetComponent<UIBuildingPassiveInfo>();
                passivePanel.Init((BuildingPassive)GameBuildingManager.instance.GetBuilding(PlayerBuilding.Model.Building.id), PlayerBuilding.Model.Level.id);
                buildingInfo = passivePanel;
                break;
        }

        SubscribeEvents();

        //Set buttons functions
        destroyButton.onClick.AddListener(() => DestroyBuildingButton(pGameObject.GetComponent<PlayerBuildingBase>()));

        //Init panel data
        UpdatePanel();

        //Update timer manually
        UpdateNextActionTimer(PlayerBuilding);
    }

    /// <summary>
    /// Subscribe to events
    /// </summary>
    private void SubscribeEvents()
    {
        PlayerBuilding.Model.OnStateChanged += OnPlayerBuildingStateChanged;
        PlayerManager.instance.Player.resourceBag.OnResourceChanged += ResourceBag_OnResourceChanged;

        switch (PlayerBuilding.Model)
        {
            case PlayerBuildingProducerModel pbProducer:
                gameObjectReference.GetComponent<PlayerBuildingProducer>().OnbuildingTick += UpdateNextActionTimer;
                break;
            case PlayerBuildingPassiveModel pbPassive:
                gameObjectReference.GetComponent<PlayerBuildingPassive>().OnbuildingTick += UpdateNextActionTimer;
                break;
        }
    }

    /// <summary>
    /// TODO : mettre à jour le panneau depuis la classe de base virtual
    /// </summary>
    public override void UpdatePanel()
    {
        UpdateState();
        UpdateLevelUpButton();

        levelNumberText.text = PlayerBuilding.Model.Level.id.ToString();

        //UpdateData(gameObjectReference);
    }

    /// <summary>
    /// Update the text color with the state
    /// </summary>
    /// <param name="pbuilding"></param>
    private void UpdateState()
    {
        //Get state scriptable
        BuildingState state = GameBuildingManager.instance.GetBuildingState(PlayerBuilding.Model.State.id);

        //Update data
        buildingStateText.color = state.color;
        buildingStateText.text = state.model.name;

        TextMeshProUGUI buttonText = stateActionButton.GetComponentInChildren<TextMeshProUGUI>();
        stateActionButton.interactable = true;

        switch (PlayerBuilding.Model.State.id)
        {
            case (int)StateOfBuilding.ACTIVE:
                stateActionButton.onClick.AddListener(() => PauseBuilding());
                buttonText.text = "Pause";
                stateActionButton.interactable = true;
                break;

            case (int)StateOfBuilding.PAUSED:
                stateActionButton.onClick.AddListener(() => ActiveBuilding());
                buttonText.text = "Activate";
                stateActionButton.interactable = true;
                break;

            case (int)StateOfBuilding.OUT_OF_ENERGY:
                stateActionButton.onClick.AddListener(() => ActiveBuilding());
                buttonText.text = "Activate";
                stateActionButton.interactable = true;
                break;

            case (int)StateOfBuilding.OUT_OF_RAW_MATERIAL:
                stateActionButton.onClick.AddListener(() => ActiveBuilding());
                buttonText.text = "Activate";
                stateActionButton.interactable = true;
                break;

            case (int)StateOfBuilding.CONSTRUCTION:
                stateActionButton.onClick.RemoveAllListeners();
                buttonText.text = "";
                stateActionButton.interactable = false;
                break;
        }
    }

    /// <summary>
    /// if the player can build this building and has enough resources
    /// </summary>
    /// <param name="pBuilding"></param>
    public void UpdateLevelUpButton()
    {
        LevelUpAmount.text = PlayerBuilding.Model.Building.Levels[PlayerBuilding.Model.Level.id].cost.resources[0].amount.ToString();

        if (PlayerBuilding.Model.CanLevelUp())
        {
            levelUpButtonState.color = Color.green;
            levelUpButton.interactable = true;
        }
        else
        {
            levelUpButtonState.color = Color.red;
            levelUpButton.interactable = false;
        }
    }

    /// <summary>
    /// Update the level of the building
    /// </summary>
    private void LevelUpButtonAction()
    {
        if (!isShowingNextLevel)
        {
            switch (PlayerBuilding)
            {
                case PlayerBuildingProducer pbProducer:
                    ((UIBuildingProducerInfo)buildingInfo).levelToDisplay = PlayerBuilding.Model.Level.id;
                    ((UIBuildingProducerInfo)buildingInfo).UpdateInfo();
                    break;

                case PlayerBuildingPassive pbPassive:
                    ((UIBuildingPassiveInfo)buildingInfo).levelToDisplay = PlayerBuilding.Model.Level.id;
                    ((UIBuildingPassiveInfo)buildingInfo).UpdateInfo();
                    break;
            }

        }
        else{
            PlayerBuilding.Model.IncrementLevel(ClockManager.instance.time);
            PlayerBuilding.SendIncrementLevelMessage();
            UpdatePanel();
        }
    }

    /// <summary>
    /// Interraction for The DESTROY Button
    /// </summary>
    /// <param name="pbuilding"></param>
    public void DestroyBuildingButton(PlayerBuildingBase pbuilding)
    {
        PlayerBuildingManager.instance.DestroyBuilding(pbuilding);
        Close();
    }

    /// <summary>
    /// set the BuildingTimer
    /// </summary>
    /// <param name="pBuilding"></param>
    private void UpdateNextActionTimer(PlayerBuildingBase pBuilding)
    {
        int time = 0;
        switch (pBuilding.Model.State.id)
        {
            case (int)StateOfBuilding.ACTIVE:
                time = (int)(pBuilding.Model.GetTimeBeforeNextAction(ClockManager.instance.time).TotalSeconds);
                break;
            case (int)StateOfBuilding.CONSTRUCTION:
                time = (int)(pBuilding.Model.GetTimeBeforeNextAction(ClockManager.instance.time).TotalSeconds);
                break;
        }


        switch (PlayerBuilding)
        {
            case PlayerBuildingProducer pbProducer:
                ((UIBuildingProducerInfo)buildingInfo).consumptionTime.text = time.ToString();
                break;

            case PlayerBuildingPassive pbPassive:
                ((UIBuildingPassiveInfo)buildingInfo).consumptionTime.text = time.ToString();
                break;
        }
    }

    /// <summary>
    /// Close the panel
    /// </summary>
    public override void Close()
    {
        PlayerManager.instance.Player.resourceBag.OnResourceChanged -= ResourceBag_OnResourceChanged;
        PlayerBuilding.Model.OnStateChanged -= OnPlayerBuildingStateChanged;


        switch (PlayerBuilding)
        {
            case PlayerBuildingProducer pbProducer:
                pbProducer.OnbuildingTick -= UpdateNextActionTimer;
                break;

            case PlayerBuildingPassive pbPassive:
                pbPassive.OnbuildingTick -= UpdateNextActionTimer;

                break;
        }

        base.Close();
    }

    /// <summary>
    /// Pause The building
    /// </summary>
    public void PauseBuilding()
    {
        PlayerBuilding.SendPauseMessage();
        PlayerBuilding.Model.Pause(ClockManager.instance.time);
    }

    /// <summary>
    /// activate the building
    /// </summary>
    private void ActiveBuilding()
    {
        PlayerBuilding.SendActivationMessage();
        PlayerBuilding.Model.Activate(ClockManager.instance.time);
    }

    #endregion

    #region Suscribed events

    private void ResourceBag_OnResourceChanged(ResourceBagSlotModel pSlot)
    {
        UpdateLevelUpButton();
    }

    private void OnPlayerBuildingStateChanged(PlayerBuildingModel pPlayerBuilding)
    {
        UpdateState();
    }

    #endregion

}




