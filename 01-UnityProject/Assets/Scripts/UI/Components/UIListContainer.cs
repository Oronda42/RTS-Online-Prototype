using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIListContainer : MonoBehaviour
{
    #region Properties

    public Transform container;
    public TextMeshProUGUI text;

    #endregion

    #region Implementation

    /// <summary>
    /// Update Data
    /// </summary>
    /// <param name="pSprite"></param>
    /// <param name="pText"></param>
    public void UpdateData(string pText)
    {
        text.text = pText;
    }

    /// <summary>
    /// Reset the container
    /// </summary>
    public void ResetContainer()
    {
        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i));
        }
    }

    /// <summary>
    /// Add a game object to the container
    /// </summary>
    /// <param name="pComponent"></param>
    public void addToContainer(GameObject pComponent)
    {
        pComponent.transform.SetParent(container.transform);
    }

    
    #endregion
}
