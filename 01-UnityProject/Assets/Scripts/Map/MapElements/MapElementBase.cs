using RTS.Models;
using System.Collections.Generic;
using UnityEngine;

public class MapElementBase : MonoBehaviour, IMapInteractable
{

    #region Properties

    /// <summary>
    /// Data of the map element
    /// </summary>
    [SerializeField]
    public MapElementScriptable MapElementScriptable;

    /// <summary>
    /// Data of the map element
    /// </summary>
    public MapExtentElementModel MapExtentElementModel;

    /// <summary>
    /// Sprite of the building spot
    /// </summary>
    [HideInInspector]
    public SpriteRenderer SpriteRenderer;

    /// <summary>
    /// Available actions when clicked
    /// </summary>
    [HideInInspector]
    public List<InteractionAction> Interactions;

    #endregion

    #region Unity callbacks

    void Start()
    {
    }

    #endregion

    #region Implementation

    public virtual void Init(MapExtent pMapExtent)
    {
        tag = Constants.Tags.Map.MAP_ELEMENT;

        MapExtentElementModel.Element = MapElementScriptable.MapElementModel;
        MapExtentElementModel.Extent = pMapExtent.Model;
        MapExtentElementModel.Position = StringToVector.Vec3ToStr(transform.position);

        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Hide()
    {
        SpriteRenderer.enabled = false;
    }

    protected virtual void Display()
    {
        SpriteRenderer.enabled = true;
    }

    #endregion

    #region IMapInteractable implementation
    public virtual string GetDescription()
    {
        return "";
    }

    public virtual string GetName()
    {
        return "";
    }

    /// <summary>
    /// Returns the position of the element in the scene 
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 GetMapPosition()
    {
        return transform.position;
    }

    public virtual List<InteractionAction> GetInteractionsAction()
    {
        return Interactions;
    }
    #endregion
}
