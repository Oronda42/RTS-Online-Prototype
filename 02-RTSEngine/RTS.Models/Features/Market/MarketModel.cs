using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    [Serializable]
    public class MarketModel
    {
        /// <summary>
        /// Levels of the market
        /// </summary>
        public List<MarketLevelModel> Levels;

    }
}
