using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RTS.Models
{
    [Serializable]
    public class BuildingProducerModel : BuildingModel
    {
        #region Properties

        /// <summary>
        /// Default ressource produced by the building
        /// </summary>
        public int defaultResourceIdProduced;

        #region Levels

        /// <summary>
        /// Model of the building
        /// </summary>
        public new List<BuildingProducerLevelModel> Levels
        {
            get { return GetLevels().Cast<BuildingProducerLevelModel>().ToList(); }
            set { SetLevels(value.Cast<BuildingLevelModel>().ToList()); }
        }  

        /// <summary>
        /// List of levels
        /// </summary>
        [SerializeField]
        private List<BuildingProducerLevelModel> levels;

        protected override List<BuildingLevelModel> GetLevels()
        {
            return levels.Cast<BuildingLevelModel>().ToList();
        }

        protected override void SetLevels(List<BuildingLevelModel> pLevels)
        {
            levels = pLevels.Cast<BuildingProducerLevelModel>().ToList();
        }

        #endregion

        #endregion
    }
}
