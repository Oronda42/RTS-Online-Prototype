using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RTS.Models;
using DarkRift;
using Doozy.Engine.UI;

public class UIPlayerNameFormPanel : UIPanelBase
{
    /// <summary>
    /// Input field use by player to write his own pseudo
    /// </summary>
    public TMP_InputField nickname;

    /// <summary>
    /// Button use by player to validate his pseudo's choice
    /// </summary>
    public Button buttonOK;

    /// <summary>
    /// Use for display error message from invalid pseudo
    /// </summary>
    public TextMeshProUGUI errorText;

    /// <summary>
    /// A ref to the script of ErrorMessage
    /// </summary>
    public ErrorMessageFirstConnection errorMessageFirstCo;

    private LoginManagerModel loginManagerModel;

    /// <summary>
    /// Use for hide when validation is ok
    /// </summary>
    public UIPopup UIPopup;

    #region Unity Callbacks
    void Start()
    {
        buttonOK.onClick.AddListener(OnClickOK);
        loginManagerModel = new LoginManagerModel();

        nickname.Select();
        nickname.ActivateInputField();
    }

    #endregion

    #region Implementation

    public void Init()
    {
        errorText.gameObject.SetActive(false);
    }

    private void OnClickOK()
    {
        errorText.gameObject.SetActive(true);
        errorMessageFirstCo.RestartAnim();

        switch (loginManagerModel.ReasonForInvalidPseudo(nickname.text))
        {
            case LoginManagerModel.NICKNAME_TOO_LONG:
                errorText.text = "Your Nickname is too long (max 20 characters)";
                break;
            case LoginManagerModel.NICKNAME_TOO_SHORT:
                errorText.text = "Your Nickname is too short (min 3 characters)";
                break;
            case LoginManagerModel.NICKNAME_OK:
                errorText.gameObject.SetActive(false);
                UIPopup.Hide();
                LoginClientManager.instance.SendPlayerCreationInformationMessage(nickname.text, PlayerManager.instance.Player.DeviceId);
                break;
        }
    }
    
    #endregion
}
