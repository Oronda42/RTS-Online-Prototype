using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RTS.Models
{
    [Serializable]
    public class PlayerMapExtentElementModel
    {
        #region Properties


        /// <summary>
        /// Extent of the player
        /// </summary>
        [NonSerialized]
        public PlayerMapExtentModel PlayerExtent;

        /// <summary>
        /// Element of the player
        /// </summary>
        public MapElementModel Element;

        /// <summary>
        /// Entity id of the player element
        /// </summary>
        public int EntityId;

        /// <summary>
        /// Id of the map element instance in the map extent
        /// </summary>
        public int mapElementInstanceId;


        #endregion
    }
}
