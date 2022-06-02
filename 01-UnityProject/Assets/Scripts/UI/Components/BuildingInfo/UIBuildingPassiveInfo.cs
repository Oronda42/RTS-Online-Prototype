using UnityEngine;
using RTS.Configuration;
using RTS.Models;
using UnityEngine.UI;
using TMPro;

class UIBuildingPassiveInfo : UIBuildingInfo
{
    #region Properties

    /// <summary>
    /// References to slots
    /// </summary>
    public UIListContainer givenResource, consumedResources;
    public TextMeshProUGUI consumptionTime;

    /// <summary>
    /// Reference to the building
    /// </summary>
    BuildingPassive building;

    /// <summary>
    /// Level to display
    /// </summary>
    public int levelToDisplay;

    #endregion

    #region Implementation

    public void Init(BuildingPassive pBuilding, int pLevelToDisplay)
    {
        building = pBuilding;
        levelToDisplay = pLevelToDisplay;
        UpdateInfo();
    }

    public void UpdateConsumptionTime(int pTime)
    {
        consumptionTime.text = pTime.ToString() + " s";
    }

    public void UpdateInfo()
    {
        buildingSprite.sprite = building.icon;
        buildingName.text = building.Model.name;

        if (building.Model.Levels[levelToDisplay - 1].persistentBagNeeded.resources.Count > 0)
            powerNeeded.text = "x " + building.Model.Levels[levelToDisplay - 1].persistentBagNeeded.resources[0].amount.ToString();
        else
            powerNeeded.text = "x 0";

        UpdateConsumptionTime(building.Model.Levels[levelToDisplay - 1].secondsToConsume);


        // Delete all child
        consumedResources.ResetContainer();
        //// given resource
        for (int i = 0; i < building.Model.Levels[levelToDisplay - 1].consumptionBag.resources.Count; i++)
        {
            consumedResources.addToContainer(
                UIComponentLoader.CreateIconInformation(
                    GameResourceManager.instance.GetResource(building.Model.Levels[levelToDisplay - 1].consumptionBag.resources[i].resourceId).sprite,
                    "x " + building.Model.Levels[levelToDisplay - 1].consumptionBag.resources[i].amount.ToString(),
                    consumedResources.container.transform)
                );
        }

        // Delete all child
        givenResource.ResetContainer();
        //// producedResource
        for (int i = 0; i < building.Model.Levels[levelToDisplay - 1].persistentBag.resources.Count; i++)
        {
            givenResource.addToContainer(
                UIComponentLoader.CreateIconInformation(
                    GameResourceManager.instance.GetResource(building.Model.Levels[levelToDisplay - 1].persistentBag.resources[i].resourceId).sprite,
                    "x " + building.Model.Levels[levelToDisplay - 1].persistentBag.resources[i].amount.ToString(),
                    consumedResources.container.transform)
                );

        }

    }


    #endregion

}