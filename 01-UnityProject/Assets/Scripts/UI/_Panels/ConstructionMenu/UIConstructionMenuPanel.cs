using RTS.Configuration;
using RTS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class UIConstructionMenuPanel : UIPanelBase
{
    #region Properties


    /// <summary>
    /// Bundle reference
    /// </summary>
    GameObject uiBuildingMenuSlot, uiBuildingProducerInfoPanel, uiBuildingPassiveInfoPanel;

    /// <summary>
    /// Container of the UI of a building
    /// </summary>
    public GameObject UIBuildingInfoContainer;

    /// <summary>
    /// List of All Slots in the Menu
    /// </summary>
    public List<UIConstructionBuildingSlot> slots;

    /// <summary>
    /// Buttons to Select type of Buildings to Select
    /// </summary>
    public Button productionButton, passivButton, militaryButton;

    /// <summary>
    /// current Slot in UiMenuBuildingInfo Panel
    /// </summary>
    UIConstructionBuildingSlot currentSlot;

    /// <summary>
    /// the current UibuildingInfoPanel
    /// </summary>
    UIBuildingInfo currentInfoPanel;

    /// <summary>
    /// The building Spot selected by the player
    /// </summary>
    [HideInInspector] public MapElementBuildingSpot buildingSpot;

    /// <summary>
    /// Button to bulid
    /// </summary>
   public Button buildButton;

    /// <summary>
    /// Color of the build button
    /// </summary>
   public Image buildButtonState;

    #endregion

    #region Implementation

    public override void Init(GameObject pGameObject)
    {
        buildingSpot = pGameObject.GetComponent<MapElementBuildingSpot>();
        GetAssetFromBundle();
        SetTypeSelectionButtons();
        OpenDefaultSelection();
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        PlayerManager.instance.Player.resourceBag.OnResourceChanged += ResourceBag_OnResourceChanged;
    }

    private void UnsubscribeEvents()
    {
        PlayerManager.instance.Player.resourceBag.OnResourceChanged -= ResourceBag_OnResourceChanged;
    }

    private void ResourceBag_OnResourceChanged(ResourceBagSlotModel pSlot)
    {
        UpdateBuildButton();
    }

    private void OpenDefaultSelection()
    {
        OnTypeChanged(TypeOfBuilding.BUILDING_PRODUCER);
        OnSlotClicked(slots[0]);
    }

    private void GetAssetFromBundle()
    {
        uiBuildingMenuSlot = (GameObject)AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.UI, Constants.UI.Panels.CONSTRUCTION_BUILDING_MENU_SLOT);
        uiBuildingProducerInfoPanel = (GameObject)AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.UI, Constants.UI.Panels.BUILDING_PRODUCER_INFO);
        uiBuildingPassiveInfoPanel = (GameObject)AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.UI, Constants.UI.Panels.BUILDING_PASSIVE_INFO);
    }

    /// <summary>
    /// Set The Building Type Selection
    /// </summary>
    private void SetTypeSelectionButtons()
    {
        productionButton.onClick.AddListener(() => OnTypeChanged(TypeOfBuilding.BUILDING_PRODUCER));
        passivButton.onClick.AddListener(() => OnTypeChanged(TypeOfBuilding.BUILDING_PASSIVE));
        // militaryButton.onClick.AddListener(() => OnTypeChanged(TypeOfBuilding.BUILDING_PASSIVE));
    }

    /// <summary>
    /// Create all slots for the Menu
    /// </summary>
    /// <param name="pBuildings"></param>
    public void CreateBuildingSlots(List<BuildingBase> pBuildings)
    {
        //Delete actual spots 
        if (slots.Count != 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].onSlotCliked -= OnSlotClicked;
                Destroy(slots[i].gameObject);
            }
        }

        //Create new spots
        slots = new List<UIConstructionBuildingSlot>();

        for (int i = 0; i < pBuildings.Count; i++)
        {
            GameObject slotGo = Instantiate(uiBuildingMenuSlot, transform.GetChild(1).GetChild(0).GetChild(0));
            UIConstructionBuildingSlot slot = slotGo.GetComponent<UIConstructionBuildingSlot>();
            slots.Add(slot);
            slot.Init(pBuildings[i]);
            slot.onSlotCliked += OnSlotClicked;
        }

    }

    /// <summary>
    /// OnslotClicked Event reaction
    /// </summary>
    /// <param name="pSlot"></param>
    public void OnSlotClicked(UIConstructionBuildingSlot pSlot)
    {
        currentSlot = pSlot;
        CreateBuildingInfo(pSlot);
        UpdateBuildButton();
    }

    /// <summary>
    ///  Retrive the Type of buildings
    /// </summary>
    /// <param name="pType"></param>
    public void OnTypeChanged(TypeOfBuilding pType)
    {
        CreateBuildingSlots(GameBuildingManager.instance.GetBuildingsOfType(pType));
    }

    /// <summary>
    /// Create a UiBuildingInfoPanel
    /// </summary>
    /// <param name="pSlot"></param>
    private void CreateBuildingInfo(UIConstructionBuildingSlot pSlot)
    {
        if (currentInfoPanel != null)
        {
            Destroy(currentInfoPanel.gameObject);
        }

        switch (pSlot.buildingBase.Model)
        {
            case BuildingProducerModel pbProducer:
                GameObject producerInfoPanel = Instantiate(uiBuildingProducerInfoPanel, UIBuildingInfoContainer.transform);
                UIBuildingProducerInfo producerPanel = producerInfoPanel.GetComponent<UIBuildingProducerInfo>();
                currentInfoPanel = producerPanel;
                producerPanel.Init((BuildingProducer)pSlot.buildingBase, 1);
                break;

            case BuildingPassiveModel pbPassive:
                GameObject passiveInfoPanel = Instantiate(uiBuildingPassiveInfoPanel, UIBuildingInfoContainer.transform);
                UIBuildingPassiveInfo passivePanel = passiveInfoPanel.GetComponent<UIBuildingPassiveInfo>();
                currentInfoPanel = passivePanel;
                passivePanel.Init((BuildingPassive)pSlot.buildingBase, 1);
                break;
        }
    }
    
    /// <summary>
    /// if the player has enough resource ?
    /// </summary>
    public void UpdateBuildButton()
    {
        buildButton.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = currentSlot.buildingBase.Model.cost.resources[0].amount.ToString();
        if (PlayerManager.instance.Player.resourceBag.HasEnoughResource(currentSlot.buildingBase.Model.cost))
        {
            buildButtonState.color = Color.green;
            buildButton.interactable = true;
        }
        else
        {
            buildButtonState.color = Color.red;
            buildButton.interactable = false;
        }

    }

    public void BuildAction()
    {
        if (PlayerManager.instance.Player.resourceBag.HasEnoughResource(currentSlot.buildingBase.Model.cost))
        {
            PlayerBuildingManager.instance.CreatePlayerBuilding(currentSlot.buildingBase.Model.id, buildingSpot);
            UIManager.instance.CloseAllPanels();
        }
    }

    public override void Close()
    {
        base.Close();
        UnsubscribeEvents();
    }

    #endregion

}

