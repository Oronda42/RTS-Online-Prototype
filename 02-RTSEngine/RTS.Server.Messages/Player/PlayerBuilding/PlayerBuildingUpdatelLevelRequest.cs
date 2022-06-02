using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.Messages
{
    public class PlayerBuildingUpdatelLevelRequest : CredentialMessage, IDarkRiftSerializable
    {

        #region Properties

        /// <summary>
        /// Id of the building
        /// </summary>
        public int buildingId;

        /// <summary>
        /// BuildingNumber
        /// </summary>
        public int buildingNumber;

        /// <summary>
        /// Buillding Level ID
        /// </summary>
        public int buildingType;


        #endregion


        #region IDarkRiftSerializable implementation

        new public void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);
            buildingId = e.Reader.ReadInt32();
            buildingNumber = e.Reader.ReadInt32();
            buildingType = e.Reader.ReadInt32();
        }

        new public void Serialize(SerializeEvent e)
        {
            base.Serialize(e);
            e.Writer.Write(buildingId);
            e.Writer.Write(buildingNumber);
            e.Writer.Write(buildingType);
        }
        #endregion
    }
}
