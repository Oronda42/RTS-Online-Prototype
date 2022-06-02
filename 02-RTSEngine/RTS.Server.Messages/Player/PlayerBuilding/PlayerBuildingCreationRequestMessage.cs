using DarkRift;


namespace RTS.Server.Messages
{
    public class PlayerBuildingCreationRequestMessage : CredentialMessage, IDarkRiftSerializable
    {

        #region Properties

        /// <summary>
        /// Id of the building
        /// </summary>
        public int buildingId;

        /// <summary>
        /// Map extent of the building
        /// </summary>
        public int mapExtentId;

        /// <summary>
        /// Map element position of the building
        /// </summary>
        public int mapElementInstanceId;


        #endregion


        #region IDarkRiftSerializable implementation

        new public void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);
            buildingId = e.Reader.ReadInt32();
            mapExtentId = e.Reader.ReadInt32();
            mapElementInstanceId = e.Reader.ReadInt32();
        }

        new public void Serialize(SerializeEvent e)
        {
            base.Serialize(e);
            e.Writer.Write(buildingId);
            e.Writer.Write(mapExtentId);
            e.Writer.Write(mapElementInstanceId);

        }
        #endregion
    }
}
