using DarkRift;
using RTS.Configuration;
using RTS.Server.Messages;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerMarketPanel : UIPanelBase
{
    #region Properties

    /// <summary>
    /// performing trade
    /// </summary>
    PlayerMarketTradeModel currentTrade;

    public TMP_Dropdown ResourceReceivedDropdown;
    public TMP_Dropdown ResourceGivenDropdown;
    public TextMeshProUGUI AmountResourceReceivedText;
    public TextMeshProUGUI AmountResourceGivenText;
    public TextMeshProUGUI ReceiveLimitText;

    public Slider ReceiveLimitSlider;
    public Image BackgroundImageValidation;
    public Button TradeButton;

    /// <summary>
    /// TRadable resources
    /// </summary>
    List<Resource> resources = new List<Resource>();

    #endregion

    #region Implémentation

    public override void Init(GameObject pGameobject)
    {
        base.Init(pGameobject);

        //Get all resources tradables 
        // TODO : add a property tradable in the ResourceModel (and database)
        resources = GameResourceManager.instance.GetAllResources().Where(r => r.model.type.id != (int)TypeOfResource.PERSISTENT).ToList();

        ResetTrade();

        //Fill dropdown and update the panel
        FillDropdowns();

        //This make update the panel
        OnResourceChanged();

        //Subscribe to events
        SubscribeEvents();
    }

    /// <summary>
    /// Update the entire panel
    /// </summary>
    public override void UpdatePanel()
    {
        AmountResourceReceivedText.text = currentTrade.AmountReceived.ToString();
        AmountResourceGivenText.text = currentTrade.AmountGiven.ToString();

        UpdateTradeLimitSlider();
        UpdateTradeButton();
    }

    /// <summary>
    /// Reset the current trade 
    /// </summary>
    public void ResetTrade()
    {
        currentTrade = new PlayerMarketTradeModel();
        currentTrade.playerId = PlayerManager.instance.Player.id;
        currentTrade.quantity = 0;
        currentTrade.amountReceivedForOneGiven = 0;

        currentTrade.resourceIdGiven = resources[ResourceGivenDropdown.value].model.id;
        currentTrade.resourceIdReceived = resources[ResourceReceivedDropdown.value].model.id;

        currentTrade.amountReceivedForOneGiven = MarketData.GetAmountRatio(
            currentTrade.resourceIdGiven,
            currentTrade.resourceIdReceived);
    }

    /// <summary>
    /// When the player change a resource from the panel
    /// </summary>
    public void OnResourceChanged()
    {
        ResetTrade();
        UpdatePanel();
    }

    /// <summary>
    /// When the player change the amount traded
    /// </summary>
    public void OnTradeQuantityIncreased()
    {
        if (currentTrade.amountReceivedForOneGiven == 0)
            return;

        currentTrade.quantity++;

        //If we have passed the limit
        if ((float)PlayerMarket.instance.Model.GetAmountReceived() + currentTrade.AmountReceived > PlayerMarket.instance.Model.Level.AmountReceivedLimit)
        {
            currentTrade.quantity--;
        }

        UpdatePanel();
    }

    /// <summary>
    /// When the player change the amount traded
    /// </summary>
    public void OnTradeQuantityDecreased()
    {
        if (currentTrade.quantity > 0)
            currentTrade.quantity--;

        UpdatePanel();
    }

    /// <summary>
    /// When the player confirms the trade
    /// </summary>
    public void OnTradeAccepted()
    {
        if (GameClientManager.instance.Client)
        {
            if (GameClientManager.instance.Client.ConnectionState == DarkRift.ConnectionState.Connected)
            {
                currentTrade.creation = DateTime.Now;

                //If the client can perform the trade, send a message to the client
                if (PlayerMarket.instance.Model.Trade(currentTrade))
                {
                    //Get credential
                    PlayerMarketTradeMessage tradeRequest = new PlayerMarketTradeMessage
                    {
                        playerId = PlayerManager.instance.Player.id,
                        token = PlayerManager.instance.Player.CurrentToken,
                        //Set the trade
                        PlayerTrade = currentTrade,
                        amountGivenControl = currentTrade.AmountGiven,
                        amountReceivedControl = currentTrade.AmountReceived,
                    };

                    //Send message
                    using (Message tradeMessage = Message.Create(CommunicationTag.PlayerMarket.PLAYER_MARKET_TRADE_REQUEST, tradeRequest))
                    {
                        GameClientManager.instance.Client.SendMessage(tradeMessage, SendMode.Reliable);
                    }

                }

                ResetTrade();
                UpdatePanel();
            }
        }
    }

    /// <summary>
    /// Fill drop downs data
    /// </summary>
    private void FillDropdowns()
    {
        ResourceReceivedDropdown.options = new List<TMP_Dropdown.OptionData>();
        ResourceGivenDropdown.options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < resources.Count; i++)
        {
            ResourceReceivedDropdown.options.Add(new TMP_Dropdown.OptionData(resources[i].name, resources[i].sprite));
            ResourceGivenDropdown.options.Add(new TMP_Dropdown.OptionData(resources[i].name, resources[i].sprite));
        }

        ResourceReceivedDropdown.value = 0;
        ResourceGivenDropdown.value = 1;

        OnResourceChanged();

    }

    /// <summary>
    /// Update the Recycle Limit Slider
    /// </summary>
    /// <param name="pCurrentTradeLimit"></param>
    /// <param name="pAmountToReceive"></param>
    public void UpdateTradeLimitSlider()
    {
        if (currentTrade.AmountReceived == 0)
            ReceiveLimitSlider.value = 1 - (((float)PlayerMarket.instance.Model.GetAmountReceived()) / (float)PlayerMarket.instance.Model.Level.AmountReceivedLimit);
        else
            ReceiveLimitSlider.value = 1 - (((float)PlayerMarket.instance.Model.GetAmountReceived() + currentTrade.AmountReceived) / (float)PlayerMarket.instance.Model.Level.AmountReceivedLimit);

        ReceiveLimitText.text = ((float)PlayerMarket.instance.Model.GetAmountReceived() + currentTrade.AmountReceived).ToString();
    }

    //Update de trade button
    public void UpdateTradeButton()
    {
        if (PlayerManager.instance.Player.resourceBag.GetAmountByResourceId(resources[ResourceGivenDropdown.value].model.id) > currentTrade.AmountReceived)
        {
            BackgroundImageValidation.color = Color.green;
            TradeButton.image.color = Color.green;
            TradeButton.interactable = true;
        }
        else
        {
            BackgroundImageValidation.color = Color.red;
            TradeButton.image.color = Color.red;
            TradeButton.interactable = false;
        }
    }

    public override void Close()
    {
        UnsbsrcibeEvents();
        base.Close();
    }

    #endregion

    #region Events

    public void SubscribeEvents()
    {
        PlayerManager.instance.Player.resourceBag.OnResourceChanged += ResourceBag_OnResourceChanged;
    }

    public void UnsbsrcibeEvents()
    {
        PlayerManager.instance.Player.resourceBag.OnResourceChanged -= ResourceBag_OnResourceChanged;
    }

    private void ResourceBag_OnResourceChanged(ResourceBagSlotModel pSlot)
    {
        UpdateTradeButton();
    }

    #endregion
}
