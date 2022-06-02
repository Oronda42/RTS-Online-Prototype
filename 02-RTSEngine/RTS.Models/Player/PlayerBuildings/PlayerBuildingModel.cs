using RTS.Configuration;
using System;
using UnityEngine;

namespace RTS.Models
{
    #region Delegates
    public delegate void PlayerBuildingModelEventHandler(PlayerBuildingModel pPlayerBuilding);
    #endregion

    [Serializable]
    public class PlayerBuildingModel
    {

        #region Events
        public event PlayerBuildingModelEventHandler OnBuildingModelUpdated, OnBuildingConstructionStarted, OnBuildingConstructionEnded, OnBuildingConstructed, OnStateChanged;
        public event PlayerBuildingModelEventHandler OnLevelUpdated, OnBuildingInitialized, OnBuildingDestroyed, OnOutOfEnergy, OnOutOfRawMaterial, OnActivated, OnPaused;
        #endregion

        #region Properties

        /// <summary>
        /// Player that owns the building
        /// </summary>
        public PlayerModel Player { get; set; }

        #region Building property

        /// <summary>
        /// Model of the building
        /// </summary>
        public BuildingModel Building
        {
            get { return GetBuilding(); }
            set { SetBuilding(value); }
        }

        [NonSerialized]
        private BuildingModel building;

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected virtual BuildingModel GetBuilding()
        {
            return building;
        }

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected virtual void SetBuilding(BuildingModel pBuilding)
        {
            building = pBuilding;
        }

        #endregion

        /// <summary>
        /// Building number
        /// </summary>
        public int buildingNumber;

        /// <summary>
        /// Id of the extent where the building is
        /// </summary>
        public int mapExtentId;

        /// <summary>
        /// Element instance id where the building is built
        /// </summary>
        public int mapElementInstanceId;

        /// <summary>
        /// Creation of the building
        /// </summary>
        public DateTime creation;

        /// <summary>
        /// State of the building
        /// </summary>
        public BuildingStateModel State
        {
            get { return state; }
            set
            {
                state = value;
                OnStateChanged?.Invoke(this);
            }
        }
        [SerializeField]
        private BuildingStateModel state;

        #region Level

        /// <summary>
        /// List of availables levels for the building
        /// </summary>
        public BuildingLevelModel Level
        {
            get { return GetLevel(); }
            set { SetLevel(value); }
        }

        /// <summary>
        /// List of levels
        /// </summary>
        [NonSerialized]
        private BuildingLevelModel level;

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected virtual BuildingLevelModel GetLevel()
        {
            return level;
        }

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected virtual void SetLevel(BuildingLevelModel pLevel)
        {
            level = pLevel;
        }

        #endregion

        #endregion

        #region static

        #endregion

        #region Implementation

        /// <summary>
        /// To be overriden in children
        /// </summary>
        public virtual void Init(DateTime pTimeCursor, PlayerModel pPlayer)
        {
            Player = pPlayer;
            Level = Building.Levels[0];
            OnBuildingInitialized?.Invoke(this);
        }

        /// <summary>
        /// Returns a timespan that indicates the time left before the next action
        /// </summary>
        /// <param name="pTimeCursor"></param>
        /// <returns></returns>
        public virtual TimeSpan GetTimeBeforeNextAction(DateTime pTimeCursor)
        {
            return (GetNextActionTime() - pTimeCursor);
        }

        /// <summary>
        /// To be overriden. Time when the building will do something
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetNextActionTime()
        {
            return DateTime.MaxValue;
        }

