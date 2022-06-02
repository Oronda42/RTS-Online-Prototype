using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Models
{
    [Serializable]
    public class PlayerMapExtentModel
    {
        #region Properties

        /// <summary>
        /// Who own the extent
        /// </summary>
        [HideInInspector] 
        public PlayerMapModel Map;

        /// <summary>
        /// Extent owned
        /// </summary>
        [HideInInspector]
        public MapExtentModel Extent;

        /// <summary>
        /// Creation date of the extent for the player
        /// </summary>
        public DateTime Creation;

        /// <summary>
        /// Elements of the extent for the player
        /// </summary>
        public List<PlayerMapExtentElementModel> PlayerElements;



        #endregion
    }
}
