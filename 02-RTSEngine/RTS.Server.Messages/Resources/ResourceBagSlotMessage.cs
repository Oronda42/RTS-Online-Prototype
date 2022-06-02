using DarkRift;
using RTS.Models;
using System.Collections.Generic;

namespace RTS.Server.Messages
{
    //TODO : inherits from Credential message
    public class ResourceBagSlotMessage : CredentialMessage, IDarkRiftSerializable
    {

        #region MyRegion

        /// <summary>
        /// Data to be sent
        /// </summary>
        public ResourceBagSlotModel ResourceBagSlot;

        #endregion

        #region IDarkRiftSerializable implementation

        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);

            ResourceBagSlot = new ResourceBagSlotModel
            {
                resourceId = e.Reader.ReadByte(),
                amount = e.Reader.ReadInt32(),
                used = e.Reader.ReadInt32(),
                maximum = e.Reader.ReadInt32(),
            };
        }

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);

            e.Writer.Write(ResourceBagSlot.resourceId);
            e.Writer.Write( ResourceBagSlot.amount);
            e.Writer.Write(ResourceBagSlot.used);
            e.Writer.Write(ResourceBagSlot.maximum);
        }

        #endregion

    }
}
