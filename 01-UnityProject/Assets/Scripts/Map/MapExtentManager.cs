using RTS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapExtentManager : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Model of the map extent manager
    /// </summary>
    public MapExtentManagerModel Model;

    /// <summary>
    /// List of all map extent game objects
    /// </summary>
    [NonSerialized]
    public List<MapExtent> Extents;

    #endregion

    #region Implementation

    /// <summary>
    /// Initialize the component
    /// </summary>
    public void Init()
    {
        Model = new MapExtentManagerModel();
        RetrieveAllMapExtents();
        DisableAllMapExtents();
    }

    /// <summary>
    /// Retrieve all map extent defined as children
    /// </summary>
    public void RetrieveAllMapExtents()
    {
        Extents = new List<MapExtent>();
        Model.ClearExtents();

        for (int i = 0; i < transform.childCount; i++)
        {
            MapExtent mapExtent = transform.GetChild(i).GetComponent<MapExtent>();
            if (mapExtent != null)
            {
                if (!Model.IsMapExtentIdExists(mapExtent.Model.id))
                {
                    Model.AddExtent(mapExtent.Model);
                    Extents.Add(mapExtent);
                }
            }
        }
    }

    public void DisableAllMapExtents()
    {
        for (int i = 0; i < Extents.Count; i++)
        {
            Extents[i].gameObject.SetActive(false);
        }
    }


    #endregion
}
