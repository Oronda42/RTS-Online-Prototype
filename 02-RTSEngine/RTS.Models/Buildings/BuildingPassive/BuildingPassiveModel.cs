using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RTS.Models
{
    [Serializable]
    public class BuildingPassiveModel : BuildingModel
    {
        #region Properties

        /// <summary>
        /// Model of the building
        /// </summary>
        public new List<BuildingPassiveLevelModel> Levels
        {
            get { return GetLevels().Cast<BuildingPassiveLevelModel>().ToList(); }
            set { SetLevels(value.Cast<BuildingLevelModel>().ToList()); }
        }

        /// <summary>
        /// List of levels
        /// </summary>
        [SerializeField]
        private List<BuildingPassiveLevelModel> levels; 

        protected override List<BuildingLevelModel> GetLevels()
        {
            return levels.Cast<BuildingLevelModel>().ToList();
        }

        protected override void SetLevels(List<BuildingLevelModel> pLevels)
        {
            levels = pLevels.Cast<BuildingPassiveLevelModel>().ToList();
        }

        #endregion
    }
}
