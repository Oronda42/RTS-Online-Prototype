using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS.Models;

[CreateAssetMenu(fileName = "MarketRatios", menuName = "RTS/Market/01 - New resource market ratios", order = 1)]
public class MarketRatios : ScriptableObject
{
    #region Properties

    /// <summary>
    /// Data of the building
    /// </summary>
    [SerializeField]
    public List<MarketResourceRatioModel> marketRatios;

    #endregion
}
