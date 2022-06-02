using DarkRift;
using RTS.Models;
using System.Collections.Generic;


namespace RTS.Server.Messages
{
    public class PlayerResourcesResponseMessage : CredentialMessage, IDarkRiftSerializable
    {
        #region Properties

        /// <summary>
        /// Resource bag
        /// </summary>
        public ResourceBagModel ResourceBag;

        /// <summary>
        /// Number of Ressources. Needed to deserialize
        /// </summary>
        public int NumberOfResources;

        #endregion

        #region IDarkRiftSerializable implementation

        public override void Deserialize(DeserializeEvent e)
        {
            base.Deserialize(e);

            ResourceBag = new ResourceBagModel();
            NumberOfResources = e.Reader.ReadInt32();

            if (NumberOfResources != 0)
            {
                for (int i = 0; i < NumberOfResources; i++)
                {
                    //Read the message
                    ResourceBagSlotMessage resourceBagSlotMessage = new ResourceBagSlotMessage();
                    resourceBagSlotMessage.SerializeCredentials = false;
                    resourceBagSlotMessage.Deserialize(e);

                    ResourceBag.CreateOrUpdateSlot(resourceBagSlotMessage.ResourceBagSlot);
                }
            }
        }

        public override void Serialize(SerializeEvent e)
        {
            base.Serialize(e);

            //If there are extents, send them
            if (ResourceBag != null)
            {
                //Write number of extents
                e.Writer.Write(ResourceBag.resources.Count);
                
                for (int i = 0; i < ResourceBag.resources.Count; i++)
                {
                    //Read the message
                    ResourceBagSlotMessage resourceBagSlotMessage = new ResourceBagSlotMessage();
                    resourceBagSlotMessage.SerializeCredentials = false;
                    resourceBagSlotMessage.ResourceBagSlot = ResourceBag.resources[i];
                    resourceBagSlotMessage.Serialize(e);
                }                
            }
            else
                e.Writer.Write(0);

        }

        #endregion

    }
}
