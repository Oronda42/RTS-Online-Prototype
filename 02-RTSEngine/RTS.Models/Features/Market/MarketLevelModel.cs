using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Models
{
    [Serializable]
    public class MarketLevelModel
    {
        #region Properties

        /// <summary>
        /// Level of the market
        /// </summary>
        public int Id;

        /// <summary>
        /// Limit trade per day
        /// </summary>
        public int AmountReceivedLimit;


        #endregion     
    }
}
