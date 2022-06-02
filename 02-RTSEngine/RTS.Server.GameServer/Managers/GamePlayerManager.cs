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
    public static class GamePlayerManager
    {
        public static void SendAllPLayers(ThreadBase pThread, IClient pClient, AllPlayerRequestMessage pMessage)
        {
            try
            {
                //If player is authorized
                if (GamePlayerCommunicationPlugin.Instance.CheckClientToken(pMessage.playerId, pMessage.token))
                {
                    ClientInformation client = GamePlayerCommunicationPlugin.Instance.GetClientByPlayerId(pMessage.playerId);

                    List<PlayerModel> playersModel = PlayerFactory.GetAll(pMessage.playerId, pThread.DBConnection.Connection);

                    AllPlayerResponseMessage resourceMessageData = new AllPlayerResponseMessage
                    {
                        players = playersModel,
                        numberOfPlayer = playersModel.Count,
                        playerId = pMessage.playerId,
                        token = pMessage.token
                        
                    };

                    //Create and send message
                    using (Message resourceMessage = Message.Create(CommunicationTag.Player.ALL_PLAYERS_RESPONSE, resourceMessageData))
                    {
                        Console.WriteLine(string.Format("[RTS] INFO : All PLayers sent to player {0}", client.Simulation.Player.id));
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
