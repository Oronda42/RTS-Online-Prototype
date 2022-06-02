using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS.Models;

[CreateAssetMenu(fileName = "ResourceProducerBuilding", menuName = "RTS/Buildings/Producer/01 - New resource producer building", order = 1)]
public class BuildingProducer : BuildingBase
{
    #region Properties


    /// <summary>
    /// Model of the building producer
    /// </summary>
    public new BuildingProducerModel Model
    {
        get { return GetModel() as BuildingProducerModel;  }
        set { SetModel(value); }
    }
    [SerializeField]
    private BuildingProducerModel modelProducer;

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building properties
    /// </summary>
    /// <returns></returns>
    protected override BuildingModel GetModel()
    {
        return modelProducer;
    }

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building property
    /// </summary>
    /// <returns></returns>
    protected override void SetModel(BuildingModel pBuilding)
    {
        modelProducer = pBuilding as BuildingProducerModel;
    }

    #endregion

}
