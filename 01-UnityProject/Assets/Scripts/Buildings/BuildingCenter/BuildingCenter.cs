using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS.Models;

[CreateAssetMenu(fileName = "ResourceCenterBuilding", menuName = "RTS/Buildings/Center/01 - New resource center building", order = 1)]
public class BuildingCenter : BuildingBase
{
    #region Properties


    /// <summary>
    /// Model of the building producer
    /// </summary>
    public new BuildingCenterModel Model
    {
        get { return GetModel() as BuildingCenterModel; }
        set { SetModel(value); }
    }
    [SerializeField]
    private BuildingCenterModel modelCenter;

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building property
    /// </summary>
    /// <returns></returns>
    protected override BuildingModel GetModel()
    {
        return modelCenter;
    }

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building property
    /// </summary>
    /// <returns></returns>
    protected override void SetModel(BuildingModel pBuilding)
    {
        modelCenter = pBuilding as BuildingCenterModel;
    }

    #endregion

}
