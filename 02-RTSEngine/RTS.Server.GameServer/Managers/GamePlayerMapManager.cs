using DarkRift;
using DarkRift.Server;
using RTS.Database;
using RTS.Models;
using RTS.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.GameServer
{
    public static class GamePlayerMapManager
    {

        /// <summary>
        /// Send The PlayerMap to Client
        /// </summary>
        /// <param name="pThread"></param>
        /// <param name="pClient"></param>
        /// <param name="pMessage"></param>
        public static void SendPlayerMap(ThreadBase pThread, IClient pClient, CredentialMessage pMessage)
        {
            try
            {
                //If player is authorized
                if (GamePlayerCommunicationPlugin.Instance.CheckClientToken(pMessage.playerId, pMessage.token))
                {
                    ClientInformation client = GamePlayerCommunicationPlugin.Instance.GetClientByPlayerId(pMessage.playerId);
                    if (client == null)
                        throw new Exception("[RTS Info] : There is no client for the player " + client.Simulation.Player.id);


                    //Fill extent data & get player data
                    foreach (PlayerMapExtentModel playerExtent in client.Simulation.PlayerMap.Extents)
                    {
                        playerExtent.Extent = PlayerMapService.MapExtents.Where(me => me.id == playerExtent.Extent.id).FirstOrDefault();
                        playerExtent.PlayerElements = PlayerMapFactory.GetPlayerElements(pThread.DBConnection.Connection, playerExtent);
                    }

                    PlayerMapResponseMessage mapMessageData = new PlayerMapResponseMessage();
                    mapMessageData.SerializeCredentials = false;

                    mapMessageData.SetData(client.Simulation.PlayerMap);

                    //Create and send message
                    using (Message mapMessage = Message.Create(CommunicationTag.PlayerMap.PLAYER_MAP_RESPONSE, mapMessageData))
                    {
                        pClient.SendMessage(mapMessage, SendMode.Reliable);
                        Console.WriteLine("[RTS] INFO : Map sent to player " + pMessage.playerId);
                    }

                }
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }
    }
}
