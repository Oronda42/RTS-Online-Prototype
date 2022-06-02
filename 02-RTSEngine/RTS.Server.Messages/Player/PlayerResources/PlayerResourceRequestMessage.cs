using DarkRift;

namespace RTS.Server.Messages
{
    public class PlayerResourceRequestMessage : CredentialMessage, IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// Request for getting resource of this player
        /// </summary>
        public int playerIdResourceRequest;

        #endregion

        #region IDarkRiftSerializable implementation

        new public void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);
            playerIdResourceRequest = e.Reader.ReadInt32();
        }

        new public void Serialize(SerializeEvent e)
        {
            base.Serialize(e);
            e.Writer.Write(playerIdResourceRequest);
            
        }
        #endregion
    }
}
