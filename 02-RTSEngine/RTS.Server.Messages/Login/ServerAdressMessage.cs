using DarkRift;

namespace RTS.Server.Messages
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerAdressMessage : IDarkRiftSerializable
    {

        #region Properties

        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        #endregion

        #region IDarkRiftSerializable

        public void Deserialize(DeserializeEvent e)
        {
            Host = e.Reader.ReadString();
            Host = e.Reader.ReadString();
            Port = e.Reader.ReadInt16();

        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Name);
            e.Writer.Write(Host);
            e.Writer.Write(Port);
        }

        #endregion
    }
}
