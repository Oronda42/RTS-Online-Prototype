using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RTS.Models
{
    /// <summary>
    /// Representation of a building
    /// </summary>
    [Serializable]
    public class BuildingModel
    {
        #region Properties

        /// <summary>
        /// Id of the building in the database
        /// </summary>
        public int id;

        /// <summary>
        /// Name of the building 
        /// </summary>
        public string name;

        /// <summary>
        /// type of the building
        /// </summary>
        public BuildingTypeModel buildingType;

        /// <summary>
        /// Time to construct this building
        /// </summary>
        public int constructionTime;

        /// <summary>
        /// A List of ContructionCost
        /// </summary>
        public ResourceBagModel cost;
                          
        #region Levels

        /// <summary>
        /// List of availables levels for the building
        /// </summary>
        public List<BuildingLevelModel> Levels
        {
            get { return GetLevels(); }
            set { SetLevels(value); }
        }

        /// <summary>
        /// List of levels
        /// </summary>
        [NonSerialized]
        private List<BuildingLevelModel> levels;

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected virtual List<BuildingLevelModel> GetLevels()
        {
            return levels;
        }

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected virtual void SetLevels(List<BuildingLevelModel> pLevels)
        {
            levels = pLevels;
        } 

        #endregion

        #endregion
    }
}
