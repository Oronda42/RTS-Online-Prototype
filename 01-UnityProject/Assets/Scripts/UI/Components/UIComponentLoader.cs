using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIComponentLoader
{

    #region Implementation


    public static GameObject CreateIconInformation(Sprite pSprite, string pText, Transform pTransform)
    {
        GameObject prefab = (GameObject)AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.UI, Constants.UI.Components.ICON_INFORMATION);
        GameObject prefabInstance = GameObject.Instantiate(prefab, pTransform);
        prefabInstance.GetComponent<UIIconInformation>().UpdateData(pSprite, pText);

        return prefabInstance;
    }

    public static GameObject CreateTextInformation(Sprite pSprite, string pText, string pUnit, Transform pTransform)
    {
        GameObject prefab = (GameObject)AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.UI, Constants.UI.Components.TEXT_INFORMATION);
        GameObject prefabInstance = GameObject.Instantiate(prefab, pTransform);
        prefabInstance.GetComponent<UITextInformation>().UpdateData(pSprite, pText, pUnit);

        return prefabInstance;
    }

    public static GameObject CreateListContainer(string pText, Transform pTransform)
    {
        GameObject prefab = (GameObject)AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.UI, Constants.UI.Components.LIST_CONTAINER);
        GameObject prefabInstance = GameObject.Instantiate(prefab, pTransform);

        if (pText != null)
            prefabInstance.GetComponent<UIListContainer>().UpdateData(pText);

        return prefabInstance;
    }
    #endregion
}
