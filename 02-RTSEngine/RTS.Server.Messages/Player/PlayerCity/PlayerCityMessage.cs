using DarkRift;
using RTS.Models;
using System.Collections.Generic;

namespace RTS.Server.Messages
{
    public class PlayerCityMessage : CredentialMessage , IDarkRiftSerializable
    {

        public PlayerCityModel PlayerCity
        {
            get { return GetPlayerCity(); }
            set { SetPlayerBuilding(value); }
        }

        private PlayerCityModel playerCity;
        protected virtual PlayerCityModel GetPlayerCity() { return playerCity; }
        protected virtual void SetPlayerBuilding(PlayerCityModel pPlayerCity) { playerCity = pPlayerCity; }



        public void SetData(PlayerCityModel pData)
        {
            PlayerCity = pData;
        }

        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);
            if (PlayerCity == null)
                PlayerCity = new PlayerCityModel();

            PlayerCity.playerId = e.Reader.ReadInt32();
            PlayerCity.position = e.Reader.ReadString();
            PlayerCity.level = new PlayerCityLevelModel { id = e.Reader.ReadInt32() };
        }

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);
            e.Writer.Write(PlayerCity.playerId);
            e.Writer.Write(PlayerCity.position);
            e.Writer.Write(PlayerCity.level.id);
        }
    }
}
