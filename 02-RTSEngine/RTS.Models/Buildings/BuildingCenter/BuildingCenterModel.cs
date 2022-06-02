using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RTS.Models
{
    [Serializable]
    public class BuildingCenterModel : BuildingModel
    {
        #region Properties

        /// <summary>
        /// Model of the building
        /// </summary>
        public new List<BuildingCenterLevelModel> Levels
        {
            get { return GetLevels().Cast<BuildingCenterLevelModel>().ToList(); }
            set { SetLevels(value.Cast<BuildingLevelModel>().ToList()); }
        }

        /// <summary>
        /// List of levels
        /// </summary>
        [SerializeField]
        private List<BuildingCenterLevelModel> levels; 

        protected override List<BuildingLevelModel> GetLevels()
        {
            return levels.Cast<BuildingLevelModel>().ToList();
        }

        protected override void SetLevels(List<BuildingLevelModel> pLevels)
        {
            levels = pLevels.Cast<BuildingCenterLevelModel>().ToList();
        }

        #endregion
    }
}
