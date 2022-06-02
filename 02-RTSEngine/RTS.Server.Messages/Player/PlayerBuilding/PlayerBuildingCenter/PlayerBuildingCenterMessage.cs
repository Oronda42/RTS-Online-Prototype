using DarkRift;
using RTS.Configuration;
using RTS.Models;
using System;
using System.Globalization;

namespace RTS.Server.Messages
{
    public class PlayerBuildingCenterMessage : PlayerBuildingMessage, IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// Data to be sent
        /// </summary>
        public new PlayerBuildingCenterModel PlayerBuilding
        {
            get { return GetPlayerBuilding() as PlayerBuildingCenterModel; }
            set { SetPlayerBuilding(value); }
        }
        private PlayerBuildingCenterModel playerBuilding;

        protected override PlayerBuildingModel GetPlayerBuilding() { return playerBuilding; }
        protected override void SetPlayerBuilding(PlayerBuildingModel pPlayerBuilding) { playerBuilding = (PlayerBuildingCenterModel)pPlayerBuilding; }

        #endregion

        #region IDarkRiftSerializable implementation

        public override void Deserialize(DeserializeEvent e)
        {
            PlayerBuilding = new PlayerBuildingCenterModel();

            //Common building information
            base.Deserialize(e);

            //Building center specific information
            PlayerBuilding.Building = new BuildingCenterModel { id = e.Reader.ReadInt32() };
            PlayerBuilding.Level = new BuildingCenterLevelModel { id = e.Reader.ReadInt32() };
        }

        public new void Serialize(SerializeEvent e)
        {
            //Common building information
            base.Serialize(e);

            //Building center specific information

        }
        #endregion
    }
}




