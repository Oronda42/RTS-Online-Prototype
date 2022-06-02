
using DarkRift;
using RTS.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AllPlayerRequestMessage : CredentialMessage, IDarkRiftSerializable
{


    /// <summary>
    /// Request for getting resource of this player
    /// </summary>
    public string position;



    #region IDarkRiftSerializable implementation

    new public void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        position = e.Reader.ReadString();
    }

    new public void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
        e.Writer.Write(position);

    }

    #endregion
}