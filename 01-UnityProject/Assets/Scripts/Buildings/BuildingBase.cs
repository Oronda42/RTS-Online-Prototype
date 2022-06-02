using RTS.Models;
using System;
using UnityEngine;

public class BuildingBase : ScriptableObject
{

    #region Properties

    #region Building 
    /// <summary>
    /// Model of the building
    /// </summary>
    [SerializeField]
    public BuildingModel Model
    {
        get { return GetModel(); }
        set { SetModel(value); }
    }
    private BuildingModel model;

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building property
    /// </summary>
    /// <returns></returns>
    protected virtual BuildingModel GetModel()
    {
        return model;
    }

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building property
    /// </summary>
    /// <returns></returns>
    protected virtual void SetModel(BuildingModel pBuilding)
    {
        model = pBuilding;
    } 
    #endregion

    /// <summary>
    /// Reference to the game object for the player
    /// </summary>
    public GameObject playerBuildingPrefab;

    /// <summary>
    /// Icon of the building
    /// </summary>
    public Sprite icon;

    #endregion

}