using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    /// <summary>
    /// Reperesentation of a Player
    /// </summary>
    [Serializable]
    public class PlayerModel
    {
        #region Properties

        /// <summary>
        /// Id of the player in the database
        /// </summary>
        public int id = -1;

        /// <summary>
        /// Login of the player, must be unique
        /// </summary>
        public string nickname = "";

        /// <summary>
        /// Encrypted password of the player
        /// </summary>
        public string password;

        /// <summary>
        /// Use for identify the player when dialog with a server
        /// </summary>
        public string CurrentToken { get; set; } = "";

        /// <summary>
        /// Date creation of the player
        /// </summary>
        public DateTime creation;

        /// <summary>
        /// Resources that the players owns and are availables
        /// </summary>
        public ResourceBagModel resourceBag;

        /// <summary>
        /// Unique identifier of the device
        /// </summary>
        public string DeviceId { get; set; } = "";

        public string position;

        #endregion
    }
}
