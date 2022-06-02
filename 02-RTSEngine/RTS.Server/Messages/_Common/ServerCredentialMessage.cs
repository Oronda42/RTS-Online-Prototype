using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public class ServerCredentialMessage : IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// If true, data are serialized, if false, not serialized
        /// </summary>
        public bool SerializeCredentials { get; set; } = true;

        /// <summary>
        /// The login of the player
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The token of the Player
        /// </summary>
        public string Token { get; set; }

        #endregion

        #region IDarkRiftSerializable implementation

        public virtual void Deserialize(DeserializeEvent e)
        {
            SerializeCredentials = e.Reader.ReadBoolean();
            if (SerializeCredentials)
            {
                ServerName = e.Reader.ReadString();
                Token = e.Reader.ReadString();
            }
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(SerializeCredentials);
            if (SerializeCredentials)
            {
                e.Writer.Write(ServerName);
                e.Writer.Write(Token);
            }
        }
        #endregion
    }
}
