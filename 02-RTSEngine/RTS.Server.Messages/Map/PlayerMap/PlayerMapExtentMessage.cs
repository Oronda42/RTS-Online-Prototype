using DarkRift;
using RTS.Configuration;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace RTS.Server.Messages
{
    public class PlayerMapExtentMessage : CredentialMessage, IDarkRiftSerializable
    {
        #region Properties


        /// <summary>
        /// Number of extents. Needed to deserialize
        /// </summary>
        public int numberOfElements;

        /// <summary>
        /// Player map extent
        /// </summary>
        public PlayerMapExtentModel PlayerMapExtent;

        #endregion

        #region Implementation

        public void SetData(PlayerMapExtentModel pPlayerMapExtent)
        {
            PlayerMapExtent = pPlayerMapExtent;
        }

        #endregion

        #region IDarkRiftSerializable implementation

        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);

            PlayerMapExtent = new PlayerMapExtentModel
            {
                Map = new PlayerMapModel { owner = new PlayerModel { id = e.Reader.ReadInt32() } },
                Extent = new MapExtentModel { id = e.Reader.ReadInt32() },
                Creation = DateTime.ParseExact(e.Reader.ReadString(), LocaleSettings.DATETIME_FORMAT, new CultureInfo(LocaleSettings.DATE_FORMAT_PROVIDER), DateTimeStyles.None)
            }; 
             
            /////////////////////////
            // Deserialize player map elements
            numberOfElements = e.Reader.ReadInt32();

            if (numberOfElements != 0)
            {
                PlayerMapExtent.PlayerElements = new List<PlayerMapExtentElementModel>();
                for (int i = 0; i < numberOfElements; i++)
                {
                    PlayerMapExtentElementMessage playerMapExtentElementMessage = new PlayerMapExtentElementMessage();
                    playerMapExtentElementMessage.Deserialize(e);

                    PlayerMapExtentElementModel playerElement = playerMapExtentElementMessage.PlayerMapExtentElement;
                    PlayerMapExtent.PlayerElements.Add(playerElement);
                }
            }
        }

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);

            e.Writer.Write(PlayerMapExtent.Map.owner.id);
            e.Writer.Write(PlayerMapExtent.Extent.id);
            e.Writer.Write(PlayerMapExtent.Creation.ToString(LocaleSettings.DATETIME_FORMAT_KEYCODE, DateTimeFormatInfo.InvariantInfo));

            /////////////////////////
            // Serialize player map elements
            //If there are extents, send them
            if (PlayerMapExtent.PlayerElements != null)
            {
                //Write number of extents
                e.Writer.Write(PlayerMapExtent.PlayerElements.Count);

                foreach (PlayerMapExtentElementModel pmee in PlayerMapExtent.PlayerElements)
                {
                    PlayerMapExtentElementMessage playerMapExtentElementMessage = new PlayerMapExtentElementMessage();
                    playerMapExtentElementMessage.SetData(pmee);
                    playerMapExtentElementMessage.SerializeCredentials = false;
                    playerMapExtentElementMessage.Serialize(e);
                }
            }
            else
            {
                //Write number extent = 0
                e.Writer.Write(0);
            }
        }

        #endregion

    }
}
