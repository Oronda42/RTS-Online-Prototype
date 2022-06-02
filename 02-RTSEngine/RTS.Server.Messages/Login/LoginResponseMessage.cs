using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.Messages
{
    public class LoginResponseMessage : IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// Code of the error return when trying to login
        /// </summary>
        public int loginErrorCode;

        /// <summary>
        /// Token for the session
        /// </summary>
        public string token;

        /// <summary>
        /// Id of the player
        /// </summary>
        public int id;

        /// <summary>
        /// Nickname of the Player
        /// </summary>
        public string nickname;

        /// <summary>
        /// Host's address of the world server
        /// </summary>
        public string RedirectedServerHost { get; set; }

        /// <summary>
        /// Port of the world server
        /// </summary>
        public int RedirectedServerPort { get; set; }

        /// <summary>
        /// Name of the server
        /// </summary>
        public string RedirectedServerName { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public LoginResponseMessage()
        {
            //Create serializable object
            loginErrorCode = -1;
            nickname = "";
            token = "";
            id = -1;
            RedirectedServerHost = "";
            RedirectedServerPort = 0;
            RedirectedServerName = "";

        }

        #endregion

        #region IDarkRiftSerializable implementation 

        public void Deserialize(DeserializeEvent e)
        {
            loginErrorCode = e.Reader.ReadByte();
            nickname = e.Reader.ReadString();
            token = e.Reader.ReadString();
            id = e.Reader.ReadInt32();
            RedirectedServerHost = e.Reader.ReadString();
            RedirectedServerPort = e.Reader.ReadInt32();
            RedirectedServerName = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write((byte)loginErrorCode);
            e.Writer.Write(nickname);
            e.Writer.Write(token);
            e.Writer.Write(id);
            e.Writer.Write(RedirectedServerHost);
            e.Writer.Write(RedirectedServerPort);
            e.Writer.Write(RedirectedServerName);
        }

        #endregion
    }
}
