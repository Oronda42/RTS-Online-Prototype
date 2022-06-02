using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace RTS.Models
{
    public class AttributeModel
    {
        #region Delegates

        public delegate void AttributeEventHandler(AttributeModel pAttribute);

        #endregion

        #region Events
        
        public event AttributeEventHandler OnValueChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Original value of the attribute
        /// </summary>
        public float StartValue { protected set; get; }

        /// <summary>
        /// Current value of the stat
        /// </summary>
        [SerializeField]
        public float CurrentValue { protected set; get; }

        /// <summary>
        /// Maximum value allowed
        /// </summary>
        [SerializeField]
        public float MaxValue { protected set; get; }

        /// <summary>
        /// Minimum value allowed
        /// </summary>
        [SerializeField]
        public float MinValue { protected set; get; }

        /// <summary>
        /// If the attribute is active
        /// </summary>
        [SerializeField]
        public bool IsActive { protected set; get; }

        /// <summary>
        /// Last value before change
        /// </summary>
        public float LastValue { protected set; get; }
        
        #endregion
               
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AttributeModel() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pCurrentValue"></param>
        /// <param name="pMinValue"></param>
        /// <param name="pMaxValue"></param>
        public AttributeModel(float pStartValue, float pCurrentValue, float pMinValue, float pMaxValue, bool pIsActive, float pLastValue)
        {
            StartValue = pStartValue;
            CurrentValue = pCurrentValue;
            LastValue = pCurrentValue;
            MinValue = pMinValue;
            MaxValue = pMaxValue;
            IsActive = pIsActive;
            LastValue = pLastValue;
        }

        public AttributeModel(float pMinValue, float pMAxValue)
        {
            StartValue = 0;
            CurrentValue = 0;
            LastValue = 0;
            MinValue = pMinValue;
            MaxValue = pMAxValue;
        }


        #endregion

        #region Implementation
        /// <summary>
        /// Add value to the attribute (Respect max and min)
        /// </summary>
        /// <param name="pValue"></param>
        public void AddValue(float pValue)
        {
            CurrentValue += pValue;

            if (CurrentValue < MinValue)
                CurrentValue = MinValue;

            if (CurrentValue > MaxValue)
                CurrentValue = MaxValue;

            FireOnValueChanged();
        }

        /// <summary>
        /// Set value to a specific value
        /// </summary>
        /// <param name="pValue"></param>
        public void SetValue(float pValue)
        {
            CurrentValue = pValue;

            if (CurrentValue < MinValue)
                CurrentValue = MinValue;

            if (CurrentValue > MaxValue)
                CurrentValue = MaxValue;

            FireOnValueChanged();
        }

        /// <summary>
        /// Restore the original value
        /// </summary>
        public void RestoreOriginalValue()
        {
            CurrentValue = StartValue;

            FireOnValueChanged();
        }

        /// <summary>
        /// update the max value
        /// </summary>
        /// <param name="pNewMaxValue"></param>
        public void SetMaxValue(float pNewMaxValue)
        {
            MaxValue = pNewMaxValue;
        }

        /// <summary>
        /// Multiply the attribute by the given multiplicator
        /// </summary>
        /// <param name="pMultiplicator"></param>
        public void MultiplyBy(float pMultiplicator)
        {
            AddValue(CurrentValue * pMultiplicator);

            FireOnValueChanged();
        }

        /// <summary>
        /// Set the current value to the specfied percent of the max value
        /// </summary>
        /// <param name="pPercent"></param>
        public void SetToStartPercent(float pPercent)
        {
            CurrentValue = (StartValue * (pPercent / 100));

            FireOnValueChanged();
        }

        /// <summary>
        /// Set the value to the percent of max
        /// </summary>
        /// <param name="pPercent"></param>
        public void SetToMaxPercent(float pPercent)
        {
            CurrentValue = (pPercent * MaxValue) / 100;

            FireOnValueChanged();
        }


        ///// <summary>
        ///// Use this function to call the timer of the attribute and restore value, ...
        ///// </summary>
        //public void Tick(float pAdditionalTime)
        //{
        //    //If wait time is elapsed
        //    elapsedWaitTime += pAdditionalTime;        
        //    if(elapsedWaitTime > changeWaitTime)
        //    {
        //        //If frequency has been reached
        //        elapsedTime += pAdditionalTime;            
        //        if (elapsedTime > changeFrequency)
        //        {
        //            elapsedTime -= changeFrequency;

        //            if(elapsedTime < 0)
        //                elapsedTime = 0;

        //            //Add value per second multiply by the frequency
        //            if(changeFrequency < pAdditionalTime)                
        //                AddValue(pAdditionalTime * changeValuePerSecond);
        //            else
        //                AddValue(changeFrequency * changeValuePerSecond);

        //        }
        //    }
        //}



        #endregion

        #region Events trigger
        public void FireOnValueChanged()
        {
            OnValueChanged?.Invoke(this);
        }
        #endregion

    }
}


