using DarkRift;
using DarkRift.Server;
using RTS.Server.Messages;
using RTS.Database;
using RTS.Models;
using System;
using System.Collections.Generic;

namespace RTS.Plugins
{
    public class PlayerMarketManager : Plugin
    {
        #region Plugin implementation
        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public PlayerMarketManager(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {

        }

        #endregion

        #region Connection 

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.MessageReceived += OnClientMessageReceived;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// INIT
        /// </summary>
        public void Init()
        {
            //Listen to message
            ClientManager.ClientConnected += OnClientConnected;
        }

        /// <summary>
        /// Handle the player market request
        /// </summary>
        /// <param name="e"></param>
        private void PlayerMarketRequest(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.PlayerMarket.PLAYER_MARKET_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    //Get credential
                    CredentialMessage marketRequest = e.GetMessage().Deserialize<CredentialMessage>();

                    //If player is authorized
                    if (ConnectionManager.instance.CheckClientToken(marketRequest.playerId, marketRequest.token))
                    {
                        Client client = ConnectionManager.instance.GetClientByPlayerId(marketRequest.playerId);
                        if (client == null)
                            throw new Exception("[RTS Info] : There is no client for the player " + client.Simulation.Player.id);

                        PlayerMarketMessage playerMarketMessageData = new PlayerMarketMessage();
                        playerMarketMessageData.PlayerMarket = client.Simulation.PlayerMarket;

                        using (Message response = Message.Create(CommunicationTag.PlayerMarket.PLAYER_MARKET_RESPONSE, playerMarketMessageData))
                        {
                            e.Client.SendMessage(response, SendMode.Reliable);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handle the player trade request
        /// </summary>
        /// <param name="e"></param>
        private void CreateTradeRequest(MessageReceivedEventArgs e)
        {
            if (e.Tag == CommunicationTag.PlayerMarket.PLAYER_MARKET_TRADE_REQUEST)
            {
                using (DarkRiftReader reader = e.GetMessage().GetReader())
                {
                    //Get credential
                    PlayerMarketTradeMessage tradeRequest = e.GetMessage().Deserialize<PlayerMarketTradeMessage>();

                    //If player is authorized
                    if (ConnectionManager.instance.CheckClientToken(tradeRequest.playerId, tradeRequest.token))
                    {
                        Client client = ConnectionManager.instance.GetClientByPlayerId(tradeRequest.playerId);
                        if (client == null)
                            throw new Exception("[RTS Info] : There is no client for the player " + client.Simulation.Player.id);

                        //////////////////////////
                        /// Check for exactitude information
                        
                        //Control trade
                        int ratio = MarketData.GetAmountRatio(tradeRequest.PlayerTrade.resourceIdGiven, tradeRequest.PlayerTrade.resourceIdReceived);
                        if(ratio != tradeRequest.PlayerTrade.amountReceivedForOneGiven)
                            Console.WriteLine("[RTS Info] : The player "+ client.Simulation.Player.id + " hasn't send the good ratio "+ tradeRequest.PlayerTrade.amountReceivedForOneGiven + " for trade " + tradeRequest.PlayerTrade.resourceIdReceived + " to " + tradeRequest.PlayerTrade.resourceIdGiven);
                        
                        //check for exactitude
                        if (tradeRequest.PlayerTrade.AmountGiven != tradeRequest.amountGivenControl)
                            Console.WriteLine("[RTS Info] : The player " + client.Simulation.Player.id + " hasn't send the good given amount " + tradeRequest.amountGivenControl + " for trade " + tradeRequest.PlayerTrade.resourceIdReceived + " to " + tradeRequest.PlayerTrade.resourceIdGiven);

                        if (tradeRequest.PlayerTrade.AmountReceived != tradeRequest.amountReceivedControl)
                            Console.WriteLine("[RTS Info] : The player " + client.Simulation.Player.id + " hasn't send the good received amount " + tradeRequest.amountReceivedControl + " for trade " + tradeRequest.PlayerTrade.resourceIdReceived + " to " + tradeRequest.PlayerTrade.resourceIdGiven);

                        //TODO : send kick message if data are corrupted

                        ////////////////////////////
                        /// Trade and send message
                        
                        if (client.Simulation.PlayerMarket.Trade(tradeRequest.PlayerTrade))
                        {
                            Console.WriteLine("[RTS Info] : The player "+ client.Simulation.Player.id + " has successfully traded "+ tradeRequest.PlayerTrade.resourceIdReceived + " for " + tradeRequest.PlayerTrade.resourceIdGiven);

                            client.Simulation.Simulate();
                            client.Simulation.Persist();
                            PlayerMarketFactory.CreateTrade(client.Simulation.SQLConnexion, tradeRequest.PlayerTrade);
                        }
                        else
                        {
                            //TODO : Send a annulation message
                        }
                    }
                }
            }
        }
      

        #endregion

        #region Message handle

        private void OnClientMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Tag)
            {
                case CommunicationTag.PlayerMarket.PLAYER_MARKET_REQUEST:
                    PlayerMarketRequest(e);
                    break;
                case CommunicationTag.PlayerMarket.PLAYER_MARKET_TRADE_REQUEST:
                    CreateTradeRequest(e);
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
