using DarkRift;
using RTS.Configuration;
using RTS.Models;
using System;
using System.Globalization;

namespace RTS.Server.Messages
{
    public class PlayerBuildingProducerMessage : PlayerBuildingMessage, IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// Data to be sent
        /// </summary>
        public new PlayerBuildingProducerModel PlayerBuilding
        {
            get { return GetPlayerBuilding() as PlayerBuildingProducerModel; }
            set { SetPlayerBuilding(value); }
        }
        private PlayerBuildingProducerModel playerBuilding;

        protected override PlayerBuildingModel GetPlayerBuilding() { return playerBuilding; }
        protected override void SetPlayerBuilding(PlayerBuildingModel pPlayerBuilding) { playerBuilding = (PlayerBuildingProducerModel)pPlayerBuilding; }

        #endregion

        #region IDarkRiftSerializable implementation

        public override void Deserialize(DeserializeEvent e)
        {
            PlayerBuilding = new PlayerBuildingProducerModel();

            //Common building information
            base.Deserialize(e);

            //Building producer specific information
            PlayerBuilding.Building = new BuildingProducerModel { id = e.Reader.ReadInt32() };
            PlayerBuilding.Level = new BuildingProducerLevelModel { id = e.Reader.ReadInt32() };

            PlayerBuilding.startProduction = DateTime.ParseExact(e.Reader.ReadString(), LocaleSettings.DATETIME_FORMAT, new CultureInfo(LocaleSettings.DATE_FORMAT_PROVIDER), DateTimeStyles.None);
            PlayerBuilding.lastProduction = DateTime.ParseExact(e.Reader.ReadString(), LocaleSettings.DATETIME_FORMAT, new CultureInfo(LocaleSettings.DATE_FORMAT_PROVIDER), DateTimeStyles.None);
            PlayerBuilding.autoProduce = e.Reader.ReadBoolean();
            PlayerBuilding.currentResourceIdProduced = e.Reader.ReadInt32();

        }

        public override void Serialize(SerializeEvent e)
        {
            //Common building information
            base.Serialize(e);

            //Building producer specific information
            e.Writer.Write(PlayerBuilding.startProduction.ToString(LocaleSettings.DATETIME_FORMAT_KEYCODE, DateTimeFormatInfo.InvariantInfo));
            e.Writer.Write(PlayerBuilding.lastProduction.ToString(LocaleSettings.DATETIME_FORMAT_KEYCODE, DateTimeFormatInfo.InvariantInfo));
            e.Writer.Write(PlayerBuilding.autoProduce);
            e.Writer.Write(PlayerBuilding.currentResourceIdProduced);
        }
        #endregion
    }
}
