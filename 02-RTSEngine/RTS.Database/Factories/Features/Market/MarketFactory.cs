using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Database
{
    public static class MarketFactory
    {
        /// <summary>
        /// Returns the market
        /// </summary>
        /// <param name="pConnection"></param>
        /// <returns></returns>
        public static MarketModel GetMarket(MySqlConnection pConnection)
        {
            MarketModel market = new MarketModel();
            market.Levels = GetMarketLevels(pConnection);

            return market;
        }

        /// <summary>
        /// Returns market levels
        /// </summary>
        /// <param name="pConnection"></param>
        /// <returns></returns>
        public static List<MarketLevelModel> GetMarketLevels(MySqlConnection pConnection)
        {
            string query = string.Format(@"SELECT 
                ml.id Id, ml.amount_receive_limit AmountReceivedLimit
                FROM {0} ml ",
                Constants.TableName.MARKET_LEVEL);

            //Object to return
            List<MarketLevelModel> levels = pConnection.Query<MarketLevelModel>(query).ToList();
            return levels;
        }

        /// <summary>
        /// Returns all ratios
        /// </summary>
        /// <param name="pConnection"></param>
        /// <returns></returns>
        public static List<MarketResourceRatioModel> GetAllRatios(MySqlConnection pConnection)
        {
            string query = string.Format(@"SELECT 
                m.resource_id_given resourceIdGiven, m.resource_id_received resourceIdReceived, m.amount_received_for_one_given amountReceivedForOneGiven
                FROM {0} m ",
                Constants.TableName.MARKET_RESOURCE_RATIO);

            //Object to return
            List<MarketResourceRatioModel> ratioToReturn = pConnection.Query<MarketResourceRatioModel>(query).ToList();

            return ratioToReturn;
        }
    }
}
