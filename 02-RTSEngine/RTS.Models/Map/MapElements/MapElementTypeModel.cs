using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    [Serializable]
    public class MapElementTypeModel
    {
        #region Properties

        /// <summary>
        /// ID of the map element type
        /// </summary>
        public int Id;

        /// <summary>
        /// Name of the map element type
        /// </summary>
        public string Name;

        #endregion
    }
}
