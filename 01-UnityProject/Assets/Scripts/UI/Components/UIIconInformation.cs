using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIconInformation : MonoBehaviour
{
    #region Properties

    public Image icon;
    public TextMeshProUGUI text;

    #endregion

    #region Implementation

    /// <summary>
    /// Update Data
    /// </summary>
    /// <param name="pSprite"></param>
    /// <param name="pText"></param>
    public void UpdateData(Sprite pSprite, string pText)
    {
        text.text = pText;
        icon.sprite = pSprite;
    }

    #endregion
}
