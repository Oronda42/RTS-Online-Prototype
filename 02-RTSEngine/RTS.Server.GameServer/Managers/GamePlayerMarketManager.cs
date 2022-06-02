using DarkRift;
using DarkRift.Server;
using RTS.Database;
using RTS.Models;
using RTS.Server.Messages;
using System;

namespace RTS.Server.GameServer
{
    public static class GamePlayerMarketManager
    {
        /// <summary>
        /// Handle the player market request
        /// </summary>
        /// <param name="pThread"></param>
        /// <param name="pClient"></param>
        /// <param name="pMessage"></param>
        public static void HandlePlayerMarketRequest(ThreadBase pThread, IClient pClient, CredentialMessage pMessage)
        {
            try
            {
                //If player is authorized
                if (GamePlayerCommunicationPlugin.Instance.CheckClientToken(pMessage.playerId, pMessage.token))
                {
                    ClientInformation client = GamePlayerCommunicationPlugin.Instance.GetClientByPlayerId(pMessage.playerId);
                    if (client == null)
                        throw new Exception("[RTS Info] : There is no client for the player " + client.Simulation.Player.id);

                    PlayerMarketMessage playerMarketMessageData = new PlayerMarketMessage
                    {
                        PlayerMarket = client.Simulation.PlayerMarket,
                        playerId = pMessage.playerId,
                        token = pMessage.token
                    };


                    using (Message response = Message.Create(CommunicationTag.PlayerMarket.PLAYER_MARKET_RESPONSE, playerMarketMessageData))
                    {
                        pClient.SendMessage(response, SendMode.Reliable);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, ex.Message, null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }
        }

        /// <summary>
        ///  Handle the player trade request
        /// </summary>
        /// <param name="pThread"></param>
        /// <param name="pClient"></param>
        /// <param name="pMessage"></param>
        public static void CreateTradeRequest(ThreadBase pThread, IClient pClient, PlayerMarketTradeMessage pMessage)
        {
            try
            {
                //If player is authorized
                if (GamePlayerCommunicationPlugin.Instance.CheckClientToken(pMessage.playerId, pMessage.token))
                {
                    ClientInformation client = GamePlayerCommunicationPlugin.Instance.GetClientByPlayerId(pMessage.playerId);
                    if (client == null)
                        throw new Exception("[RTS Info] : There is no client for the player " + client.Simulation.Player.id);

                    //////////////////////////
                    /// Check for exactitude information

                    //Control trade
                    int ratio = MarketData.GetAmountRatio(pMessage.PlayerTrade.resourceIdGiven, pMessage.PlayerTrade.resourceIdReceived);
                    if (ratio != pMessage.PlayerTrade.amountReceivedForOneGiven)
                        Console.WriteLine("[RTS Info] : The player " + client.Simulation.Player.id + " hasn't send the good ratio " + pMessage.PlayerTrade.amountReceivedForOneGiven + " for trade " + pMessage.PlayerTrade.resourceIdReceived + " to " + pMessage.PlayerTrade.resourceIdGiven);

                    //check for exactitude
                    if (pMessage.PlayerTrade.AmountGiven != pMessage.amountGivenControl)
                        Console.WriteLine("[RTS Info] : The player " + client.Simulation.Player.id + " hasn't send the good given amount " + pMessage.amountGivenControl + " for trade " + pMessage.PlayerTrade.resourceIdReceived + " to " + pMessage.PlayerTrade.resourceIdGiven);

                    if (pMessage.PlayerTrade.AmountReceived != pMessage.amountReceivedControl)
                        Console.WriteLine("[RTS Info] : The player " + client.Simulation.Player.id + " hasn't send the good received amount " + pMessage.amountReceivedControl + " for trade " + pMessage.PlayerTrade.resourceIdReceived + " to " + pMessage.PlayerTrade.resourceIdGiven);

                    //TODO : send kick message if data are corrupted

                    ////////////////////////////
                    /// Trade and send message

                    if (client.Simulation.PlayerMarket.Trade(pMessage.PlayerTrade))
                    {
                        Console.WriteLine("[RTS Info] : The player " + client.Simulation.Player.id + " has successfully traded " + pMessage.PlayerTrade.resourceIdReceived + " for " + pMessage.PlayerTrade.resourceIdGiven);

                        client.Simulation.Simulate();
                        client.Simulation.Persist(pThread.DBConnection.Connection);
                        PlayerMarketFactory.CreateTrade(pThread.DBConnection.Connection, pMessage.PlayerTrade);
                    }
                    else
                    {
                        //TODO : Send a annulation message
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
