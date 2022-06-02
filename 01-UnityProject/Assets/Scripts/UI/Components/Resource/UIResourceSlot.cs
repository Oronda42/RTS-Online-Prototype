using UnityEngine;
using RTS.Configuration;
using RTS.Models;
using UnityEngine.UI;
using TMPro;

class UIResourceSlot: MonoBehaviour
{

    #region Static Properties

    public static GameObject resourceSlotPrefab = null;

    #endregion

    #region Properties

    public Image resourceIcon;

    public TextMeshProUGUI resourceAmount;

    #endregion


    public void Init(Sprite pResourceIcon,string pResourceAmount)
    {
        if (!pResourceIcon)
            resourceIcon.enabled = false;
        else
            resourceIcon.enabled = true;

        resourceIcon.sprite = pResourceIcon;
        resourceAmount.text = pResourceAmount;
    }

    #region Static

    public static GameObject CreateResourceSlot(Sprite pResourceIcon, string pResourceAmount, Transform pPosition)
    {
        if(resourceSlotPrefab == null)
            resourceSlotPrefab = (GameObject)AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.UI, Constants.UI.Resources.DEFAULT_RESOURCE_SLOT);

        GameObject resourceSlot = Instantiate(resourceSlotPrefab, pPosition);
        UIResourceSlot slot = resourceSlot.GetComponent<UIResourceSlot>();
        slot.Init(pResourceIcon, pResourceAmount);
        return resourceSlot;
    }

    #endregion
}
