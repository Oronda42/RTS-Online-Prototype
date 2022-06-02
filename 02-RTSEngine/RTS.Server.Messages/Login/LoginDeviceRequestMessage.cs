using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RTS.Server.Messages
{
    public class LoginDeviceRequestMessage : IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// The login of the player
        /// </summary>
        public string  deviceId;

        /// <summary>
        /// The password of the Player
        /// </summary>
        public string deviceToken;

        #endregion

        #region IDarkRiftSerializable implementation

        public void Deserialize(DeserializeEvent e)
        {
            deviceId = e.Reader.ReadString();
            deviceToken = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(deviceId);
            e.Writer.Write(deviceToken);
        }

        #endregion
    }
}

