using RTS.Models;
using UnityEngine.EventSystems;

public class MapElementBuildingSpot : MapElementBase, IPointerClickHandler
{

    #region Properties


    /// <summary>
    /// Building 
    /// </summary>
    public PlayerBuildingBase building;

    #endregion

    #region UnityCallBacks

    public void OnPointerClick(PointerEventData eventData)
    {
        if (building)
            return;

        UIManager.instance.CreatePanel(Constants.UI.Panels.CONSTRUCTION_MENU_PANEL, gameObject);
    }

    #endregion

    #region Implementation

    /// <summary>
    /// Initialize the game object
    /// </summary>
    public override void Init(MapExtent pMapExtent)
    {
        base.Init(pMapExtent);
    }

    public void SetBuilding(PlayerBuildingBase pbuilding)
    {
        building = pbuilding;
        pbuilding.Model.OnBuildingDestroyed += OnBuildingDestroy;

        Hide();
    }

    private void OnBuildingDestroy(PlayerBuildingModel pBuilding)
    {
        building = null;
        Display();
    }

    #endregion

}
