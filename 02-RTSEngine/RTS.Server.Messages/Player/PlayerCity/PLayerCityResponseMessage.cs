using DarkRift;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.Messages
{
    public class PLayerCityResponseMessage : CredentialMessage, IDarkRiftSerializable
    {

        public PlayerCityModel model;
        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);
            PlayerCityModel playerCity = e.Reader.ReadSerializable<PlayerCityMessage>().PlayerCity;
            model = playerCity;
        }

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);
            PlayerCityMessage playerCitymessage = new PlayerCityMessage();
            playerCitymessage.SetData(model);
            playerCitymessage.Serialize(e);
        }
    }
}
