using RTS.Configuration;
using System;
using UnityEngine;

namespace RTS.Models
{

    #region Delegates
    public delegate void PlayerBuildingPassiveModelEventHandler(PlayerBuildingPassiveModel pPlayerBuilding);
    #endregion

    [Serializable]
    public class PlayerBuildingPassiveModel : PlayerBuildingModel
    {
        #region Events
        public event PlayerBuildingPassiveModelEventHandler OnBuildingPassiveCycleBegan, OnBuildingPassiveCycleStarted, OnBuildingPassiveCycleFinished;
        public event PlayerBuildingPassiveModelEventHandler OnBuildingPassiveCycleAborted;
        #endregion

        #region Properties

        #region Building
        /// <summary>
        /// Model of the building
        /// </summary>
        public new BuildingPassiveModel Building
        {
            get { return GetBuilding() as BuildingPassiveModel; }
            set { SetBuilding(value); }
        }
        private BuildingPassiveModel buildingPassive;

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected override BuildingModel GetBuilding()
        {
            return buildingPassive;
        }

        protected override void SetBuilding(BuildingModel pBuilding)
        {
            buildingPassive = pBuilding as BuildingPassiveModel;
            FireOnBuildingUpdated();
        }

        #endregion

        #region Level

        /// <summary>
        /// List of availables levels for the building
        /// </summary>
        public new BuildingPassiveLevelModel Level
        {
            get { return GetLevel() as BuildingPassiveLevelModel; }
            set { SetLevel(value); }
        }

        /// <summary>
        /// List of levels
        /// </summary>
        [SerializeField]
        private BuildingPassiveLevelModel level;

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected override BuildingLevelModel GetLevel()
        {
            return level;
        }

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected override void SetLevel(BuildingLevelModel pLevel)
        {
            level = pLevel as BuildingPassiveLevelModel;
        }

        #endregion

        /// <summary>
        /// start consumption of resources needed to produce something
        /// </summary>
        public DateTime startConsumption;

        /// <summary>
        /// Last consumption of resources needed to produce something
        /// </summary>
        public DateTime lastConsumption;

