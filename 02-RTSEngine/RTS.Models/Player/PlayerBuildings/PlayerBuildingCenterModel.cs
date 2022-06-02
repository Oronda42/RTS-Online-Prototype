using System;
using UnityEngine;

namespace RTS.Models
{
    public class PlayerBuildingCenterModel : PlayerBuildingModel
    {
        #region Static

        public static PlayerBuildingCenterModel CreateDefault(DateTime pTime, PlayerModel pPlayer)
        {
            PlayerBuildingCenterModel playerCenter = new PlayerBuildingCenterModel();

            //Set values
            playerCenter.Building = BuildingData.CommandCenter;
            playerCenter.creation = pTime;            

            playerCenter.Init(pTime, pPlayer);

            return playerCenter;
        }

        #endregion

        #region Properties

        #region Building
        /// <summary>
        /// Model of the building
        /// </summary>
        public new BuildingCenterModel Building
        {
            get { return GetBuilding() as BuildingCenterModel; }
            set { SetBuilding(value); }
        }
        private BuildingCenterModel buildingCenter;

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected override BuildingModel GetBuilding()
        {
            return buildingCenter;
        }

        protected override void SetBuilding(BuildingModel pBuilding)
        {
            buildingCenter = pBuilding as BuildingCenterModel;
            FireOnBuildingUpdated();
        }

        #endregion

        #region Level

        /// <summary>
        /// List of availables levels for the building
        /// </summary>
        public new BuildingCenterLevelModel Level
        {
            get { return GetLevel() as BuildingCenterLevelModel; }
            set { SetLevel(value); }
        }

        /// <summary>
        /// List of levels
        /// </summary>
        [SerializeField]
        private BuildingCenterLevelModel level;

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected override BuildingLevelModel GetLevel()
        {
            return level;
        }

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected override void SetLevel(BuildingLevelModel pLevel)
        {
            level = pLevel as BuildingCenterLevelModel;
        }

        #endregion

        #endregion

        #region Constructor

        public PlayerBuildingCenterModel()
        { }

        public PlayerBuildingCenterModel(BuildingCenterModel pBuilding)
        {
            Building = pBuilding;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Initialize the player building
        /// </summary>
        /// <param name="pTimeCursor"></param>
        /// <param name="pPlayer"></param>
        public override void Init(DateTime pTimeCursor, PlayerModel pPlayer)
        {
            base.Init(pTimeCursor, pPlayer);
            State = BuildingStateData.Active;
        }

        #endregion
    }
}
