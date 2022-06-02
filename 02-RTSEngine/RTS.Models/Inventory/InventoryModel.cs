using System;
using System.Collections.Generic;
using System.Linq;


namespace RTS.Models
{
    public class InventoryModel
    {
        #region Properties
        List<InventorySlotModel> slots = new List<InventorySlotModel>(); 
        #endregion

        #region Implementation

        public void Init(int pinventorySize)
        {
            for (int i = 0; i < pinventorySize; i++)
            {
                slots.Add(new InventorySlotModel(i));
            }
        }

        /// <summary>
        /// add Item to inventory, return the items left if inventory is full
        /// </summary>
        /// <param name="pItem"></param>
        /// <param name="pAmount"></param>
        /// <returns></returns>
        public int AddItem(IInventoriableModel pItem, int pAmount)
        {
            while (pAmount > 0)
            {
                InventorySlotModel slot = GetAvailableSlot(pItem);

                if (slot == null)
                    return pAmount; // There is no slot available, inventory is full

                if (pAmount <= 0) // There is no item left to put in slot
                    return 0;

                else
                {
                    if (slot.item == null) // The slot is free
                    {
                        slot.item = pItem;
                        slot.item.InventorySettings.position = slot.position; // set the position of the item

                        if (pAmount >= pItem.InventorySettings.stackLimit) //  fill the slot with a full stack of item
                        {
                            slot.itemAmount = pItem.InventorySettings.stackLimit;
                            pAmount -= slot.itemAmount;
                        }
                        else // there is not enough ammount to fill the slot entirely
                        {
                            slot.itemAmount = pAmount;
                            pAmount -= slot.itemAmount;
                        }
                    }
                    else // The slot is not free, we add items to it
                    {
                        int ammountToAdd = slot.item.InventorySettings.stackLimit - slot.itemAmount;
                        slot.itemAmount += ammountToAdd;
                        pAmount -= ammountToAdd;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Remove an object for the inventory
        /// </summary>
        /// <param name="pItem"></param>
        /// <param name="pAmount"></param>
        /// <returns></returns>
        public bool RemoveItem(IInventoriableModel pItem, int pAmount)
        {
            int amountToRemove = pAmount;

            while (amountToRemove > 0)
            {
                InventorySlotModel slot = GetSlot(pItem);

                if (slot == null)
                    break; // There is no slot with this is item to remove

                if (amountToRemove <= 0) // There is no item left to remove
                    return true;

                if (amountToRemove >= pItem.InventorySettings.stackLimit) // Remove a full Stack of item
                {
                    amountToRemove -= slot.itemAmount;
                    slot.itemAmount = 0;
                    slot.item = null;
                }
                else
                {
                    slot.itemAmount -= amountToRemove;
                    amountToRemove = 0;
                }
            }
            return false;
        }

        /// <summary>
        /// Get a Available slot for this object or a free slot if there is not already a slot with for object
        /// </summary>
        /// <param name="pObject"></param>
        /// <returns></returns>
        public InventorySlotModel GetAvailableSlot(IInventoriableModel pObject)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].item != null)
                {
                    if (CompareItem(slots[i].item, pObject))
                    {
                        if (slots[i].IsAvailable)
                        {
                            return slots[i];
                        }
                    }
                }
            }
            return GetFreeSlot();
        }

        /// <summary>
        /// compare two items
        /// </summary>
        /// <param name="pItem1"></param>
        /// <param name="pItem2"></param>
        /// <returns></returns>
        private bool CompareItem(IInventoriableModel pItem1, IInventoriableModel pItem2)
        {
            return pItem1.InventorySettings.itemId == pItem2.InventorySettings.itemId
                   && pItem1.InventorySettings.itemType.id == pItem2.InventorySettings.itemType.id;
        }

        /// <summary>
        /// return an object in the inventory
        /// </summary>
        /// <param name="pObject"></param>
        /// <returns></returns>
        public InventorySlotModel GetSlot(IInventoriableModel pObject)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (CompareItem(slots[i].item, pObject))
                {
                    return slots[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Return A free slot
        /// </summary>
        /// <returns></returns>
        public InventorySlotModel GetFreeSlot()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].item == null)
                {
                    return slots[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Get an Object with the position in the inventory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pPosition"></param>
        /// <returns></returns>
        public T GetObjectByPosition<T>(int pPosition)
        {
            IInventoriableModel item = slots.Where(x => x.item.InventorySettings.position == pPosition).FirstOrDefault().item;
            return (T)item;
        } 
        #endregion

    }
}
