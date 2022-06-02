using DarkRift;
using DarkRift.Server;
using RTS.Server.Messages;
using RTS.Database;
using RTS.Models;
using System;
using System.Linq;

namespace RTS.Plugins
{
    public class PlayerMapManager: Plugin
    {
        #region Plugin implementation
        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public PlayerMapManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        public void Init()
        {
            //Listen to message
            ClientManager.ClientConnected += OnClientConnected;
        }

        #endregion

        #region Connection 

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnClientMessageReceived;
        }

        #endregion

        #region Message handle

        private void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Tag)
            {
                case CommunicationTag.PlayerMap.PLAYER_MAP_REQUEST:
                    SendPlayerMap(e);
                    break;
                //case CommunicationTag.PlayerMap.CREATE_MAP_EXTENT_REQUEST:
                //    CreateMapExtent(e);
                    //break;
                default:
                    break;
            }
        }
      
        #endregion

        #region Implementation

        /// <summary>
        /// Send The PlayerMap to Client
        /// </summary>
        /// <param name="e"></param>
        private void SendPlayerMap(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.PlayerMap.PLAYER_MAP_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    //Get credential
                    CredentialMessage credential = e.GetMessage().Deserialize<CredentialMessage>();

                    //If player is authorized
                    if (ConnectionManager.instance.CheckClientToken(credential.playerId, credential.token))
                    {
                        Client client = ConnectionManager.instance.GetClientByPlayerId(credential.playerId);
                        if (client == null)
                            throw new Exception("[RTS Info] : There is no client for the player " + client.Simulation.Player.id);

                        
                        //Fill extent data & get player data
                        foreach(PlayerMapExtentModel playerExtent in client.Simulation.PlayerMap.Extents)
                        {
                            playerExtent.Extent = PlayerMapService.MapExtents.Where(me => me.id == playerExtent.Extent.id).FirstOrDefault();
                            playerExtent.PlayerElements = PlayerMapFactory.GetPlayerElements(client.Simulation.SQLConnexion, playerExtent);
                        }

                        PlayerMapResponseMessage mapMessageData = new PlayerMapResponseMessage();
                        mapMessageData.SerializeCredentials = false;

                        mapMessageData.SetData(client.Simulation.PlayerMap);

                        //Create and send message
                        using (Message mapMessage = Message.Create(CommunicationTag.PlayerMap.PLAYER_MAP_RESPONSE, mapMessageData))
                        {
                            e.Client.SendMessage(mapMessage, SendMode.Reliable);
                            Console.WriteLine("[RTS] INFO : Map sent to player " + credential.playerId );
                        }

                    }
                }
            }            
        }


        /// <summary>
        /// Create a new map extent on DB
        /// </summary>
        /// <param name="e"></param>
        //private void CreateMapExtent(MessageReceivedEventArgs e)
        //{
        //    if (e.Tag == CommunicationTag.PlayerMap.CREATE_MAP_EXTENT_REQUEST)
        //    {
        //        using (DarkRiftReader reader = e.GetMessage().GetReader())
        //        {
        //            //Get credential
        //            CreateMapExtentRequest request = e.GetMessage().Deserialize<CreateMapExtentRequest>();

        //            //If player is authorized
        //            if (ConnectionManager.instance.CheckClientToken(request.playerId, request.token))
        //            {
        //                //TODO , CHECK IF THE MAP_EXTENT_POSITION IS EMPTY

        //                if (PlayerMapFactory.IsMapExtentSpotFree(request.playerId, request.positionOnMap))
        //                {
        //                    PlayerMapFactory.CreateMapExtent(request.playerId, request.mapExtentId, request.positionOnMap);
        //                    Console.WriteLine(string.Format("Player : {0} has Created a MapExtent, id : {1}, position : {2}",
        //                                                       request.playerId, request.mapExtentId, request.positionOnMap));
        //                }
        //                else
        //                {
        //                    CreateMapExtentResponse responseData = new CreateMapExtentResponse
        //                    {
        //                        isMapExtentValid = false,
        //                        mapExtentId = request.mapExtentId,
        //                        positionOnMap = request.positionOnMap,

        //                    };

        //                    using (Message response = Message.Create(CommunicationTag.PlayerMap.CREATE_MAP_EXTENT_RESPONSE, responseData))
        //                    {
        //                        e.Client.SendMessage(response, SendMode.Reliable);
        //                    }
        //                }




        //            }
        //        }
        //    }
        //}

        #endregion
    }
}

