using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;


public class UIManager : MonoBehaviourSingletonPersistent<UIManager>
{
    #region Properties
    /// <summary>
    /// The Stack Of UI Panels
    /// </summary>
    public Stack<UIPanelBase> uiPanels = new Stack<UIPanelBase>();
    #endregion

    #region Unity CallBacks

    #endregion

    #region Implementation

    private void AddPanelToStack(UIPanelBase pUIBasePanel)
    {
        uiPanels.Push(pUIBasePanel);
    }

    public void OnPanelClosed(UIPanelBase pPanel)
    {
        UIPanelBase panel = uiPanels.Pop();
        panel.OnClosed -= OnPanelClosed;
    }

    public void CreatePanel(string pPanelName, GameObject pGameObject)
    {
        GameObject panelGO = Instantiate((GameObject)AssetBundleManager.instance.GetAssetFromBundle(Constants.Bundles.UI, pPanelName), GameObject.Find(Constants.UI.UI_PLAYERMAP).transform);
        UIPanelBase panel = panelGO.GetComponent<UIPanelBase>();
        AddPanelToStack(panel);
        panel.OnClosed += OnPanelClosed;
        panel.Init(pGameObject);
    }

    internal void CloseAllPanels()
    {
        while (uiPanels.Count > 0)
        {
            UIPanelBase panel = uiPanels.Peek();
            panel.Close();
        }
    }

    #endregion
}

