using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.Messages
{
    public class PlayerBuildingDestroyRequest : CredentialMessage, IDarkRiftSerializable
    {

        #region Properties

        /// <summary>
        /// Id of the building
        /// </summary>
        public int buildingId;

        /// <summary>
        /// Position on the map (not the extent)
        /// </summary>
        public string positionOnMap;


        /// <summary>
        /// the number of building when create
        /// </summary>
        public int buildingNumber;

        /// <summary>
        /// The type of Building
        /// </summary>
        public int buildingTypeId;


        #endregion

        #region IDarkRiftSerializable implementation

        new public void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);
            buildingId = e.Reader.ReadInt32();
            buildingNumber = e.Reader.ReadInt32();
            buildingTypeId = e.Reader.ReadInt32();

        }

        new public void Serialize(SerializeEvent e)
        {
            base.Serialize(e);
            e.Writer.Write(buildingId);
            e.Writer.Write(buildingNumber);
            e.Writer.Write(buildingTypeId);
        }
        #endregion
    }
}

