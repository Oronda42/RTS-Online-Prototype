using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void BuildingMenuSlotDelegate(UIConstructionBuildingSlot pSlot);

public class UIConstructionBuildingSlot : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// reference of the buildingBase Actualy in the slot
    /// </summary>
    public BuildingBase buildingBase;

    /// <summary>
    /// TextInfo
    /// </summary>
    public TextMeshProUGUI buildingName, buildingConstructionTime;

    /// <summary>
    /// IconInfo
    /// </summary>
    public Image buildingImage;

    #endregion

    #region Events
    public event BuildingMenuSlotDelegate onSlotCliked;
    #endregion

    #region Implementation

    public void Init(BuildingBase pBuildingBase)
    {
        buildingBase = pBuildingBase;
        SetBuildingData(pBuildingBase);
    }

    /// <summary>
    /// apply buildingBase data in the slot
    /// </summary>
    /// <param name="pBuildingBase"></param>
    private void SetBuildingData(BuildingBase pBuildingBase)
    {
        buildingName.text = pBuildingBase.Model.name;
        buildingConstructionTime.text = pBuildingBase.Model.constructionTime.ToString() + " s";
        buildingImage.sprite = pBuildingBase.icon;
    }


    /// <summary>
    /// Fire OnSlotCliked Event
    /// </summary>
    /// <param name="pSlot"></param>
    void FireOnSlotClicked(UIConstructionBuildingSlot pSlot)
    {
        onSlotCliked?.Invoke(pSlot);
    }

    /// <summary>
    /// Event
    /// </summary>
    public void OnSlotCliked()
    {
        FireOnSlotClicked(this);
    }


    #endregion
}


