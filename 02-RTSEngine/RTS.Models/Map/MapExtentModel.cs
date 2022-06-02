using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RTS.Models
{
    [Serializable]
    public class MapExtentModel
    {
        /// <summary>
        /// Id of the extent
        /// </summary>
        public int id;

        /// <summary>
        /// Name of the extent
        /// </summary>
        public string name;

        /// <summary>
        /// size on x axis
        /// </summary>
        public int width;

        /// <summary>
        /// Size on the y axis
        /// </summary>
        public int depth;

        /// <summary>
        /// Interactive elements in the extent
        /// </summary>
        [SerializeField]
        public List<MapExtentElementModel> Elements;

        #region Implementation

        /// <summary>
        /// Returns the specified element wich the specified instance id if exists
        /// </summary>
        /// <param name="pElementInstanceId"></param>
        /// <returns></returns>
        public MapExtentElementModel GetMapElementByInstanceId(int pElementInstanceId)
        {
            return Elements.Where(e => e.InstanceId == pElementInstanceId).FirstOrDefault();
        }


        #endregion


    }
}
