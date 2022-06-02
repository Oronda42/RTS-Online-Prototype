using RTS.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapElement", menuName = "RTS/Map/01 - New map element", order = 1)]
public class MapElementScriptable : ScriptableObject
{
    #region Properties

    /// <summary>
    /// Data of the map element
    /// </summary>
    public MapElementModel MapElementModel;

    #endregion
}
