












using System.Collections.Generic;
using System.Linq;

namespace RTS.Models
{
    public static class MarketData
    {

	    /// <summary>
        /// Returns the market
        /// </summary>
        /// <returns></returns>
        public static MarketModel GetMarket()
        {
            return new MarketModel
            {
                Levels = new List<MarketLevelModel>
                {

				new MarketLevelModel{ Id = 1, AmountReceivedLimit = 100 },

                }
            };
        }// end MarketModel()

		/// <summary>
        /// Returns all ratios
        /// </summary>
        /// <returns></returns>
		public static List<MarketResourceRatioModel> GetAllRatios(){

			return new List<MarketResourceRatioModel>{

				new MarketResourceRatioModel { resourceIdGiven = 2, resourceIdReceived = 1, amountReceivedForOneGiven = 2 },

				new MarketResourceRatioModel { resourceIdGiven = 3, resourceIdReceived = 1, amountReceivedForOneGiven = 5 },

				new MarketResourceRatioModel { resourceIdGiven = 3, resourceIdReceived = 2, amountReceivedForOneGiven = 2 },

				new MarketResourceRatioModel { resourceIdGiven = 4, resourceIdReceived = 1, amountReceivedForOneGiven = 10 },

				new MarketResourceRatioModel { resourceIdGiven = 4, resourceIdReceived = 2, amountReceivedForOneGiven = 5 },

				new MarketResourceRatioModel { resourceIdGiven = 4, resourceIdReceived = 3, amountReceivedForOneGiven = 2 },

				new MarketResourceRatioModel { resourceIdGiven = 5, resourceIdReceived = 1, amountReceivedForOneGiven = 15 },

				new MarketResourceRatioModel { resourceIdGiven = 5, resourceIdReceived = 2, amountReceivedForOneGiven = 5 },

				new MarketResourceRatioModel { resourceIdGiven = 5, resourceIdReceived = 3, amountReceivedForOneGiven = 3 },

				new MarketResourceRatioModel { resourceIdGiven = 5, resourceIdReceived = 4, amountReceivedForOneGiven = 5 },

				new MarketResourceRatioModel { resourceIdGiven = 6, resourceIdReceived = 1, amountReceivedForOneGiven = 20 },

				new MarketResourceRatioModel { resourceIdGiven = 6, resourceIdReceived = 2, amountReceivedForOneGiven = 15 },

				new MarketResourceRatioModel { resourceIdGiven = 6, resourceIdReceived = 3, amountReceivedForOneGiven = 5 },

				new MarketResourceRatioModel { resourceIdGiven = 6, resourceIdReceived = 4, amountReceivedForOneGiven = 5 },

				new MarketResourceRatioModel { resourceIdGiven = 6, resourceIdReceived = 5, amountReceivedForOneGiven = 2 },

			};

		}//End GetAllRatios();

		/// <summary>
        /// Positive means Amount * A = 1B. Negative means B / Amount = 1A. O means no ratio
        /// </summary>
        /// <param name="pResourceIdA"></param>
        /// <param name="pResourceIdB"></param>
        /// <returns></returns>
        public static int GetAmountRatio(int pResourceIdGiven, int pResourceIdReceived)
        {
            MarketResourceRatioModel ratio;
            //Try to get the ratio defined
            ratio = GetAllRatios().Where(r => r.resourceIdGiven == pResourceIdGiven && r.resourceIdReceived == pResourceIdReceived).FirstOrDefault();

            if (ratio != null)
            {
				return ratio.amountReceivedForOneGiven;                
            }
            else
            {
                //Otherwise, get the inverse
                ratio = GetAllRatios().Where(r => r.resourceIdGiven == pResourceIdReceived && r.resourceIdReceived == pResourceIdGiven).FirstOrDefault();
                if (ratio == null)
                    return 0;
                else
                    return ratio.amountReceivedForOneGiven * -1;
            }

        }//End GetAmountRatio

	}
}