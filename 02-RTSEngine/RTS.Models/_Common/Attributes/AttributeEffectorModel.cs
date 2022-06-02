using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    public class AttributeEffectorModel
    {
        #region Properties
        /// <summary>
        /// Id of the effector
        /// </summary>
        public int Id { private set; get; } 

        /// <summary>
        /// Name of the effector
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
        /// Frequence when the effector is applied
        /// </summary>
        public float Frequency;

        /// <summary>
        /// elapsed time since last frequency
        /// </summary>
        public float ElapsedTime;

        /// <summary>
        /// Amount
        /// </summary>
        public float Value;

        /// <summary>
        /// List of modifiers for this attribute
        /// </summary>
        public List<AttributeModifierModel> Modifiers;

        /// <summary>
        /// List of effectors for this attribute
        /// </summary>
        public List<AttributeEffectorModel> Effectors;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AttributeEffectorModel() { }

        #endregion
    }
}
