using DarkRift;
using RTS.Configuration;
using RTS.Server.Messages;
using RTS.Models;
using System;
using UnityEngine;
using UnityEngine.EventSystems;


#region Delegates

public delegate void PlayerBuildingEvents(PlayerBuildingBase pBuilding);

#endregion

public abstract class PlayerBuildingBase : MapElementBase, IPointerClickHandler
{
    #region Events

    //public event PlayerBuildingEvents OnBuildingDestroyed, OnConstructionFinished, OnStateChanged;

    #endregion

    #region Properties

    /// <summary>
    /// Model of the player building
    /// </summary>
    public PlayerBuildingModel Model
    {
        get { return GetModel(); }
        set { SetModel(value); }
    }

    /// <summary>
    /// Model of the player building
    /// </summary>
    [NonSerialized]
    private PlayerBuildingModel model;

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building properties
    /// </summary>
    /// <returns></returns>
    protected virtual PlayerBuildingModel GetModel()
    {
        return model;
    }

    /// <summary>
    /// Useful to use metamorphism and thus, get the child model when call Building properties
    /// </summary>
    /// <returns></returns>
    protected virtual void SetModel(PlayerBuildingModel pPlayerBuilding)
    {
        model = pPlayerBuilding;
    }
    #endregion

    #region Unity events

    public virtual void Start()
    {
        ClockManager.instance.onTickEvent += OnClockTick;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
          UIManager.instance.CreatePanel(Constants.UI.Panels.PLAYER_BUILDING_INFO_PANEL, gameObject);
           
    }

    #endregion

    #region Implementation

    /// <summary>
    /// To be overrident
    /// </summary>
    public virtual void Init(PlayerBuildingModel pPlayerBuildingModel = null)
    {


    }

    /// <summary>
    /// To be orriden
    /// </summary>
    public virtual void OnClockTick()
    { }

    /// <summary>
    /// Destroy the building
    /// </summary>
    public virtual void Destroy()
    {
        ClockManager.instance.onTickEvent -= OnClockTick;
        PlayerBuildingManager.instance.DestroyBuilding(this);
    }

    #endregion

    #region Common messages

    public virtual void SendActivationMessage()
    {
        //Send the messge to the server
        if (GameClientManager.instance.Client)
        {
            if (GameClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
            {
                //Serialize model
                PlayerBuildingMessage playerBuilding = new PlayerBuildingMessage {
                    playerId = PlayerManager.instance.Player.id,
                    token = PlayerManager.instance.Player.CurrentToken,
                    PlayerBuilding = Model };
                //Send message
                using (Message updateMessage = Message.Create(CommunicationTag.PlayerBuildings.ACTIVATE_PLAYER_BUILDING_REQUEST, playerBuilding))
                {
                    GameClientManager.instance.Client.SendMessage(updateMessage, SendMode.Reliable);
                }
            }
        }
    }

    public virtual void SendPauseMessage()
    {
        //Send the messge to the server
        if (WorldClientManager.instance.Client)
        {
            if (WorldClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
            {
                //Serialize model
                PlayerBuildingMessage playerBuilding = new PlayerBuildingMessage {
                    playerId = PlayerManager.instance.Player.id,
                    token = PlayerManager.instance.Player.CurrentToken,
                    PlayerBuilding = Model };
                //Send message
                using (Message updateMessage = Message.Create(CommunicationTag.PlayerBuildings.PAUSE_PLAYER_BUILDING_REQUEST, playerBuilding))
                {
                    WorldClientManager.instance.Client.SendMessage(updateMessage, SendMode.Reliable);
                }
            }
        }
    }

    /// <summary>
    /// Send an increment level for the player building specified in parameter
    /// </summary>
    /// <param name="pBuilding"></param>
    public void SendIncrementLevelMessage()
    {
        //Send the messge to the server
        if (WorldClientManager.instance.Client)
        {
            if (WorldClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
            {
                PlayerBuildingUpdatelLevelRequest updateLevelRequest = new PlayerBuildingUpdatelLevelRequest
                {
                    playerId = PlayerManager.instance.Player.id,
                    token = PlayerManager.instance.Player.CurrentToken,

                    buildingId = Model.Building.id,
                    buildingNumber = Model.buildingNumber,
                    buildingType = Model.Building.buildingType.id
                };

                //Send message
                using (Message incrementLevelMessage = Message.Create(CommunicationTag.PlayerBuildings.INCREMENT_LEVEL_PLAYER_BUILDING_REQUEST, updateLevelRequest))
                {
                    WorldClientManager.instance.Client.SendMessage(incrementLevelMessage, SendMode.Reliable);
                }
            }
        }
    }

    #endregion

    //#region Interactions & conditions

    ///// <summary>
    /////  Level up Interaction condition
    ///// </summary>
    ///// <param name="pParameters"></param>
    ///// <returns></returns>
    //protected virtual bool IncrementLevelCondition(params object[] pParameters)
    //{
    //    if (Model.State.id == (int)StateOfBuilding.CONSTRUCTION)
    //        return false;

    //    if (!Model.CanLevelUp())
    //        return false;

    //    if (!PlayerManager.instance.Player.resourceBag.HasEnoughResource(Model.Building.Levels[Model.Level.id].cost))
    //        return false;

    //    return true;
    //}

    ///// <summary>
    ///// Activation conditions
    ///// </summary>
    ///// <returns></returns>
    //protected virtual bool ActivateCondition(params object[] pParameters)
    //{
    //    return Model.CanActivate();
    //}

    ///// <summary>
    ///// Activation interaction
    ///// </summary>
    //protected virtual void ActivateInteraction(params object[] pParameters)
    //{
    //    if (ActivateCondition())
    //    {
    //        Model.Activate(ClockManager.instance.time);
    //        SendActivationMessage();
    //    }
    //}

    ///// <summary>
    ///// Level up Interaction action
    ///// </summary>
    ///// <param name="pParameters"></param>
    //protected virtual void IncrementLevelInteraction(params object[] pParameters)
    //{
    //    if (Model.CanLevelUp())
    //    {
    //        Debug.Log("The building N°" + Model.buildingNumber + " has been leveled up !");

    //        //Increment level and consume resources
    //        Model.IncrementLevel(ClockManager.instance.time);
    //        SendIncrementLevelMessage();
    //    }
    //    else
    //        Debug.Log("The building N°" + Model.buildingNumber + " cannot be leveled up");

    //}

    ///// <summary>
    ///// Destroy Interaction action
    ///// </summary>
    ///// <param name="pParameters"></param>
    //protected virtual void DestroyInteraction(params object[] pParameters)
    //{
    //    Destroy();
    //}

    //protected virtual bool PauseCondition(params object[] pParameters)
    //{
    //    return Model.CanPause();
    //}

    //protected virtual void PauseInteraction(params object[] pParameters)
    //{
    //    if (PauseCondition())
    //    {
    //        Model.Pause(ClockManager.instance.time);
    //        SendPauseMessage();
    //    }
    //}



    
}