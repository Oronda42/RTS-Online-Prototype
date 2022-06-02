using DarkRift;

namespace RTS.Server.Messages
{
    public class PlayerBuildingCreationResponseMessage : IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// Is the Building is Valid
        /// </summary>
        public bool isBuidlingValid;

        /// <summary>
        /// The position of the of the building in the database table
        /// </summary>
        public int buildingNumber;

        /// <summary>
        /// map extent id where the building is built
        /// </summary>
        public int mapExtentId;

        /// <summary>
        /// Map element instance ID where the building is built
        /// </summary>
        public int mapElementInstanceId;

        #endregion

        #region IDarkRiftSerializable implementation

        public void Deserialize(DeserializeEvent e)
        {
            isBuidlingValid = e.Reader.ReadBoolean();
            buildingNumber = e.Reader.ReadInt32();
            mapExtentId = e.Reader.ReadInt32();
            mapElementInstanceId = e.Reader.ReadInt32();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(isBuidlingValid);
            e.Writer.Write(buildingNumber);
            e.Writer.Write(mapExtentId);
            e.Writer.Write(mapElementInstanceId);
        }

        #endregion
    }
}
