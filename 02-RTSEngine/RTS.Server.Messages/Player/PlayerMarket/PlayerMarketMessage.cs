using DarkRift;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.Messages
{
    public class PlayerMarketMessage : CredentialMessage, IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// Player market
        /// </summary>
        public PlayerMarketModel PlayerMarket;

        /// <summary>
        /// Number of trades in the player market
        /// </summary>
        public int numberOfTrades;

        #endregion

        #region IDarkRiftSerializable implementation 

        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);

            PlayerMarket = new PlayerMarketModel
            {
                Player = new PlayerModel { id = e.Reader.ReadInt32() },
                Level = new MarketLevelModel { Id = e.Reader.ReadInt32() },
            };

            PlayerMarket.Trades = new List<PlayerMarketTradeModel>();

            /////////////////////////
            // Deserialize player trades
            numberOfTrades = e.Reader.ReadInt32();

            if (numberOfTrades != 0)
            {
                for (int i = 0; i < numberOfTrades; i++)
                {
                    PlayerMarketTradeMessage playerTradeMessage = new PlayerMarketTradeMessage();
                    playerTradeMessage.Deserialize(e);
                    PlayerMarket.Trades.Add(playerTradeMessage.PlayerTrade);
                }
            }

        }

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);

            e.Writer.Write(PlayerMarket.Player.id);
            e.Writer.Write(PlayerMarket.Level.Id);

            /////////////////////////
            // Serialize player trades
            e.Writer.Write(PlayerMarket.Trades.Count);
            for (int i = 0; i < PlayerMarket.Trades.Count; i++)
            {
                PlayerMarketTradeMessage playerTradeMessage = new PlayerMarketTradeMessage();
                playerTradeMessage.PlayerTrade = PlayerMarket.Trades[i];
                playerTradeMessage.SerializeCredentials = false;

                playerTradeMessage.Serialize(e);
            }

        }

        #endregion

    }
}
