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
    public static class GamePlayerResourceManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pThread">Thread that execute the event</param>
        /// <param name="pClient">Client to send the message</param>
        /// <param name="pMessage">Message to handle</param>
        public static void SendResources(ThreadBase pThread, IClient pClient, PlayerResourceRequestMessage pMessage)
        {
            try
            {
                //If player is authorized
                if (GamePlayerCommunicationPlugin.Instance.CheckClientToken(pMessage.playerId, pMessage.token))
                {
                    ClientInformation client = GamePlayerCommunicationPlugin.Instance.GetClientByPlayerId(pMessage.playerId);

                    PlayerResourcesResponseMessage resourceMessageData = new PlayerResourcesResponseMessage
                    {
                        ResourceBag = client.Simulation.Player.resourceBag,
                        playerId = pMessage.playerId,
                        token = pMessage.token,
                    };

                    //Create and send message
                    using (Message resourceMessage = Message.Create(CommunicationTag.PlayerConsumableResources.ALL_RESOURCE_RESPONSE, resourceMessageData))
                    {
                        Console.WriteLine(string.Format("[RTS] INFO : Ressource sent to player {0}", client.Simulation.Player.id));
                        pClient.SendMessage(resourceMessage, SendMode.Reliable);
                    }
                }
                else
                    throw new Exception("Player is not logged in");
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }
    }
}