        /// <summary>
        /// If a player building can level up
        /// </summary>
        /// <returns></returns>
        public virtual bool CanLevelUp()
        {
            if (Level.id < Building.Levels.Count
                && Player.resourceBag.HasEnoughResource(Building.Levels[Level.id].cost))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Increment a player building level
        /// </summary>
        /// <returns></returns>
        public virtual bool IncrementLevel(DateTime pTimeCursor)
        {
            if (CanLevelUp())
            {
                Level = Building.Levels[Level.id];
                Player.resourceBag.Consume(Level.cost);

                OnLevelUpdated?.Invoke(this);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set player building level at the specified id
        /// </summary>
        /// <returns></returns>
        public virtual bool SetLevel(int pId)
        {
            if (pId <= Building.Levels.Count)
            {
                Level = Building.Levels[pId - 1];
                OnLevelUpdated?.Invoke(this);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// To be overriden. When the building perform an action
        /// </summary>
        /// <param name="pTimeCursor"></param>
        public virtual void Tick(DateTime pTimeCursor)
        {
            //If we have reached the next action time
            if (!(pTimeCursor >= GetNextActionTime()))
                return;
        }

        /// <summary>
        /// Is the building can be activated
        /// </summary>
        public virtual bool CanActivate()
        {
            if (State.id != (int)StateOfBuilding.ACTIVE
                && Player.resourceBag.HasEnoughResource(Level.persistentBagNeeded))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Activate the building if it's not
        /// </summary>
        /// <param name="pTimeCursor"></param>
        /// <param name="pPlayer"></param>
        public virtual void Activate(DateTime pTimeCursor)
        {
            State = BuildingStateData.Active;
            FireOnActivated();
        }

        /// <summary>
        /// Is the building can be paused
        /// </summary>
        public virtual bool CanPause()
        {
            if (State.id != (int)StateOfBuilding.PAUSED)
                return true;
            else
                return false;
        }

        /// <summary>
        /// STop the building if it's not
        /// </summary>
        /// <param name="pTimeCursor"></param>
        /// <param name="pPlayer"></param>
        public virtual void Pause(DateTime pTimeCursor)
        {
            State = BuildingStateData.Paused;
            FireOnPaused();
        }

        /// <summary>
        /// To be overriden. When the building is destroyed
        /// </summary>
        /// <param name="pPlayer"></param>
        public virtual void Destroy()
        {
            if(State.id == (int)StateOfBuilding.ACTIVE)
            {
                Player.resourceBag.Unuse(Level.persistentBagNeeded);
            }
            
            OnBuildingDestroyed?.Invoke(this);
        }

        /// <summary>
        /// To be overriden. When the building is out of energy
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <returns></returns>
        public virtual void OutOfEnergy()
        {
            State = BuildingStateData.OutOfEnergy;
            FireOnOutOfEnergy();

        }

        /// <summary>
        /// To be overriden. When the building is out of energy
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <returns></returns>
        public virtual void OutOfRawMaterial()
        {
            State = BuildingStateData.OutOfRawMaterial;
            Player.resourceBag.Unuse(Level.persistentBagNeeded);
            FireOnOutOfRawMaterial();
        }

        /// <summary>
        /// To be overriden. When the construction has started
        /// </summary>
        /// <param name="pState"></param>
        public virtual void StartConstruction(DateTime pTimeCursor)
        {
            creation = pTimeCursor;
            State = BuildingStateData.Construction;
            OnBuildingConstructionStarted?.Invoke(this);
        }
         
        /// <summary>
        /// To be overriden. When state has changed
        /// </summary>
        /// <param name="pState"></param>
        public virtual void EndConstruction(DateTime pTimeCursor)
        {
            //Use persistent resources
            if (!Player.resourceBag.Use(Level.persistentBagNeeded))
            {
                OutOfEnergy();
            }

            //Fire event
            OnBuildingConstructionEnded?.Invoke(this);
        }

        #endregion

        #region Fire events

        protected void FireOnBuildingConstructed()
        {
            OnBuildingConstructed?.Invoke(this);
        }

        protected void FireOnBuildingUpdated()
        {
            OnBuildingModelUpdated?.Invoke(this);
        }

        protected void FireOnOutOfRawMaterial()
        {
            OnOutOfRawMaterial?.Invoke(this);
        }

        protected void FireOnOutOfEnergy()
        {
            OnOutOfEnergy?.Invoke(this);
        }

        protected void FireOnActivated()
        {
            OnActivated?.Invoke(this);
        }

        protected void FireOnPaused()
        {
            OnPaused?.Invoke(this);
        }


        #endregion
    }
}
