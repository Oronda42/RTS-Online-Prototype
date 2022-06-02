using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UILoginManager : MonoBehaviour
{
    #region References

    /// <summary>
    /// Login InputField
    /// </summary>
    public InputField loginInputField;

    /// <summary>
    /// Password InputField
    /// </summary>
    public InputField passwordInputField;

    /// <summary>
    /// Login Button
    /// </summary>
    public Button loginButton;

    #endregion

    #region Implementation

    public void OnLogingButtonPressed()
    {
        string _loginText = loginInputField.text;
        string _passwordText = passwordInputField.text;

        //ClientManager.instance.LoginWithPassword(_loginText, _passwordText);
    }

    #endregion

}
