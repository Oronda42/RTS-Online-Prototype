using DarkRift;
using RTS.Models;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Server.Messages
{
    public class PlayerMapResponseMessage : CredentialMessage, IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// Number of extents. Needed to deserialize
        /// </summary>
        public int numberOfExtents;

        /// <summary>
        /// Data of the map
        /// </summary>
        public PlayerMapModel PlayerMap;

        #endregion

        #region Implementation

        public void SetData(PlayerMapModel pPlayerMap)
        {
            PlayerMap = pPlayerMap;
        }

        #endregion

        #region IDarkRiftSerializable implementation

        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);

            PlayerMap = new PlayerMapModel();

            PlayerMap.owner = new PlayerModel() { id = e.Reader.ReadInt32() };
            PlayerMap.name = e.Reader.ReadString();

            numberOfExtents = e.Reader.ReadInt32();

            if (numberOfExtents != 0)
            {
                PlayerMap.Extents = new List<PlayerMapExtentModel>();
                for (int i = 0; i < numberOfExtents; i++)
                {
                    PlayerMapExtentMessage playerMapExtentMessage = new PlayerMapExtentMessage();
                    playerMapExtentMessage.SerializeCredentials = false;
                    playerMapExtentMessage.Deserialize(e);

                    PlayerMapExtentModel mapExtent = playerMapExtentMessage.PlayerMapExtent;
                    PlayerMap.Extents.Add(mapExtent);
                }
            }

        }

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);

            e.Writer.Write(PlayerMap.owner.id);
            e.Writer.Write(PlayerMap.name);

            //If there are extents, send them
            if (PlayerMap.Extents != null)
            {
                //Write number of extents
                e.Writer.Write(PlayerMap.Extents.Count());

                foreach (PlayerMapExtentModel pme in PlayerMap.Extents)
                {
                    PlayerMapExtentMessage playerMapExtentMessage =  new PlayerMapExtentMessage();
                    playerMapExtentMessage.SetData(pme);
                    playerMapExtentMessage.SerializeCredentials = false;

                    playerMapExtentMessage.Serialize(e);
                }
            }
            else
            {
                //Write number extent = 0
                e.Writer.Write(0);
            }
        }

        #endregion
    }
}
