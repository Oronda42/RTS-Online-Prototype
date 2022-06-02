namespace RTS.Models
{
    public class InventoryItemSettingsModel
    {
        #region Properties

        /// <summary>
        /// index position of the item in the inventory
        /// </summary>
        public int position;

        /// <summary>
        /// the number of item that can be stacked in one inventory slot
        /// </summary>
        public int stackLimit;

        /// <summary>
        /// unique indentifier of the item with that type
        /// </summary>
        public int itemId;

        /// <summary>
        /// type of item
        /// </summary>
        public InventoryItemTypeModels itemType;

        #endregion

        #region Constructor

        public InventoryItemSettingsModel(int pPosition, int pStackLimit, int pItemId, InventoryItemTypeModels pItemType)
        {

            position = pPosition;
            stackLimit = pStackLimit;
            itemId = pItemId;
            itemType = pItemType;
        } 

        #endregion
    }
}
