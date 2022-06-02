using DarkRift;
using DarkRift.Server;
using MySql.Data.MySqlClient;
using RTS.Configuration;
using RTS.Database;
using RTS.Models;
using RTS.Models.Server;
using RTS.Server.Messages;
using System;

namespace RTS.Server.WorldServer
{
    internal sealed class WorldPlayerCommunicationPlugin : PlayerCommunicationPluginBase, ISchedulablePlugin
    {

        #region Properties

        /// <summary>
        /// Singleton
        /// </summary>
        public static WorldPlayerCommunicationPlugin Instance { get; private set; }

        #endregion

        #region Plugin Implementation

        public override bool ThreadSafe => true;

        public override Version Version => new Version(1, 0, 0);

        public WorldPlayerCommunicationPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        #endregion

        #region ISchedulablePlugin Implementation

        public override void Init(object[] args)
        {
            base.Init(args);

            Instance = this;
            isInitialized = true;
        }

        #endregion

        #region Implementation

        protected override void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                base.OnClientMessageReceived(sender, e);

                ThreadEvent eventToDispatch = null;
                using (Message message = e.GetMessage())
                {
                    switch (e.Tag)
                    {
                        case CommunicationTag.Connection.PLAYER_IDENTIFICATION_REQUEST:
                            eventToDispatch = new GameEvent(CommunicationTag.Connection.PLAYER_IDENTIFICATION_REQUEST, null,
                                new Action<IClient, CredentialMessage>(WorldPlayerConnexionManager.TryIdentifyPlayer),
                                new object[] { e.Client, message.Deserialize<CredentialMessage>() });
                            break;
                    }
                }

                if (eventToDispatch != null)
                    DispatcherThread.Instance.EnqueueEvent(eventToDispatch);
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }

        protected override void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            try
            {
                //Before removing client, we need to update DataBase
                PlayerSessionModel playerSession = new PlayerSessionModel()
                {
                    PlayerId = GetClientByClientId(e.Client.ID).PlayerId,
                    Token = GetClientByClientId(e.Client.ID).Token,
                    Start = GetClientByClientId(e.Client.ID).PlayerSession.Start,
                    End = DateTime.Now
                };

                //Delegate the action to the database thread
                DatabaseEvent dbEvent = new DatabaseEvent(1, null,
                    new Action<MySqlConnection, PlayerSessionModel>(PlayerSessionFactory.Update),
                    new object[] { DatabaseIOThread.Instance.DBConnection.Connection, playerSession });

                //Set the same event on error in order to retry the insertion
                dbEvent.ErrorEvent = dbEvent;

                DispatcherThread.Instance.EnqueueEvent(dbEvent);
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }

            base.OnClientDisconnected(sender, e);
        }


        #endregion
    }
}
