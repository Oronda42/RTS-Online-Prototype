using RTS.Configuration;
using RTS.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

public class GameBuildingManager : MonoBehaviourSingletonPersistent<GameBuildingManager>
{
    #region Properties

    /// <summary>
    /// Buildings available in the manager
    /// </summary>
    [SerializeField]
    List<BuildingBase> buildings;


    /// <summary>
    /// States available in the manager
    /// </summary>
    [SerializeField]
    List<BuildingState> buildingStates;

    #endregion

    #region Implementation

    /// <summary>
    /// Initialize the list of resource
    /// </summary>
    public void Init()
    {
        buildings = new List<BuildingBase>();
        buildingStates = new List<BuildingState>();

        /////////////////////
        /// Add producer buildings
        Object[] buildingsAssets = AssetBundleManager.instance.GetAllAssetFromBundle(Constants.Bundles.BUILDING_PRODUCER);

        for (int i = 0; i < buildingsAssets.Count(); i++)
        {
            //Create The Building
            BuildingBase building = Instantiate((BuildingBase)buildingsAssets[i]);
            buildings.Add(building);
        }

        /////////////////////
        /// Add passive buildings
        buildingsAssets = AssetBundleManager.instance.GetAllAssetFromBundle(Constants.Bundles.BUILDING_PASSIVE);
        for (int i = 0; i < buildingsAssets.Count(); i++)
        {
            //Create The Building
            BuildingBase building = Instantiate((BuildingBase)buildingsAssets[i]);
            buildings.Add(building);
        }

        /////////////////////
        /// Add center buildings
        buildingsAssets = AssetBundleManager.instance.GetAllAssetFromBundle(Constants.Bundles.BUILDING_CENTER);
        for (int i = 0; i < buildingsAssets.Count(); i++)
        {
            //Create The Building
            BuildingBase building = Instantiate((BuildingBase)buildingsAssets[i]);
            buildings.Add(building);
        }

        /////////////////////
        /// Add producer buildings
        Object[] buildingsStatesAssets = AssetBundleManager.instance.GetAllAssetFromBundle(Constants.Bundles.BUILDING_STATE);

        for (int i = 0; i < buildingsStatesAssets.Count(); i++)
        {
            //Create The Building
            BuildingState buildingState = Instantiate((BuildingState)buildingsStatesAssets[i]);
            buildingStates.Add(buildingState);
        }

    }

    public List<BuildingBase> GetBuildingsOfType(TypeOfBuilding pType)
    {
        List<BuildingBase> buildingsBases = buildings.FindAll(t => t.Model.buildingType.id == (int)pType);
        return buildingsBases;
    }

    /// <summary>
    /// Returns the resource specified
    /// </summary>
    /// <param name="pId"></param>
    /// <returns></returns>
    public BuildingBase GetBuilding(int pId)
    {
        for (int i = 0; i < buildings.Count(); i++)
        {
            if (buildings[i].Model.id == pId)
            {
                return buildings[i];
            }
        }

        return null;
    }


    /// <summary>
    /// Returns the states specified
    /// </summary>
    /// <param name="pId"></param>
    /// <returns></returns>
    public BuildingState GetBuildingState(int pId)
    {
        for (int i = 0; i < buildingStates.Count(); i++)
        {
            if (buildingStates[i].model.id == pId)
            {
                return buildingStates[i];
            }
        }

        return null;
    }


    #endregion
}
