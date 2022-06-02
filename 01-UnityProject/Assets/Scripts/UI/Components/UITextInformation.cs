using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITextInformation : MonoBehaviour
{
    #region Properties

    public Image icon;
    public TextMeshProUGUI text, unit;

    #endregion

    #region Implementation

    /// <summary>
    /// Update Data
    /// </summary>
    /// <param name="pSprite"></param>
    /// <param name="pText"></param>
    public void UpdateData(Sprite pSprite, string pText, string pUnit)
    {
        text.text = pText;
        unit.text = pUnit;
        icon.sprite = pSprite;
    }

    #endregion
}
