using DarkRift;
using RTS.Models;
using RTS.Server.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AllPlayerResponseMessage : CredentialMessage, IDarkRiftSerializable
{

    /// <summary>
    /// Number of Player in the message
    /// </summary>
    public int numberOfPlayer;

    /// <summary>
    /// Contains all buildings
    /// </summary>
    public List<PlayerModel> players;

    new public void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);

        numberOfPlayer = e.Reader.ReadInt32();

        if (numberOfPlayer!=0)
        {
            players = new List<PlayerModel>();

            for (int i = 0; i < numberOfPlayer; i++)
            {
                PlayerModel model = e.Reader.ReadSerializable<PlayerModelMessage>().playerModel;
                players.Add(model);
            }
        }
       
        
    }

    new public void Serialize(SerializeEvent e)
    {
        base.Serialize(e);

        e.Writer.Write(players.Count);

        for (int i = 0; i < players.Count; i++)
        {
            PlayerModelMessage playerModelMessageData = new PlayerModelMessage();
            playerModelMessageData.playerModel = players[i];
            playerModelMessageData.Serialize(e);
        }

    }
}

