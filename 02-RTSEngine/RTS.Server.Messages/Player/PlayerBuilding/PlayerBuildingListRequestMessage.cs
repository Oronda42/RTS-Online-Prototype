using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.Messages
{
    public class PlayerBuildingListRequestMessage : CredentialMessage, IDarkRiftSerializable
    {
        #region Properties

        public int playerIdBuildings;

        #endregion

        #region IDarkRiftSerializable implementation

        new public void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);
            playerIdBuildings = e.Reader.ReadInt32();
        }

        new public void Serialize(SerializeEvent e)
        {
            base.Serialize(e);
            e.Writer.Write(playerIdBuildings);
        }
        #endregion
    }
}
