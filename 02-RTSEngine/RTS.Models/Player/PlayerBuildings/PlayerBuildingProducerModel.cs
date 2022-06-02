using RTS.Configuration;
using System;
using UnityEngine;

namespace RTS.Models
{
    #region Delegates
    public delegate void PlayerBuildingProductionModelEventHandler(PlayerBuildingProducerModel pPlayerBuilding);
    #endregion

    [Serializable]
    public class PlayerBuildingProducerModel : PlayerBuildingModel
    {
        #region Events
        public event PlayerBuildingProductionModelEventHandler OnBuildingProductionCycleBegan, OnBuildingProductionCycleStarted, OnBuildingProductionCycleFinished,
        OnBuildingProductionCycleAborted;
        #endregion

        #region Properties

        #region Building
        /// <summary>
        /// Model of the building
        /// </summary>
        public new BuildingProducerModel Building
        {
            get { return GetBuilding() as BuildingProducerModel; }
            set { SetBuilding(value); }
        }
        private BuildingProducerModel buildingProducer;

        /// <summary>
        /// Useful to use metamorphism and thus, get the child model when call Building properties
        /// </summary>
        /// <returns></returns>
        protected override BuildingModel GetBuilding()
        {
            return buildingProducer;
        }

        protected override void SetBuilding(BuildingModel pBuilding)
        {
            buildingProducer = pBuilding as BuildingProducerModel;
            FireOnBuildingUpdated();
        }

        #endregion

        #region Level

        /// <summary>
        /// List of availables levels for the building
        /// </summary>
        public new BuildingProducerLevelModel Level
        {
            get { return GetLevel() as BuildingProducerLevelModel; }
            set { SetLevel(value); }
        }

        /// <summary>
        /// List of levels
        /// </summary>
        [SerializeField]
        private BuildingProducerLevelModel level;

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
            level = pLevel as BuildingProducerLevelModel;
        }

        #endregion

        /// <summary>
        /// Resource produced by the building
        /// </summary>
        public int currentResourceIdProduced;

        /// <summary>
        /// Is the building auto produce resource
        /// </summary>
        public bool autoProduce;

        /// <summary>
        /// Start of production cycle
        /// </summary>
        public DateTime startProduction;

        /// <summary>
        /// End of the last production cycle
        /// </summary>
        public DateTime lastProduction;

