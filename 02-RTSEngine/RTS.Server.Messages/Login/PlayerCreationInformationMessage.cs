using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RTS.Server.Messages
{
    public class PlayerCreationInformationMessage : IDarkRiftSerializable
    {

        #region Properties

        /// <summary>
        /// The nickname of the player
        /// </summary>
        public string nickname;

        /// <summary>
        /// The deviceId of the player
        /// </summary>
        public string deviceId;

        #endregion

        #region IDarkRiftSerializable implementation

        public void Deserialize(DeserializeEvent e)
        {
            nickname = e.Reader.ReadString();
            deviceId = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(nickname);
            e.Writer.Write(deviceId);
        }

        #endregion
    }
}

