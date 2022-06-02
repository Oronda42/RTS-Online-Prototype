using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

public class GameResourceManager : MonoBehaviourSingletonPersistent<GameResourceManager>
{
    #region Properties

    /// <summary>
    /// Resources available in the manager
    /// </summary>
    [SerializeField]
    List<Resource> resources;

    #endregion

    #region Implementation

    /// <summary>
    /// Initialize the list of resource
    /// </summary>
    public void Init()
    {
        Object[] resourcesAssets = AssetBundleManager.instance.GetAllAssetFromBundle(Constants.Bundles.RESOURCES);
        resources = new List<Resource>();

        for (int i = 0; i < resourcesAssets.Count(); i++)
        {
            //Create The Building
            Resource resourceScriptableObject = Instantiate((Resource)resourcesAssets[i]);
            resources.Add(resourceScriptableObject);
        }

    }

    /// <summary>
    /// Returns the resource specified
    /// </summary>
    /// <param name="pId"></param>
    /// <returns></returns>
    public Resource GetResource(int pId)
    {
        return resources.Where(r => r.model.id == pId).FirstOrDefault();
    }

    /// <summary>
    /// Get all resources
    /// </summary>
    /// <returns></returns>
    public List<Resource> GetAllResources()
    {
        return resources;
    }



    #endregion
}
