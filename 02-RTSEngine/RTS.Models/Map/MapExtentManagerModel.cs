using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Models
{
    [Serializable]
    public class MapExtentManagerModel
    {
        #region Implementation

        /// <summary>
        /// List of extent available in this map (not modifiable)
        /// </summary>
        public List<MapExtentModel> Extents;

        #endregion


        #region Constructor

        public MapExtentManagerModel()
        {
            Extents = new List<MapExtentModel>();
        }

        #endregion

        #region Implementation

        /// <summary>
        /// returns the specified map extent
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public MapExtentModel GetById(int pId)
        {
            return Extents.Where(e => e.id == pId).FirstOrDefault();
        }

        /// <summary>
        /// Returns true if the specified id exists 
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public bool IsMapExtentIdExists(int pId)
        {
            if (Extents.Where(e => e.id == pId).FirstOrDefault() != null)
                return true;

            return false;
        }

        /// <summary>
        /// Add an extent to the list
        /// </summary>
        /// <param name="pExtent"></param>
        public void AddExtent(MapExtentModel pExtent)
        {
            if (!IsMapExtentIdExists(pExtent.id))
                Extents.Add(pExtent);
        }

        /// <summary>
        /// Remove all extents from the manager
        /// </summary>
        public void ClearExtents()
        {
            Extents = new List<MapExtentModel>();
        }

        /// <summary>
        /// Return the map element with the specified instance ID if exists
        /// </summary>
        /// <param name="pMapExtentId"></param>
        /// <param name="pElementInstanceId"></param>
        /// <returns></returns>
        public MapExtentElementModel GetMapElementByInstanceId(int pMapExtentId, int pElementInstanceId)
        {
            MapExtentElementModel mapExtentElement = null;

            MapExtentModel mapExtent = GetById(pMapExtentId);
            if (mapExtent != null)
            {
                mapExtentElement = mapExtent.GetMapElementByInstanceId(pElementInstanceId);
            }

            return mapExtentElement;
        }

        #endregion
    }
}
