using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS.Models;

[CreateAssetMenu(fileName = "ResourcePassiveBuilding", menuName = "RTS/Buildings/Passive/01 - New resource passive building", order = 1)]
public class BuildingPassive : BuildingBase
{
    #region Properties


    /// <summary>
    /// Model of the building producer
    /// </summary>
    public new BuildingPassiveModel Model
    {
        get { return GetModel() as BuildingPassiveModel; }
        set { SetModel(value); }
    }
    [SerializeField]
    private BuildingPassiveModel modelPassive;

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building property
    /// </summary>
    /// <returns></returns>
    protected override BuildingModel GetModel()
    {
        return modelPassive;
    }

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building property
    /// </summary>
    /// <returns></returns>
    protected override void SetModel(BuildingModel pBuilding)
    {
        modelPassive = pBuilding as BuildingPassiveModel;
    }

    #endregion

}
