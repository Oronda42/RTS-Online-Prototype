using DarkRift;
using System.Collections.Generic;
using RTS.Models;

namespace RTS.Server.Messages
{
    public class PlayerBuildingProducerListResponseMessage : CredentialMessage, IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// Number of building in the message
        /// </summary>
        public int numberOfBuilding;

        /// <summary>
        /// Contains all buildings
        /// </summary>
        public List<PlayerBuildingProducerModel> buildings;

        #endregion

        #region IDarkRiftSerializable implementation

        new public void Deserialize(DeserializeEvent e)
        {
            numberOfBuilding = e.Reader.ReadInt32();

            if (numberOfBuilding != 0)
            {
                buildings = new List<PlayerBuildingProducerModel>();
                for (int i = 0; i < numberOfBuilding; i++)
                {
                    PlayerBuildingProducerModel building = e.Reader.ReadSerializable<PlayerBuildingProducerMessage>().PlayerBuilding;
                    buildings.Add(building);
                }
            }
        }

       new public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(buildings.Count);

            //If there are extents, send them
            for (int i = 0; i < buildings.Count; i++)
            {
                PlayerBuildingProducerMessage buildingMessageData = new PlayerBuildingProducerMessage();
                buildingMessageData.PlayerBuilding = buildings[i];
                buildingMessageData.playerId = playerId;
                buildingMessageData.token = token;
                buildingMessageData.Serialize(e);
            } 
        }

        #endregion


    }
}
