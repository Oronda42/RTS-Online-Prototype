using DarkRift;
using DarkRift.Server;
using RTS.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.GameServer
{
    class GamePlayerCommunicationPlugin : PlayerCommunicationPluginBase
    {
        #region Properties

        public static GamePlayerCommunicationPlugin Instance { get; private set; }

        #endregion

        #region Plugin Implementation

        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public GamePlayerCommunicationPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }
        #endregion

        #region Implementation

        public override void Init(object[] args)
        {
            base.Init(args);

            Instance = this;
            isInitialized = true;
        }

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
                        #region PlayerLogin

                        case CommunicationTag.Connection.DEVICE_IDENTIFICATION_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.Connection.PLAYER_IDENTIFICATION_REQUEST, null,
                                new Action<ThreadBase, IClient, CredentialMessage>(GamePlayerConnectionManager.TryIdentifyDevice),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<CredentialMessage>() });
                            break;

                        case CommunicationTag.PlayerConsumableResources.ALL_RESOURCE_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.PlayerConsumableResources.ALL_RESOURCE_REQUEST, null,
                                new Action<ThreadBase, IClient, PlayerResourceRequestMessage>(GamePlayerResourceManager.SendResources),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<PlayerResourceRequestMessage>() });
                            break;

                        #endregion

                        #region PlayerBuildings

                        case CommunicationTag.PlayerBuildings.CREATE_PLAYER_BUILDING_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.PlayerBuildings.CREATE_PLAYER_BUILDING_REQUEST, null,
                                new Action<ThreadBase, IClient, PlayerBuildingCreationRequestMessage>(GamePlayerBuildingManager.CreatePlayerBuilding),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<PlayerBuildingCreationRequestMessage>() });
                            break;

                        case CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.PlayerBuildings.ALL_PLAYER_BUILDINGS_REQUEST, null,
                                new Action<ThreadBase, IClient, PlayerBuildingListRequestMessage>(GamePlayerBuildingManager.SendAllBuildings),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<PlayerBuildingListRequestMessage>() });
                            break;

                        case CommunicationTag.PlayerBuildings.DESTROY_PLAYER_BUILDING_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.PlayerBuildings.DESTROY_PLAYER_BUILDING_REQUEST, null,
                                new Action<ThreadBase, IClient, PlayerBuildingDestroyRequest>(GamePlayerBuildingManager.DestroyPlayerBuilding),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<PlayerBuildingDestroyRequest>() });
                            break;

                        case CommunicationTag.PlayerBuildings.INCREMENT_LEVEL_PLAYER_BUILDING_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.PlayerBuildings.INCREMENT_LEVEL_PLAYER_BUILDING_REQUEST, null,
                                new Action<ThreadBase, IClient, PlayerBuildingUpdatelLevelRequest>(GamePlayerBuildingManager.IncrementPlayerBuildingLevel),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<PlayerBuildingUpdatelLevelRequest>() });
                            break;

                        case CommunicationTag.PlayerBuildings.ACTIVATE_PLAYER_BUILDING_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.PlayerBuildings.ACTIVATE_PLAYER_BUILDING_REQUEST, null,
                                new Action<ThreadBase, IClient, PlayerBuildingMessage>(GamePlayerBuildingManager.ActivatePlayerBuilding),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<PlayerBuildingMessage>() });
                            break;

                        case CommunicationTag.PlayerBuildings.PAUSE_PLAYER_BUILDING_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.PlayerBuildings.PAUSE_PLAYER_BUILDING_REQUEST, null,
                                new Action<ThreadBase, IClient, PlayerBuildingMessage>(GamePlayerBuildingManager.PausePlayerBuilding),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<PlayerBuildingMessage>() });
                            break;

                        #endregion

                        #region PlayerMap

                        case CommunicationTag.PlayerMap.PLAYER_MAP_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.PlayerMap.PLAYER_MAP_REQUEST, null,
                                new Action<ThreadBase, IClient, CredentialMessage>(GamePlayerMapManager.SendPlayerMap),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<CredentialMessage>() });
                            break;
                        #endregion

                        #region PlayerMarket

                        case CommunicationTag.PlayerMarket.PLAYER_MARKET_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.PlayerMarket.PLAYER_MARKET_REQUEST, null,
                                new Action<ThreadBase, IClient, CredentialMessage>(GamePlayerMarketManager.HandlePlayerMarketRequest),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<CredentialMessage>() });
                            break;


                        case CommunicationTag.PlayerMarket.PLAYER_MARKET_TRADE_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.PlayerMarket.PLAYER_MARKET_TRADE_REQUEST, null,
                                new Action<ThreadBase, IClient, PlayerMarketTradeMessage>(GamePlayerMarketManager.CreateTradeRequest),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<PlayerMarketTradeMessage>() });
                            break;

                        #endregion

                        #region Player

                        case CommunicationTag.Player.ALL_PLAYERS_REQUEST:

                            eventToDispatch = new GameEvent(CommunicationTag.Player.ALL_PLAYERS_REQUEST, null,
                                new Action<ThreadBase, IClient, AllPlayerRequestMessage>(GamePlayerManager.SendAllPLayers),
                                new object[] { GameThread.Instance, e.Client, message.Deserialize<AllPlayerRequestMessage>() });
                            break;

                            #endregion
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

        /// <summary>
        /// Handle client disconnection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            //Get client information from the plugin
            ClientInformation client = GetClientByClientId(e.Client.ID);

            //Create the event
            GameEvent eventToDispatch = new GameEvent(0, null,
                                new Action<ThreadBase, IClient, ClientInformation>(GamePlayerConnectionManager.DisconnectPlayer),
                                new object[] { GameThread.Instance, e.Client, client });

            //Dispatch the event
            DispatcherThread.Instance.EnqueueEvent(eventToDispatch);

            base.OnClientDisconnected(sender, e);
        }

        #endregion
    }
}
