using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{

    public class AttributeModifierModel
    {
        #region Properties

        /// <summary>
        /// ID of the modifier
        /// </summary>
        public int Id;

        /// <summary>
        /// name of the modifier
        /// </summary>
        public string Name;

        /// <summary>
        /// Start time when the modifier is applied
        /// </summary>
        public DateTime Start;

        /// <summary>
        /// Duration of the effector
        /// </summary>
        public int Duration;

        /// <summary>
        /// Amount given by the modifier
        /// </summary>
        public float Value;

        #endregion

        #region Constructor

        public AttributeModifierModel()
        {

        }

        #endregion
    }
}
