using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    /// <summary>
    /// Class wich store each detail for a building to produce something
    /// </summary>
    [Serializable]
    public class BuildingProducerLevelModel : BuildingLevelModel
    {

        /// <summary>
        /// Resource produced
        /// </summary>
        public int resourceIdProduced;
               
        /// <summary>
        /// List of resources needed to produce
        /// </summary>
        public ResourceBagModel consumptionBag;

        /// <summary>
        /// How many time to produce
        /// </summary>
        public int secondsToProduce;

        /// <summary>
        /// How many resource is produced
        /// </summary>
        public int amountProduced;
    }
}
