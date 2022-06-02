using DarkRift;
using RTS.Configuration;
using RTS.Models;
using System;
using System.Globalization;

namespace RTS.Server.Messages
{
    public class PlayerMapExtentElementMessage : CredentialMessage, IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// Player map extent
        /// </summary>
        public PlayerMapExtentElementModel PlayerMapExtentElement;

        #endregion

        #region Implementation

        public void SetData(PlayerMapExtentElementModel pPlayerMapExtentElement)
        {
            PlayerMapExtentElement = pPlayerMapExtentElement;
        }

        #endregion

        #region IDarkRiftSerializable implementation

        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);

            PlayerMapExtentElement = new PlayerMapExtentElementModel
            {
                Element = new MapElementModel { Id = e.Reader.ReadInt32() },
                PlayerExtent = new PlayerMapExtentModel {
                    Map = new PlayerMapModel { owner = new PlayerModel { id = e.Reader.ReadInt32() } },
                    Extent = new MapExtentModel { id = e.Reader.ReadInt32() }
                },
                EntityId = e.Reader.ReadInt32(),
                mapElementInstanceId = e.Reader.ReadInt32(), 
            };
        }

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);

            e.Writer.Write(PlayerMapExtentElement.Element.Id);
            e.Writer.Write(PlayerMapExtentElement.PlayerExtent.Map.owner.id);
            e.Writer.Write(PlayerMapExtentElement.PlayerExtent.Extent.id);         
            e.Writer.Write(PlayerMapExtentElement.EntityId);            
            e.Writer.Write(PlayerMapExtentElement.mapElementInstanceId);            
        }

        #endregion

    }
}
