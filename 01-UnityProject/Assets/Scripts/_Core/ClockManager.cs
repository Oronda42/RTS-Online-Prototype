using DarkRift;
using DarkRift.Client;
using RTS.Server.Messages;
using System;
using System.Collections;
using UnityEngine;
using Utilities;
using RTS.Configuration;
using System.Globalization;

#region Delegates
public delegate void TickHandler();
#endregion

public class ClockManager : MonoBehaviourSingletonPersistent<ClockManager>
{

    #region Events
    public event TickHandler onTickEvent;
    #endregion

    #region Properties

    [SerializeField]
    public DateTime time;
        
    Coroutine TickCoroutine;

    #endregion

    #region Implementation

    /// <summary>
    /// Ask the Time on server
    /// </summary>
    private void SendHourRequest()
    { 
        if (LoginClientManager.instance.Client)
        {
            if (LoginClientManager.instance.Client.ConnectionState == ConnectionState.Connected)
            {
                CredentialMessage requestHour = new CredentialMessage
                {
                    SerializeCredentials = true,
                    playerId = PlayerManager.instance.Player.id,
                    token = PlayerManager.instance.Player.CurrentToken,
                };

                //Send message
                using (Message message = Message.Create(CommunicationTag.Clock.HOUR_SYNC_REQUEST, requestHour))
                {
                    LoginClientManager.instance.Client.SendMessage(message, SendMode.Reliable);
                }
            }
        }
    }

    /// <summary>
    /// Update the client dateTime From the server Time
    /// </summary>
    /// <param name="e"></param>
    private void HandleHourResponse(MessageReceivedEventArgs e)
    {
        using (DarkRiftReader reader = e.GetMessage().GetReader())
        {
            HourSyncResponseMessage syncData = e.GetMessage().Deserialize<HourSyncResponseMessage>();

            try
            {
                time = DateTime.ParseExact(syncData.serverTime, LocaleSettings.DATETIME_FORMAT, new CultureInfo(LocaleSettings.DATE_FORMAT_PROVIDER), DateTimeStyles.None);
            }
            catch(Exception ex)
            {
                throw new Exception("Error : DAYTIME NOT SYNC : " + e.GetMessage() +"/ "+ ex.ToString());
            }
        }
    }

    /// <summary>
    /// Tick every second
    /// </summary>
    /// <returns></returns>
    public IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            FireOnTickEvent();
            time = time.AddSeconds(1);
        }
    }

    /// <summary>
    /// Fire event fo tick listeners
    /// </summary>
    void FireOnTickEvent()
    {
        onTickEvent?.Invoke();
    }

    #endregion

    #region Message Handle

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        switch (e.Tag)
        {
            case CommunicationTag.Clock.HOUR_SYNC_RESPONSE:
                HandleHourResponse(e);
                break;
            default:
                break;
        }
    }

    #endregion

    #region Event subscription

    /// <summary>
    /// Called when client is connected
    /// </summary>
    /// <param name="pClient"></param>
    public void OnLoginClientConnected(LoginClientManager pClient)
    {
        if (LoginClientManager.instance.Client)
        {
            //Listen for message
            LoginClientManager.instance.Client.MessageReceived += OnMessageReceived;
            SendHourRequest();
            TickCoroutine = StartCoroutine(Tick());
        }
        else
        {
            throw new Exception("There is no client active");
        }
    }

    #endregion

}