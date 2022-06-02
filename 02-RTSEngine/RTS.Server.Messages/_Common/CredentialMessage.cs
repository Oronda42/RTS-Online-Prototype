using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.Messages
{
    /// <summary>
    /// This class has to be used only when you need to specify login and token
    /// </summary>
    public class CredentialMessage : IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// If true, data are serialized, if false, not serialized
        /// </summary>
        public bool SerializeCredentials = true; 

        /// <summary>
        /// The login of the player
        /// </summary>
        public int playerId;

        /// <summary>
        /// The token of the Player
        /// </summary>
        public string token;

        #endregion

        #region IDarkRiftSerializable implementation

        public virtual void Deserialize(DeserializeEvent e)
        {            
            SerializeCredentials = e.Reader.ReadBoolean();
            if (SerializeCredentials)
            {
                playerId = e.Reader.ReadInt32();
                token = e.Reader.ReadString();
            }
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write(SerializeCredentials); 
            if (SerializeCredentials)
            {
                e.Writer.Write(playerId);
                e.Writer.Write(token);
            }
        } 

        #endregion
    }
}
