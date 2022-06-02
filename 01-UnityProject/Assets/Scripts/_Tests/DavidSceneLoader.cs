using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DavidSceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject _map = (GameObject)AssetBundleManager.instance.GetAssetFromBundle("map_extents", "MapExtent_01");
        Instantiate(_map, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
