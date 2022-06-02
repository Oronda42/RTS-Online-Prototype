using DarkRift;
using DarkRift.Server;
using MySql.Data.MySqlClient;
using RTS.Database;
using RTS.Models;
using RTS.Models.Server;
using RTS.Server.Messages;
using RTS.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.LoginServer
{
    internal class LoginPlayerCommunicationPlugin : PlayerCommunicationPluginBase, ISchedulablePlugin
    {

        #region Properties

        public static LoginPlayerCommunicationPlugin Instance { get; private set; }

        #endregion

        #region Plugin Implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public LoginPlayerCommunicationPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
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
                        case CommunicationTag.Connection.DEVICE_IDENTIFICATION_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.Connection.PLAYER_IDENTIFICATION_REQUEST, null,
                                new Action<ThreadBase, IClient, LoginDeviceRequestMessage>(LoginPlayerConnectionManager.TryIdentifyDevice),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<LoginDeviceRequestMessage>() });
                            break;

                        case CommunicationTag.Connection.PLAYER_CREATION_INFORMATION_MESSAGE:
                            eventToDispatch = new GameEvent(CommunicationTag.Connection.PLAYER_CREATION_INFORMATION_MESSAGE, null,
                                new Action<ThreadBase, IClient, PlayerCreationInformationMessage>(LoginPlayerConnectionManager.TryCreatePlayer),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<PlayerCreationInformationMessage>() });
                            break;

                        case CommunicationTag.Clock.HOUR_SYNC_REQUEST:
                            eventToDispatch = new GameEvent(CommunicationTag.Clock.HOUR_SYNC_REQUEST, null,
                                new Action<ThreadBase, IClient, CredentialMessage>(LoginPlayerClockManager.HandleHourSyncRequest),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<CredentialMessage>() });
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


        #endregion
    }
}
