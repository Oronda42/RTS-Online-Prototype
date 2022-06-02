using Dapper;
using MySql.Data.MySqlClient;
using RTS.Configuration;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Database
{
    public class PlayerMarketFactory
    {
        #region Implementation

        /// <summary>
        /// Create a new market for the player
        /// </summary>
        /// <param name="pTransaction"></param>
        /// <param name="pPlayer"></param>
        /// <returns></returns>
        public static PlayerMarketModel CreatePlayerMarket(MySqlTransaction pTransaction, PlayerModel pPlayer)
        {
            PlayerMarketModel playerMarket = new PlayerMarketModel
            {
                Player = pPlayer,
                Trades = new List<PlayerMarketTradeModel>(),
                Level = new MarketLevelModel { Id = 1}
            };

            //Query
            string query = string.Format(@"
            INSERT INTO {0} (player_id, level_id) 
            VALUES
            ({1},1)",
            Constants.TableName.PLAYER_MARKET,
            pPlayer.id);

            pTransaction.Connection.Query(query);

            return playerMarket;
        }

        /// <summary>
        /// Returns the player market
        /// </summary>
        /// <param name="pConnection"></param>
        /// <param name="pPlayer"></param>
        /// <returns></returns>
        public static PlayerMarketModel GetPlayerMarket(MySqlConnection pConnection, PlayerModel pPlayer)
        {
            PlayerMarketModel playerMarket = new PlayerMarketModel
            {
                Player = pPlayer,
            };
            
            //Get level
            string query = string.Format(@"
            SELECT level_id 
            FROM {0} pm 
            WHERE player_id={1}",
            Constants.TableName.PLAYER_MARKET,
            pPlayer.id);

            //Get level Id
            int levelId = pConnection.QueryFirst<int>(query);
            playerMarket.Level = new MarketLevelModel { Id = levelId };
            playerMarket.Trades = GetTradesForDay(pConnection, pPlayer, DateTime.Now);

            return playerMarket;
        }

        /// <summary>
        /// Returns all trade for a player on the specified day
        /// </summary>
        /// <param name="pConnection"></param>
        /// <param name="pPlayer"></param>
        /// <param name="pDay"></param>
        /// <returns></returns>
        public static List<PlayerMarketTradeModel> GetTradesForDay(MySqlConnection pConnection, PlayerModel pPlayer, DateTime pDay)
        {
            List<PlayerMarketTradeModel> trades = new List<PlayerMarketTradeModel>();

            string query = string.Format(@"SELECT 
            pmt.resource_id_given resourceIdGiven, pmt.resource_id_received resourceIdReceived, pmt.quantity, pmt.amount_received_for_one_given amountReceivedForOneGiven, pmt.creation 
            FROM {0} pmt
            WHERE player_id={1} 
            AND DATE_FORMAT(pmt.creation,'%d/%m/%Y') = '{2}' ",
            Constants.TableName.PLAYER_MARKET_TRADES,
            pPlayer.id,
            pDay.ToString(LocaleSettings.DATE_FORMAT_KEYCODE));

            pConnection.Query(
                query,
                new[] {
                    typeof(PlayerMarketTradeModel),
                },
                objects =>
                {
                    PlayerMarketTradeModel trade = objects[0] as PlayerMarketTradeModel;
                    trade.playerId = pPlayer.id;

                    trades.Add(trade);
                    return trade;
                });

            return trades;
        }
        
        /// <summary>
        /// Create a Trade
        /// </summary>
        /// <param name="pMapextentId"></param>
        /// <param name="pPositionOnMap"></param>
        public static void CreateTrade(MySqlConnection pConnection, PlayerMarketTradeModel pTrade)
        {
            //Query
            string query = string.Format(@"
            INSERT INTO {0} (player_id, resource_id_given, resource_id_received, quantity, amount_received_for_one_given, creation) 
            VALUES
            ({1},{2},{3},{4},{5},NOW())",
            Constants.TableName.PLAYER_MARKET_TRADES,
            pTrade.playerId,
            pTrade.resourceIdGiven,
            pTrade.resourceIdReceived,
            pTrade.quantity,
            pTrade.amountReceivedForOneGiven);

            pConnection.Query(query);

        }

        #endregion
    }
}
