using DarkRift;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerModelMessage : IDarkRiftSerializable
{

    public PlayerModel playerModel;

    public void Deserialize(DeserializeEvent e)
    {
       playerModel = new PlayerModel();
       playerModel.id =  e.Reader.ReadInt32();
       playerModel.nickname = e.Reader.ReadString();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(playerModel.id);
        e.Writer.Write(playerModel.nickname);
    }
}

