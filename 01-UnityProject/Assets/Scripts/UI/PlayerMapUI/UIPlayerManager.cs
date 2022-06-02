using RTS.Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayerManager : MonoBehaviour
{
    #region References
    /// <summary>
    /// PlayerManager Reference
    /// </summary>
    PlayerManager playerManager;

    /// <summary>
    /// Main Building resource Reference
    /// </summary>
    public TextMeshProUGUI resource1, resource2;
    public TextMeshProUGUI resource3, resource4;
    public TextMeshProUGUI resource5, resource6;
    public TextMeshProUGUI resource7, resource8;

    /// <summary>
    /// Display Player's Nickname
    /// </summary>
    public TextMeshProUGUI nickname;

    Animator animator;

    #endregion

    #region Unity Callbacks

    

    #endregion

    #region Implementation

    public void Init(PlayerManager pPlayerManager)
    {
        playerManager = pPlayerManager;
        nickname.text = playerManager.Player.nickname;
        SubscribeEvent();

        //Update resources panel with playerResourceBag
        for (int i = 0; i < playerManager.Player.resourceBag.resources.Count; i++)
        {
            OnResourceChanged(playerManager.Player.resourceBag.resources[i]);
        }
       
    }

    #endregion

    #region Events

    /// <summary>
    /// Execute when a resource in the playerManager Change
    /// </summary>
    /// <param name="number"></param>
    public void OnResourceChanged(ResourceBagSlotModel pSlot)
    {
        switch (pSlot.resourceId)
        {
            case 1:
                resource1.text = pSlot.amount.ToString();
                break;
            case 2:
                resource2.text = pSlot.amount.ToString();
                break;
            case 3:
                resource3.text = pSlot.amount.ToString();
                break;
            case 4:
                resource4.text = pSlot.amount.ToString();
                break;
            case 5:
                resource5.text = pSlot.amount.ToString();
                break;
            case 6:
                resource6.text = pSlot.amount.ToString();
                break;
            case 7:
                resource7.text = pSlot.used.ToString() + " / " + pSlot.amount.ToString();
                break;
            case 8:
                resource8.text = pSlot.used.ToString() + " / " + pSlot.amount.ToString();
                break;
        }

    }

    #endregion

    #region Subscribe Events

    private void SubscribeEvent()
    {
        playerManager.Player.resourceBag.OnResourceChanged += OnResourceChanged;
    }

    private void UnSubscribeEvent()
    {
        playerManager.Player.resourceBag.OnResourceChanged -= OnResourceChanged;
    }

    #endregion
}
