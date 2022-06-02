using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fax_testOnClic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    /*
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject popUp = (GameObject)Instantiate(AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.VFX, "TestFax"), GameObject.Find(Constants.UI.UI_PLAYERMAP).transform);

            VFXPopup nVFXPopup = popUp.GetComponent<VFXPopup>();
            nVFXPopup.AnimateResource(Random.Range(1, 3), Random.Range(-50, 50), Constants.VFX.Popups.GO_UP_TRIGGER);
            nVFXPopup.transform.position = Input.mousePosition;
        }
    }*/
}
