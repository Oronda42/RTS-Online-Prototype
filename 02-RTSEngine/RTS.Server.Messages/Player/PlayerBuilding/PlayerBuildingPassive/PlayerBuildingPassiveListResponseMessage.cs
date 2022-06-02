using DarkRift;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.Messages
{
    public class PlayerBuildingPassiveListResponseMessage : CredentialMessage, IDarkRiftSerializable
    {

        #region Properties

        /// <summary>
        /// Number of building in the message
        /// </summary>
        public int numberOfBuilding;

        /// <summary>
        /// Contains all buildings
        /// </summary>
        public List<PlayerBuildingPassiveModel> playerBuildings;

        #endregion

        #region IDarkRiftSerializable implementation

        new public void Deserialize(DeserializeEvent e)
        {
            numberOfBuilding = e.Reader.ReadInt32();

            if (numberOfBuilding != 0)
            {
                playerBuildings = new List<PlayerBuildingPassiveModel>();
                for (int i = 0; i < numberOfBuilding; i++)
                {
                    PlayerBuildingPassiveModel playerBuilding = e.Reader.ReadSerializable<PlayerBuildingPassiveMessage>().PlayerBuilding;
                    playerBuildings.Add(playerBuilding);
                }
            }
        }

        new public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(playerBuildings.Count);

            //If there are extents, send them
            foreach (PlayerBuildingPassiveModel b in playerBuildings)
            {
                PlayerBuildingPassiveMessage buildingMessageData = new PlayerBuildingPassiveMessage();
                buildingMessageData.PlayerBuilding = b;
                buildingMessageData.token = token;
                buildingMessageData.playerId = playerId;
                buildingMessageData.Serialize(e);
            }
        }

        #endregion

    }
}
