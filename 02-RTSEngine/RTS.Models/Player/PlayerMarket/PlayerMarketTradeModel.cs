using System;

namespace RTS.Models
{
    public class PlayerMarketTradeModel
    {
        #region Properties

        /// <summary>
        /// Player id of the trade
        /// </summary>
        public int playerId;

        /// <summary>
        /// Resource given for the trade
        /// </summary>
        public int resourceIdGiven;

        /// <summary>
        /// Resource received for the trade
        /// </summary>
        public int resourceIdReceived;

        /// <summary>
        /// Amount received
        /// </summary>
        public int AmountReceived
        {
            get
            {
                if (amountReceivedForOneGiven == 0)
                    return 0;

                if (amountReceivedForOneGiven > 0)
                    return quantity * amountReceivedForOneGiven;
                else
                    return quantity;
            }
        }

        /// <summary>
        /// Amount Given
        /// </summary>
        public int AmountGiven
        {
            get
            {
                if (amountReceivedForOneGiven == 0)
                    return 0;

                if (amountReceivedForOneGiven > 0)
                    return quantity;
                else
                    return quantity * amountReceivedForOneGiven * -1;
            }
        }

        /// <summary>
        /// This is the quantity of ratio. Enable to calculate amounts
        /// </summary>
        public int quantity;

        /// <summary>
        /// This is the ratio for the trade (negative means 1/ (amountReceivedForOneGiven*-1))
        /// </summary>
        public int amountReceivedForOneGiven;

        /// <summary>
        /// Date of the trade
        /// </summary>
        public DateTime creation;

        #endregion

        #region Implementation


        #endregion

    }
}
