using RTS.Models;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapExtent : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Model of the player map extent
    /// </summary>
    public PlayerMapExtentModel Model;

    /// <summary>
    /// Unity representation of a map element
    /// </summary>
    private List<MapElementBase> Elements;


    #endregion

    #region Implementation

    /// <summary>
    /// Initialization of the map extents
    /// </summary>
    /// <param name="pModel"></param>
    public void Init(PlayerMapExtentModel pModel)
    {
        Model = pModel;
    }

    /// <summary>
    /// Add a new element into the model
    /// </summary>
    /// <param name="pMapElement"></param>
    public void AddMapElement(MapElementBase pMapElement)
    {
        Elements.Add(pMapElement);
        //Model.extent.Elements.Add(pMapElement.Model);
    }
    #endregion




}

