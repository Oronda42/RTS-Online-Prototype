using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.Messages
{
    public class PlayerDisconnectionMessage : IDarkRiftSerializable
    {
        #region Properties

        public ushort DisconnectionMessageId { get; set; }

        #endregion

        #region IDarkRiftSerializable

        public void Deserialize(DeserializeEvent e)
        {
            DisconnectionMessageId = e.Reader.ReadByte();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(DisconnectionMessageId);
        }

        #endregion

    }
}
