using RTS.Models;
using System.Collections.Generic;
using UnityEngine;

public class MapExtent : MonoBehaviour
{

    #region Properties

    /// <summary>
    /// Model of the extent
    /// </summary>
    public MapExtentModel Model;

    /// <summary>
    /// Elements within the extent 
    /// </summary>
    public List<MapElementBase> Elements;

    #endregion

    #region Unity callbacks

    public void Start()
    {
        Init();
    }

    #endregion

    #region Implementation

    public void Init()
    {
        InitializeElements();
    }

    /// <summary>
    /// Retrieve information from the scene
    /// </summary>
    public void RetrieveExtentData()
    {
        Elements = new List<MapElementBase>();
        Model.Elements = new List<MapExtentElementModel>();

        for (int i = 0; i < transform.childCount; i++)
        {
            MapElementBase mapElement = transform.GetChild(i).GetComponent<MapElementBase>();
            if(mapElement != null)
            {
                mapElement.Init(this);
                mapElement.MapExtentElementModel.InstanceId = i + 1;
                AddElement(mapElement);
            }
        }
    }

    /// <summary>
    /// Add a new element to the extent
    /// </summary>
    /// <param name="pElement"></param>
    public void AddElement(MapElementBase pElement)
    {
        Elements.Add(pElement);
        Model.Elements.Add(pElement.MapExtentElementModel);
    }

    /// <summary>
    /// Initialize each elements in the extent
    /// </summary>
    public void InitializeElements()
    {
        for (int i = 0; i < Elements.Count ; i++)
        {
            Elements[i].Init(this);
        }
    }

    #endregion
}
