using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerNameFormManagerPanel : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// A ref to the login prefab in order to instantiate it at start
    /// </summary>
    public GameObject loginPrefab;

    /// <summary>
    /// A string for identify the Popup which display Panel
    /// </summary>
    public string PopupName = "PlayerNameFormPopup";

    /// <summary>
    /// A ref to the canvas where put the prefab in
    /// </summary>
    public Transform canvas;
    #endregion

    #region Implementation
    private void Start()
    {
        UIPopup popup = UIPopup.GetPopup(PopupName);

        if (popup == null)
            return;

        popup.Show();
    }
    #endregion
}
