using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayerResourcePanel : MonoBehaviour, IPointerClickHandler
{

    public Animator animator;

    public bool isPanelOpen;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isPanelOpen)
        {
            OpenPanel();
        }
        else
        {
            ClosePanel();
        }
    }
   
    public void OpenPanel()
    {
        isPanelOpen = true;
        animator.SetTrigger("Open");
       
    }

    public void ClosePanel()
    {
        isPanelOpen = false;
        animator.SetTrigger("Close");
    }


}
