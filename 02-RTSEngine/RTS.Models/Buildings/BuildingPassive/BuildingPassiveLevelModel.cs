using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    [Serializable]
    public class BuildingPassiveLevelModel : BuildingLevelModel
    {
       
        /// <summary>
        /// List of persistents resources provided by the building
        /// </summary>
        public ResourceBagModel persistentBag;

        /// <summary>
        /// Seconds between each consumption
        /// </summary>
        public int secondsToConsume;

        /// <summary>
        /// Cost for the building continue to works
        /// </summary>
        public ResourceBagModel consumptionBag;
    }
}
