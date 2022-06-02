using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    [Serializable]
    public class MarketResourceRatioModel
    {
        /// <summary>
        /// Id of Resource given
        /// </summary>
        public int resourceIdGiven;

        /// <summary>
        /// Id of Resource received
        /// </summary>
        public int resourceIdReceived;

        /// <summary>
        /// The ammount received for 1 given
        /// </summary>
        public int amountReceivedForOneGiven;

    }
}