        /// <summary>
        /// Determines if the building is producing or not producing
        /// </summary>
        public bool IsProducing
        {
            get
            {
                if (startProduction > DateTime.MinValue && startProduction >= lastProduction && State.id == (int)StateOfBuilding.ACTIVE)
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
        public static void UpdateFrom(ref PlayerBuildingProducerModel pModelToUpdate, PlayerBuildingProducerModel pUpdateModel)
        {
            //Common
            pModelToUpdate.mapExtentId = pUpdateModel.mapExtentId;
            pModelToUpdate.mapElementInstanceId = pUpdateModel.mapElementInstanceId;
            pModelToUpdate.creation = pUpdateModel.creation;
            pModelToUpdate.State = BuildingStateData.GetStateById(pUpdateModel.State.id);
            pModelToUpdate.Level = pModelToUpdate.Building.Levels[pUpdateModel.Level.id - 1];

            //Specific
            pModelToUpdate.currentResourceIdProduced = pUpdateModel.currentResourceIdProduced;
            pModelToUpdate.autoProduce = pUpdateModel.autoProduce;
            pModelToUpdate.startProduction = pUpdateModel.startProduction;
            pModelToUpdate.lastProduction = pUpdateModel.lastProduction;
        }

        #endregion

        #region Constructor

        public PlayerBuildingProducerModel()
        { }

        public PlayerBuildingProducerModel(BuildingProducerModel pBuilding)
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
            currentResourceIdProduced = Building.defaultResourceIdProduced;
            autoProduce = true;

            StartConstruction(pTimeCursor);
        }

        /// <summary>
        /// To be overriden. Time when the building will do something. MaxValue when no action to be performed
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
                    if (startProduction == DateTime.MinValue)
                        throw new Exception("startProduction datetime is used but not filled with a value");
                    return startProduction.AddSeconds(Level.secondsToProduce);

                default:
                    return DateTime.MaxValue;
            }
        }

        /// <summary>
        /// To be overriden. When the building perform an action.
        /// </summary>
        /// <param name="pTimeCursor">Time to simulate</param>
        public override void Tick(DateTime pTimeCursor)
        {
            //If we have reached the next action time
            if (!(pTimeCursor >= GetNextActionTime()))
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
                    if (IsProducing && startProduction.AddSeconds(Level.secondsToProduce) >= pTimeCursor)
                        EndProductionCycle(pTimeCursor);

                    //Try to start the production cycle
                    StartProductionCycle(pTimeCursor);

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// When a production cycle is finished
        /// </summary>
        /// <param name="pTimeCursor"></param>
        /// <param name="pPlayer"></param>
        public void EndProductionCycle(DateTime pTimeCursor)
        {
            //Add resources to the player bag and update the model
         
            Player.resourceBag.Add(Level.resourceIdProduced, Level.amountProduced);
            lastProduction = pTimeCursor;

            FireOnBuildingProductionCycleFinished();
        }

        /// <summary>
        /// Try to start the production again
        /// </summary>
        /// <param name="pSimulation"></param>
        /// <param name="pPlayer"></param>
        public void StartProductionCycle(DateTime pTimeCursor)
        {
            //If the building is not active, it cannot produce
            if (State.id != (int)StateOfBuilding.ACTIVE)
                return;

            FireOnBuildingProductionCycleBegan();

            //Check if the player has enough resource
            if (Player.resourceBag.HasEnoughResource(Level.consumptionBag))
            {
                //Consume and start production
                Player.resourceBag.Consume(Level.consumptionBag);
                startProduction = pTimeCursor;
                FireOnBuildingProductionCycleStarted();
            }
            else
            {
                OutOfRawMaterial();
                FireOnBuildingProductionCycleAborted();
            }
        }

        /// <summary>
        /// To be overriden. When the building is destroyed
        /// </summary>
        /// <param name="pPlayer"></param>
        public override void Destroy()
        {
            base.Destroy();
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
                State = BuildingStateData.Active;

                StartProductionCycle(pTimeCursor);


                //FireOnBuildingProductionCycleBegan();

                ////Check if the player has enough resource
                //if (Player.resourceBag.HasEnoughResource(Level.consumptionBag))
                //{
                //    //Consume and start production
                //    Player.resourceBag.Consume(Level.consumptionBag);
                //    startProduction = pTimeCursor;
                //    FireOnBuildingProductionCycleStarted();
                //}
                //else
                //{
                //    OutOfRawMaterial();
                //    FireOnBuildingProductionCycleAborted();
                //}

            }
        }

        /// <summary>
        /// Is the building can be paused
        /// </summary>
        public override bool CanPause()
        {
            if (!base.CanPause())
                return false;
            else return true;
        }

        /// <summary>
        /// Pause the building if it's not
        /// </summary>
        /// <param name="pTimeCursor"></param>
        /// <param name="pPlayer"></param>
        public override void Pause(DateTime pTimeCursor)
        {
            if (CanPause())
            {
                //Player.resourceBag.Unuse(Level.persistentBagNeeded);
                StopProduction(pTimeCursor);
                State = BuildingStateData.Paused;
            }
        }

        /// <summary>
        /// Stop The production
        /// </summary>
        /// <param name="pTimeCursor"></param>
        private void StopProduction(DateTime pTimeCursor)
        {
           

            if (State.id != (int)StateOfBuilding.ACTIVE)
                return;

            Player.resourceBag.Unuse(Level.persistentBagNeeded);

            if (IsProducing)
                FireOnBuildingProductionCycleAborted();
            
               
        }

        /// <summary>
        /// To be overriden. When the building is out of energy
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <returns></returns>
        public override void OutOfEnergy()
        {
            if (State.id == (int)StateOfBuilding.ACTIVE)
            {
                Player.resourceBag.Unuse(Level.persistentBagNeeded);

                if (IsProducing)
                    FireOnBuildingProductionCycleAborted();
            }

            base.OutOfEnergy();
        }

        /// <summary>
        /// To be overriden. When the building is starting to construct
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
                //Start production cycle 
                State = BuildingStateData.Active;
                StartProductionCycle(pTimeCursor);
            }
        }

        /// <summary>
        /// Increment a player building level
        /// </summary>
        /// <returns></returns>
        public override bool IncrementLevel(DateTime pTimeCursor)
        {
            return base.IncrementLevel(pTimeCursor);
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

        public virtual void FireOnBuildingProductionCycleFinished()
        {
            OnBuildingProductionCycleFinished?.Invoke(this);
        }

        public virtual void FireOnBuildingProductionCycleStarted()
        {
            OnBuildingProductionCycleStarted?.Invoke(this);
        }

        public virtual void FireOnBuildingProductionCycleBegan()
        {
            OnBuildingProductionCycleBegan?.Invoke(this);
        }

        public virtual void FireOnBuildingProductionCycleAborted()
        {
            OnBuildingProductionCycleAborted?.Invoke(this);
        }

        #endregion
    }
}
