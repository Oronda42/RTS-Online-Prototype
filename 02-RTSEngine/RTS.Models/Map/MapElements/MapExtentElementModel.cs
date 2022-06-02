using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RTS.Models
{
    /// <summary>
    /// Class wich store the position of an element into the extent
    /// </summary>
    [Serializable]
    public class MapExtentElementModel
    {
        #region Properties

        [HideInInspector][NonSerialized]
        public MapExtentModel Extent;
        public MapElementModel Element;
        public string Position;

        /// <summary>
        /// Optional, otherwise, this is the id of the element
        /// </summary>
        public int EntityId;

        /// <summary>
        /// Id of the map element instance in the map extent
        /// </summary>
        public int InstanceId;

        #endregion
    }
}
