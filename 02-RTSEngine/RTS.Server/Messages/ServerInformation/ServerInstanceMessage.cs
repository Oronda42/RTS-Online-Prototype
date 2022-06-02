using DarkRift;
using RTS.Models.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public class ServerInstanceMessage : ServerCredentialMessage
    {
        #region Properties

        /// <summary>
        /// Server instance information
        /// </summary>
        public ServerInstanceModel ServerInstance { get; set; }

        #endregion

        #region IDarkRiftSerializable implementation 

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);

            if (ServerInstance == null)
                throw new Exception("ServerInstanceMessage : Server instance object is null");

            e.Writer.Write(ServerInstance.Name);
            e.Writer.Write(ServerInstance.Host);
            e.Writer.Write(ServerInstance.Port);
            e.Writer.Write(ServerInstance.Environment);
            e.Writer.Write(ServerInstance.Token);

        }

        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);

            ServerInstance = new ServerInstanceModel
            {
                Name = e.Reader.ReadString(),
                Host = e.Reader.ReadString(),
                Port = e.Reader.ReadInt32(),
                Environment = e.Reader.ReadString(),
                Token = e.Reader.ReadString(),
            };
        }

        #endregion

    }
}
