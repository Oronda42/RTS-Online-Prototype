using DarkRift;
using DarkRift.Server;
using RTS.Server.Messages;
using System;
using RTS.Configuration;
using System.Globalization;

namespace RTS.Plugins
{
    class ClockManager : Plugin
    {
        #region Plugin implementation
        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public ClockManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        public void Init()
        {
            //Listen to message
            ClientManager.ClientConnected += OnClientConnected;
            Console.WriteLine("[RTS] INFO : ClockManager initialized successfully");
        }

        #endregion

        #region Connection 

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnClientMessageReceived;
        }

        #endregion

        #region Implementation
        public void HandleHourSyncRequest(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.Clock.HOUR_SYNC_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    HourSyncResponseMessage HourSyncData = new HourSyncResponseMessage
                    {
                        serverTime = DateTime.Now.ToString(LocaleSettings.DATETIME_FORMAT_KEYCODE, DateTimeFormatInfo.InvariantInfo)
                    };

                    using (Message m = Message.Create(CommunicationTag.Clock.HOUR_SYNC_RESPONSE, HourSyncData))
                    {
                        e.Client.SendMessage(m, SendMode.Reliable);
                        Console.WriteLine("[RTS] INFO : Hour Sent to client : " + e.Client.ID);
                    }
                }
            }
        }

        #endregion

        #region Message Handle

        private void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Tag)
            {
                case CommunicationTag.Clock.HOUR_SYNC_REQUEST:
                    HandleHourSyncRequest(e);
                    break;

                default:
                    break;
            }
        } 

        #endregion
    }
}

