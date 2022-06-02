using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    [Serializable]
    public class BuildingCenterLevelModel : BuildingLevelModel
    {
       
        /// <summary>
        /// List of persistents resources provided by the building
        /// </summary>
        public ResourceBagModel persistentBag;

    }
}
