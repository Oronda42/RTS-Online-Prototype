using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    [Serializable]
    public class BuildingLevelModel
    {
        /// <summary>
        /// Level of the building
        /// </summary>
        public int id;

        /// <summary>
        /// Cost of the level
        /// </summary>
        public ResourceBagModel cost;

        /// <summary>
        /// List of persistents resources needed by the building to works correctly
        /// </summary>
        public ResourceBagModel persistentBagNeeded;
    }
}
