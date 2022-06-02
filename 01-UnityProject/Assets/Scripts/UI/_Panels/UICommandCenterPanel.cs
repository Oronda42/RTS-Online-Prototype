using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICommandCenterPanel : UIPanelBase
{
    public Button marketButton;

    public void Init()
    {

    }

    private void Start()
    {
        marketButton.onClick.AddListener(() => OpenMarket());
    }

    public void OpenMarket()
    {
        UIManager.instance.CreatePanel(Constants.UI.Panels.PLAYER_MARKET_PANEL, gameObject);
    }
   
}
