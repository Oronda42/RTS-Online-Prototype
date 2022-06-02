using DarkRift;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.Messages
{
    public class NeighboorsCityResponseMessage : CredentialMessage, IDarkRiftSerializable
    {
        public List<PlayerCityModel>  neighboorsCities;

        int neighboorsCount;
        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);
            neighboorsCount = e.Reader.ReadInt32();

            if (neighboorsCount != 0)
            {
                List<PlayerCityModel> neighboorsCitiesDeserialized = new List<PlayerCityModel>();
                for (int i = 0; i < neighboorsCount; i++)
                {
                    PlayerCityModel playerCity = e.Reader.ReadSerializable<PlayerCityMessage>().PlayerCity;
                    neighboorsCitiesDeserialized.Add(playerCity);
                }
                neighboorsCities = neighboorsCitiesDeserialized;
            }
        }

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);
            e.Writer.Write(neighboorsCities.Count);

            for (int i = 0; i < neighboorsCities.Count; i++)
            {
                PlayerCityMessage playerCitymessage = new PlayerCityMessage();
                playerCitymessage.SerializeCredentials = false;
                playerCitymessage.SetData(neighboorsCities[i]);
                playerCitymessage.Serialize(e);
            }
        }
    }
}
