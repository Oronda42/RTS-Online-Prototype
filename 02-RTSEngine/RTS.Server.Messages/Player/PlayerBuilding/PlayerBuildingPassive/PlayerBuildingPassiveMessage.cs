using DarkRift;
using RTS.Configuration;
using RTS.Models;
using System;
using System.Globalization;

namespace RTS.Server.Messages
{
    public class PlayerBuildingPassiveMessage : PlayerBuildingMessage, IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// Data to be sent
        /// </summary>
        public new PlayerBuildingPassiveModel PlayerBuilding
        {
            get { return GetPlayerBuilding() as PlayerBuildingPassiveModel; }
            set { SetPlayerBuilding(value); }
        }
        private PlayerBuildingPassiveModel playerBuilding;

        protected override PlayerBuildingModel GetPlayerBuilding() { return playerBuilding; }
        protected override void SetPlayerBuilding(PlayerBuildingModel pPlayerBuilding) { playerBuilding = (PlayerBuildingPassiveModel)pPlayerBuilding; }

        #endregion

        #region IDarkRiftSerializable implementation

        public override void Deserialize(DeserializeEvent e)
        {
            PlayerBuilding = new PlayerBuildingPassiveModel();
            
            //Common building information
            base.Deserialize(e);

            //Building passive specific information
            PlayerBuilding.Building = new BuildingPassiveModel { id = e.Reader.ReadInt32() };
            PlayerBuilding.Level = new BuildingPassiveLevelModel { id = e.Reader.ReadInt32() };

            PlayerBuilding.startConsumption = DateTime.ParseExact(e.Reader.ReadString(), LocaleSettings.DATETIME_FORMAT, new CultureInfo(LocaleSettings.DATE_FORMAT_PROVIDER), DateTimeStyles.None);
            PlayerBuilding.lastConsumption = DateTime.ParseExact(e.Reader.ReadString(), LocaleSettings.DATETIME_FORMAT, new CultureInfo(LocaleSettings.DATE_FORMAT_PROVIDER), DateTimeStyles.None);

        }

        public override void Serialize(SerializeEvent e)
        {
            //Common building information
            base.Serialize(e);

            //Building passive specific information
            e.Writer.Write(PlayerBuilding.startConsumption.ToString(LocaleSettings.DATETIME_FORMAT_KEYCODE, DateTimeFormatInfo.InvariantInfo));
            e.Writer.Write(PlayerBuilding.lastConsumption.ToString(LocaleSettings.DATETIME_FORMAT_KEYCODE, DateTimeFormatInfo.InvariantInfo));

        }
        #endregion
    }
}




