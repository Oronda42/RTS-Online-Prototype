﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RTS.Models
{
    [Serializable]
    public class ResourceBagSlotModel
    {
        #region Properties

        /// <summary>
        /// The id of the resource in the slot
        /// </summary>
        public byte resourceId;

        /// <summary>
        /// the ammount of the resource in the slot
        /// </summary>
        public int amount;

        /// <summary>
        /// the limit of the resource in the slot (-1 means no maximum)
        /// </summary>
        public int maximum = -1;

        /// <summary>
        /// The usage of the resource (regards persistent resource only, 0 means no usage)
        /// </summary>
        public int used = 0;

        #endregion

        #region Constructor

        public ResourceBagSlotModel(byte pResourceId, int pAmount, int pMaximum, int pUsed)
        {
            resourceId = pResourceId;
            amount = pAmount;
            maximum = pMaximum;
            used = pUsed;
        }


        public ResourceBagSlotModel()
        {

        }

        #endregion
    }
}