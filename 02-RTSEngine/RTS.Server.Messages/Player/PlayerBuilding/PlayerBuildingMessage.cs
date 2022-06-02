using DarkRift;
using RTS.Models;
using System;
using System.Globalization;

namespace RTS.Server.Messages
{
    public class PlayerBuildingMessage : CredentialMessage
    {
        #region Properties

        public PlayerBuildingModel PlayerBuilding {
            get { return GetPlayerBuilding(); }
            set { SetPlayerBuilding(value); }
        }

        private PlayerBuildingModel playerBuilding;
        protected virtual PlayerBuildingModel GetPlayerBuilding() { return playerBuilding; }
        protected virtual void SetPlayerBuilding(PlayerBuildingModel pPlayerBuilding) { playerBuilding = pPlayerBuilding; }

        #endregion

        #region Custom IDarkRiftSerializable implementation

        public override void Deserialize(DeserializeEvent e)
        {
            if (PlayerBuilding == null)
                PlayerBuilding = new PlayerBuildingModel();

            base.Deserialize(e);

            
            PlayerBuilding.Player = new PlayerModel { id = e.Reader.ReadInt32() };
            PlayerBuilding.mapExtentId = e.Reader.ReadInt32();
            PlayerBuilding.mapElementInstanceId = e.Reader.ReadInt32();
            PlayerBuilding.buildingNumber = e.Reader.ReadInt32();
            PlayerBuilding.creation = DateTime.ParseExact(e.Reader.ReadString(), "MM/dd/yyyy HH:mm:ss", new CultureInfo("fr-FR"), DateTimeStyles.None);
            PlayerBuilding.State = new BuildingStateModel(e.Reader.ReadInt32());

        }

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);
            
            e.Writer.Write(PlayerBuilding.Player.id);
            e.Writer.Write(PlayerBuilding.mapExtentId);
            e.Writer.Write(PlayerBuilding.mapElementInstanceId);
            e.Writer.Write(PlayerBuilding.buildingNumber);
            e.Writer.Write(PlayerBuilding.creation.ToString("G", DateTimeFormatInfo.InvariantInfo));
            e.Writer.Write(PlayerBuilding.State.id);
            e.Writer.Write(PlayerBuilding.Building.id);
            e.Writer.Write(PlayerBuilding.Level.id);

        }

            #endregion
        }
}