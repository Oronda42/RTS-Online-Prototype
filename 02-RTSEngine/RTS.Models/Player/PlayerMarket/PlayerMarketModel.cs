using RTS.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    public class PlayerMarketModel
    {
        #region Properties

        /// <summary>
        /// Reference to the player
        /// </summary>
        public PlayerModel Player;

        /// <summary>
        /// Level of the player market
        /// </summary>
        public MarketLevelModel Level;

        /// <summary>
        /// Trades of the day
        /// </summary>
        public List<PlayerMarketTradeModel> Trades;

        #endregion

        #region Implementation

        /// <summary>
        /// Returns true if the player can trade this amount
        /// </summary>
        /// <param name="pAmountReceived"></param>
        /// <returns></returns>
        public bool CanTrade(PlayerMarketTradeModel pTrade)
        {
            if (GetRemainingAmountToReceive() >= pTrade.AmountReceived &&
                Player.resourceBag.GetAmountByResourceId(pTrade.resourceIdGiven) >= pTrade.AmountGiven)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Returns true if the trade has been performed
        /// </summary>
        /// <param name="pResourceIdReceived"></param>
        /// <param name="pResourceIdGiven"></param>
        /// <param name="pAmountReceived"></param>
        /// <returns></returns>
        public bool Trade(PlayerMarketTradeModel pTrade)
        {
            if(CanTrade(pTrade))
            {
                pTrade.creation = DateTime.Now;
                Trades.Add(pTrade);

                //substract resources
                Player.resourceBag.Add(pTrade.resourceIdGiven, -pTrade.AmountGiven);
                //Add resources
                Player.resourceBag.Add(pTrade.resourceIdReceived, pTrade.AmountReceived);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Remaining amount allowed
        /// </summary>
        /// <returns></returns>
        public int GetRemainingAmountToReceive()
        {
            return Level.AmountReceivedLimit - GetAmountReceived();
        }

        /// <summary>
        /// Remaining amount allowed
        /// </summary>
        /// <returns></returns>
        public int GetAmountReceived()
        {
            string currentDay = DateTime.Now.ToString(LocaleSettings.DATE_FORMAT_KEYCODE);
            List<PlayerMarketTradeModel> dayTrades = Trades.Where(t => t.creation.ToString(LocaleSettings.DATE_FORMAT_KEYCODE) == currentDay).ToList();

            int amountReceived = 0;
            for (int i = 0; i < dayTrades.Count; i++)
            {
                amountReceived += dayTrades[i].AmountReceived;
            }

            return amountReceived; 
        } 

        #endregion
    }
}
