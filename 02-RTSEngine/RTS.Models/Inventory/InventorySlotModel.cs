namespace RTS.Models
{
    public class InventorySlotModel
    {
        #region Properties

        /// <summary>
        /// The item inside the slot
        /// </summary>
        public IInventoriableModel item;

        /// <summary>
        /// The amount of item in the slot
        /// </summary>
        public int itemAmount;

        /// <summary>
        /// true is the slot is not full
        /// </summary>
        public bool IsAvailable { get => IsSlotAvailable(); }

        /// <summary>
        /// index position in the inventory
        /// </summary>
        public int position;


        private bool IsSlotAvailable()
        {
            if (item == null)
                return true;

            if (itemAmount >= item.InventorySettings.stackLimit)
                return false;
            else
                return true;
        }

        #endregion

        #region Constructor

        public InventorySlotModel(int position)
        {
            this.position = position;
        } 

        #endregion

    }
}
