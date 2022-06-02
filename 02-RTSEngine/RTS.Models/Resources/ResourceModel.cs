using System;

namespace RTS.Models
{
    /// <summary>
    /// Representation of a resource. Can be a unit or a consumable
    /// </summary>
    [Serializable]
    public class ResourceModel
    {
        #region Properties

        /// <summary>
        /// Id of the resource in the database
        /// </summary>
        public byte id;

        /// <summary>
        /// Name of the resource
        /// </summary>
        public string name;

        /// <summary>
        /// Resource type
        /// </summary>
        public ResourceTypeModel type;

        /// <summary>
        /// Is the resource can be found in the player map
        /// </summary>
        public bool AvailableInPlayerMap;
        
        #endregion
    }
}