        /// <summary>
        /// Determines if the building is producing or not producing
        /// </summary>
        public bool IsActivated
        {
            get
            {
                if (startConsumption > DateTime.MinValue && startConsumption >= lastConsumption && State.id == (int)StateOfBuilding.ACTIVE)
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region Static 

        /// <summary>
        /// Update a model from another
        /// </summary>
        /// <param name="pModelToUpdate"></param>
        /// <param name="pUpdateModel"></param>
        public static void UpdateFrom(ref PlayerBuildingPassiveModel pModelToUpdate, PlayerBuildingPassiveModel pUpdateModel)
        {
            //Common
            pModelToUpdate.mapExtentId = pUpdateModel.mapExtentId;
            pModelToUpdate.mapElementInstanceId = pUpdateModel.mapElementInstanceId;
            pModelToUpdate.creation = pUpdateModel.creation;
            pModelToUpdate.State = BuildingStateData.GetStateById(pUpdateModel.State.id);
            pModelToUpdate.Level = pModelToUpdate.Building.Levels[pUpdateModel.Level.id - 1];

            //Specific
            pModelToUpdate.startConsumption = pUpdateModel.startConsumption;
            pModelToUpdate.lastConsumption = pUpdateModel.lastConsumption;
        }

        #endregion

        #region Constructor

        public PlayerBuildingPassiveModel()
        { }

        public PlayerBuildingPassiveModel(BuildingPassiveModel pBuilding)
        {
            Building = pBuilding;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Initialize the player building
        /// </summary>
        /// <param name="pTimeCursor"></param>
        /// <param name="pPlayer"></param>
        public override void Init(DateTime pTimeCursor, PlayerModel pPlayer)
        {
            base.Init(pTimeCursor, pPlayer);

            //Set values
            StartConstruction(pTimeCursor);
        }

        /// <summary>
        /// Time when the building will do something. MaxValue when no action to be performed
        /// </summary>
        /// <returns></returns>
        public override DateTime GetNextActionTime()
        {
            switch (State.id)
            {
                case (int)StateOfBuilding.CONSTRUCTION:

                    if (creation == DateTime.MinValue)
                        throw new Exception("Creation datetime is used but not filled with a value");
                    return creation.AddSeconds(Building.constructionTime);

                case (int)StateOfBuilding.ACTIVE:
                    if (startConsumption == DateTime.MinValue)
                        throw new Exception("startConsumption datetime is used but not filled with a value");
                    return startConsumption.AddSeconds(Level.secondsToConsume);
                default:
                    return DateTime.MaxValue;
            }
        }

        /// <summary>
        /// When the building perform an action.
        /// </summary>
        /// <param name="pTimeCursor">Time to simulate</param>
        public override void Tick(DateTime pTimeCursor)
        {
            //If we have reached the next action time
            if (pTimeCursor < GetNextActionTime())
                return;

            if (State == null)
                return;

            switch (State.id)
            {
                //Action when CONSTRUCTION is finished
                case (int)StateOfBuilding.CONSTRUCTION:

                    //End construction
                    EndConstruction(pTimeCursor);
                    FireOnBuildingConstructed();

                    break;
                //Action when the building is ACTIVE          
                case (int)StateOfBuilding.ACTIVE:

                    //Fire event when passive cycle is finished
                    if (IsActivated && startConsumption.AddSeconds(Level.secondsToConsume) >= pTimeCursor)
                    {
                        EndPassiveCycle(pTimeCursor);
                    }

                    //Try to start the passive cycle
                    StartPassiveCycle(pTimeCursor);

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Try to start the production again
        /// </summary>
        /// <param name="pSimulation"></param>
        /// <param name="pPlayer"></param>
        public void EndPassiveCycle(DateTime pTimeCursor)
        {
            lastConsumption = pTimeCursor;
            FireOnBuildingPassiveCycleFinished();
        }

        /// <summary>
        /// Is the building can be activated
        /// </summary>
        public override bool CanActivate()
        {
            if (!base.CanActivate())
                return false;

            if (Player.resourceBag.HasEnoughResource(Level.consumptionBag))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Activate the building if it's not
        /// </summary>
        /// <param name="pTimeCursor"></param>
        /// <param name="pPlayer"></param>
        public override void Activate(DateTime pTimeCursor)
        {
            if (CanActivate())
            {
                Player.resourceBag.Use(Level.persistentBagNeeded);
                Player.resourceBag.AddBag(Level.persistentBag);

                State = BuildingStateData.Active;

                StartPassiveCycle(pTimeCursor);
            }
        }

        /// <summary>
        /// Try to start the production again
        /// </summary>
        /// <param name="pSimulation"></param>
        /// <param name="pPlayer"></param>
        public bool StartPassiveCycle(DateTime pTimeCursor)
        {
            //If the building is not active, it cannot produce
            if (State.id != (int)StateOfBuilding.ACTIVE)
                return false;

            //Fire event to teel to start the passive cycle
            FireOnBuildingPassiveCycleBegan();

            //If the production has a cost
            if (Level.consumptionBag != null)
            {
                //Check if the player has enough resource
                if (Player.resourceBag.HasEnoughResource(Level.consumptionBag))
                {
                    //Consume and start production
                    Player.resourceBag.Consume(Level.consumptionBag);
                    startConsumption = pTimeCursor;
                    FireOnBuildingPassiveCycleStarted();
                    return true;
                }
                else
                {
                    OutOfRawMaterial();
                    FireOnBuildingPassiveCycleAborted();
                    return false;
                }
            }
            else
            {

                startConsumption = pTimeCursor;
                FireOnBuildingPassiveCycleStarted();
                return true;
            }
        }

        /// <summary>
        /// Returns the provided bag
        /// </summary>
        /// <returns></returns>
        public ResourceBagModel GetActivePersistentBag()
        {
            switch (State.id)
            {
                case (int)StateOfBuilding.ACTIVE:
                    return Level.persistentBag;
                default:
                    return null;
            }
        }

        /// <summary>
        /// When the building is destroyed
        /// </summary>
        /// <param name="pPlayer"></param>
        public override void Destroy()
        {
            //Remove the persistent bag from the player
            Player.resourceBag.RemoveBag(Level.persistentBag);
            base.Destroy();
        }

        /// <summary>
        /// When the building is out of energy
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <returns></returns>
        public override void OutOfEnergy()
        {
            if (State == BuildingStateData.Active)
            {
                Player.resourceBag.Unuse(Level.persistentBagNeeded);
                Player.resourceBag.RemoveBag(Level.persistentBag);
                FireOnBuildingPassiveCycleAborted();
            }

            State = BuildingStateData.OutOfEnergy;
        }

        /// <summary>
        /// When a building is out of raw material
        /// </summary>
        /// <param name="pPlayer"></param>
        public override void OutOfRawMaterial()
        {
            if (State.id == (int)StateOfBuilding.ACTIVE)
            {
                Player.resourceBag.Unuse(Level.persistentBagNeeded);
                Player.resourceBag.RemoveBag(Level.persistentBag);
            }

            base.OutOfRawMaterial();
        }

        /// <summary>
        /// To be overriden. When state has changed
        /// </summary>
        /// <param name="pState"></param>
        public override void StartConstruction(DateTime pTimeCursor)
        {
            base.StartConstruction(pTimeCursor);
        }

        /// <summary>
        /// To be overriden. When state has changed
        /// </summary>
        /// <param name="pState"></param>
        public override void EndConstruction(DateTime pTimeCursor)
        {
            base.EndConstruction(pTimeCursor);

            if (State.id != (int)StateOfBuilding.OUT_OF_ENERGY)
            {
                State = BuildingStateData.Active;

                //Add persitent resource bag
                Player.resourceBag.AddBag(Level.persistentBag);

                //Start passive cycle
                StartPassiveCycle(pTimeCursor);
            }
        }

        /// <summary>
        /// Increment a player building level
        /// </summary>
        /// <returns></returns>
        public override bool IncrementLevel(DateTime pTimeCursor)
        {
            if (CanLevelUp())
            {
                //Substract current bag
                Player.resourceBag.RemoveBag(Level.persistentBag);
                //Add the new bag
                Player.resourceBag.AddBag(Building.Levels[Level.id].persistentBag);

                base.IncrementLevel(pTimeCursor);

                StartPassiveCycle(pTimeCursor);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Set player building level at the specified id
        /// </summary>
        /// <returns></returns>
        public override bool SetLevel(int pId)
        {
            return base.SetLevel(pId);
        }

        #endregion

        #region Fire events

        public virtual void FireOnBuildingPassiveCycleFinished()
        {
            OnBuildingPassiveCycleFinished?.Invoke(this);
        }

        public virtual void FireOnBuildingPassiveCycleStarted()
        {
            OnBuildingPassiveCycleStarted?.Invoke(this);
        }

        public virtual void FireOnBuildingPassiveCycleBegan()
        {
            OnBuildingPassiveCycleBegan?.Invoke(this);
        }

        public virtual void FireOnBuildingPassiveCycleAborted()
        {
            OnBuildingPassiveCycleAborted?.Invoke(this);
        }

        #endregion
    }
}
