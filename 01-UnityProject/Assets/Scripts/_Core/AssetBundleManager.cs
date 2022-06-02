using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

/// <summary>
/// Class wich manage all asset bundles
/// </summary>
public class AssetBundleManager : MonoBehaviourSingletonPersistent<AssetBundleManager>
{

    #region Properties
    /// <summary>
    /// List of asset bundle loaded
    /// </summary>
    List<AssetBundle> loadedAssetBundle;
    #endregion

    #region Unity Callbacks
    public override void Awake()
    {
        base.Awake();
    }
    #endregion

    #region Implementation

    public void Init()
    {
        LoadAllAssetBundles();
    }

    private void LoadAllAssetBundles()
    {
        loadedAssetBundle = new List<AssetBundle>();

        LoadAssetBundle(Constants.Bundles.BUILDING_PRODUCER);
        LoadAssetBundle(Constants.Bundles.BUILDING_PASSIVE);
        LoadAssetBundle(Constants.Bundles.BUILDING_CENTER);
        LoadAssetBundle(Constants.Bundles.BUILDING_STATE);
        LoadAssetBundle(Constants.Bundles.RESOURCES);
        LoadAssetBundle(Constants.Bundles.UI);
        LoadAssetBundle(Constants.Bundles.CITIES);

    }

    /// <summary>
    /// Loads an asset bundle
    /// </summary>
    /// <param name="bundle">name of the bundle</param>
    public void LoadAssetBundle(string pBundle)
    {
        AssetBundle _assetBundleNeeded = loadedAssetBundle.FirstOrDefault(l => l.name == pBundle.ToLower());

        if (_assetBundleNeeded == null)
        {
            var _loadedAssetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundles/" + pBundle);

            if (_loadedAssetBundle)
                loadedAssetBundle.Add(_loadedAssetBundle);
        }
    }

    /// <summary>^^
    /// Get a specific asset within a bundle
    /// </summary>
    /// <param name="pBundleName"></param>
    /// <param name="pAssetName"></param>
    /// <returns></returns>
    public Object GetAssetFromBundle(string pBundleName, string pAssetName)
    {
        var bundleNeeded = loadedAssetBundle.FirstOrDefault(l => l.name == pBundleName.ToLower());

        if (bundleNeeded)
            return bundleNeeded.LoadAsset(pAssetName);
        else
            return null;
    }

    /// <summary>
    /// Returns all asset wihtin a bundle
    /// </summary>
    /// <param name="pBundleName"></param>
    /// <returns></returns>
    public Object[] GetAllAssetFromBundle(string pBundleName)
    {
        var bundleNeeded = loadedAssetBundle.FirstOrDefault(l => l.name == pBundleName);

        if (bundleNeeded)
            return bundleNeeded.LoadAllAssets();
        else
            return null;

    }
    #endregion
}
