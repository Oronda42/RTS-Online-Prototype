using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RTS.Models
{
    [Serializable]
    public class PlayerMapModel
    {
        #region Properties

        /// <summary>
        /// Owner of the map
        /// </summary>
        [HideInInspector]
        public PlayerModel owner;

        /// <summary>
        /// Name of the map
        /// </summary>
        public string name;

        /// <summary>
        /// Extents owned by the playre
        /// </summary>
        [HideInInspector]
        [NonSerialized]
        public List<PlayerMapExtentModel> Extents;

        #endregion

        #region Implementation

        /// <summary>
        /// Returns the player extent specified as parameter
        /// </summary>
        /// <param name="pExtentId"></param>
        /// <returns></returns>
        public PlayerMapExtentModel GetExtent(int pExtentId)
        {
            return Extents.Where(e => e.Extent.id == pExtentId).FirstOrDefault();
        }


        #endregion
    }
}