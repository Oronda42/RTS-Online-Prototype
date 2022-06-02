using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public delegate void UIPanelBaseEventHandler(UIPanelBase pPanel);

public class UIPanelBase : MonoBehaviour
{

    #region Events

    public event UIPanelBaseEventHandler OnClosed;

    #endregion

    #region Properties

    protected GameObject gameObjectReference;

    #endregion

    
    #region Implementation

    public virtual void Init(GameObject pGameObject)
    {
        gameObjectReference = pGameObject;
    }

    public virtual void Close()
    {
        FireOnClosed();
        Destroy(gameObject);
    }

    public virtual void UpdatePanel()
    {

    }

    private void FireOnClosed()
    {
        OnClosed?.Invoke(this);
    }

    #endregion
}

