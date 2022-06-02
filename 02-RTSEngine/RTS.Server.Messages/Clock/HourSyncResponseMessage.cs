using DarkRift;

namespace RTS.Server.Messages
{
    public class HourSyncResponseMessage : IDarkRiftSerializable
    {
        #region Properties
        public string serverTime;
        #endregion

        #region IDarkRiftSerializable
        public void Deserialize(DeserializeEvent e)
        {
            serverTime = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(serverTime);
        } 
        #endregion
    }
}

