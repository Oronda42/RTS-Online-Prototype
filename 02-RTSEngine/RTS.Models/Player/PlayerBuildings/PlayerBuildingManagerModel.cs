using RTS.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RTS.Models
{
    #region Delegates
    public delegate void PlayerBuildingManagerModelEventHandler(PlayerBuildingManagerModel pPlayerBuildingManager, PlayerBuildingModel pBuilding);
    #endregion

    [Serializable]
    public class PlayerBuildingManagerModel
    {

        #region Events

        public event PlayerBuildingManagerModelEventHandler OnBuildingCreated, OnBuildingCreationAborted, OnBuildingDestroyed;

        #endregion

        #region Properties

        /// <summary>
        /// Reference to the player
        /// </summary>
        public PlayerModel player;

        /// <summary>
        /// Buildings on the player map
        /// </summary>
        [SerializeField]
        private List<PlayerBuildingModel> playerBuildings;

        #endregion

        #region Constructor

        public PlayerBuildingManagerModel(PlayerModel pPlayer, List<PlayerBuildingModel> pPlayerBuildings)
        {
            player = pPlayer;
            playerBuildings = pPlayerBuildings;
        }

        public PlayerBuildingManagerModel(PlayerModel pPlayer)
        {
            player = pPlayer;
            playerBuildings = new List<PlayerBuildingModel>();
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Return the next building number
        /// </summary>
        /// <returns></returns>
        public int GetNextPlayerBuildingNumber()
        {
            if (playerBuildings.Count() == 0)
                return 1;

            return playerBuildings.Max(pb => pb.buildingNumber) + 1;
        }

        /// <summary>
        /// Return the player building with the specified number
        /// </summary>
        /// <param name="pNumber"></param>
        /// <returns></returns>
        public PlayerBuildingModel GetPlayerBuildingByNumber(int pNumber)
        {
            return playerBuildings.Where(x => x.buildingNumber == pNumber).FirstOrDefault();
        }

        ///// <summary>
        ///// Return the player building with the specified position
        ///// </summary>
        ///// <param name="pPosition"></param>
        ///// <returns></returns>
        //public PlayerBuildingModel GetPlayerBuildingByPosition(string pPosition)
        //{
        //    return playerBuildings.Where(x => x.positionOnMap == pPosition).FirstOrDefault();
        //}

        // <summary>
        /// Return the player building with the specified position
        /// </summary>
        /// <param name="pPosition"></param>
        /// <returns></returns>
        public PlayerBuildingModel GetPlayerBuildingByElementInstanceId(int pMapExtentId, int pMapElementInstanceId)
        {
            return playerBuildings.Where(x => x.mapExtentId == pMapExtentId && x.mapElementInstanceId == pMapElementInstanceId).FirstOrDefault();
        }

        /// <summary>
        /// Destroy a building
        /// </summary>
        /// <param name="pBuildingNumber"></param>
        /// <returns></returns>
        public bool RemoveByNumber(int pBuildingNumber)
        {
            PlayerBuildingModel playerBuilding = playerBuildings.Where(pb => pb.buildingNumber == pBuildingNumber).FirstOrDefault();
            if (!Equals(playerBuilding, null))
            {
                UnsuscribeToPlayerBuildingEvents(playerBuilding);

                //Remove the building from the list
                playerBuildings.Remove(playerBuilding);

                //Regulate energy
                if (!CheckEnergySufficiency())
                    RegulateEnergy();

                //Fire event
                OnBuildingDestroyed?.Invoke(this, playerBuilding);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Destroy a building
        /// </summary>
        /// <param name="pBuildingNumber"></param>
        /// <returns></returns>
        public bool RemoveByElementInstanceId(int pMapExtentId, int pMapElementInstanceId)
        {
            PlayerBuildingModel playerBuilding = GetPlayerBuildingByElementInstanceId(pMapExtentId, pMapElementInstanceId);
            if (!Equals(playerBuilding, null))
            {
                UnsuscribeToPlayerBuildingEvents(playerBuilding);

                //Remove the building from the list
                playerBuildings.Remove(playerBuilding);

                //Regulate energy
                if (!CheckEnergySufficiency())
                    RegulateEnergy();

                //Fire event
                OnBuildingDestroyed?.Invoke(this, playerBuilding);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the persistent bag provided by all buildings of the player
        /// </summary>
        /// <returns></returns>
        public ResourceBagModel GetProvidedPersistentBag()
        {
            ResourceBagModel persistentBag = new ResourceBagModel
            {
                id = -1,
                resources = new List<ResourceBagSlotModel>()
            };

            for (int i = 0; i < playerBuildings.Count; i++)
            {
                if (playerBuildings[i].GetType().IsAssignableFrom(typeof(PlayerBuildingPassiveModel)))
                {
                    persistentBag.AddBag(((PlayerBuildingPassiveModel)playerBuildings[i]).GetActivePersistentBag());
                }
            }

            return persistentBag;
        }

        /// <summary>
        /// Create a player building
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        /// <returns></returns>
        public bool Create(PlayerBuildingModel pPlayerBuilding, DateTime pTimeCursor)
        {
            if (player.resourceBag.Consume(pPlayerBuilding.Building.cost))
            {
                //Add the player building
                Add(pPlayerBuilding);

                pPlayerBuilding.StartConstruction(pTimeCursor);

                //Fire event
                OnBuildingCreated?.Invoke(this, pPlayerBuilding);

                return true;
            }
            else
            {
                OnBuildingCreationAborted?.Invoke(this, pPlayerBuilding);
                return false;
            }
        }

        /// <summary>
        /// Add a building to the list
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        /// <returns></returns>
        public bool Add(PlayerBuildingModel pPlayerBuilding)
        {
            //Add the player building
            playerBuildings.Add(pPlayerBuilding);

            //if(pPlayerBuilding.State.id == (int)StateOfBuilding.ACTIVE)
            //{
            //    player.resourceBag.Use(pPlayerBuilding.Level.persistentBagNeeded);
            //}

            SuscribeToPlayerBuildingEvents(pPlayerBuilding); 

            return true;
        }

        /// <summary>
        /// Suscribe to buildings events
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        private void SuscribeToPlayerBuildingEvents(PlayerBuildingModel pPlayerBuilding)
        {
            //Suscribe to events
            pPlayerBuilding.OnBuildingConstructed += OnPlayerBuildingConstructed;
            pPlayerBuilding.OnOutOfEnergy += OnPlayerBuildingOutOfEnergy;
            pPlayerBuilding.OnOutOfRawMaterial += OnPlayerBuildingOutOfRawMaterial;
            pPlayerBuilding.OnActivated += OnPlayerBuildingActivated;
            pPlayerBuilding.OnPaused += OnPlayerBuildingPaused;
        }

        /// <summary>
        /// Unsuscribe from buildings events
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        private void UnsuscribeToPlayerBuildingEvents(PlayerBuildingModel pPlayerBuilding)
        {
            //Suscribe to events
            pPlayerBuilding.OnBuildingConstructed -= OnPlayerBuildingConstructed;
            pPlayerBuilding.OnOutOfEnergy -= OnPlayerBuildingOutOfEnergy;
            pPlayerBuilding.OnOutOfRawMaterial -= OnPlayerBuildingOutOfRawMaterial;
            pPlayerBuilding.OnOutOfRawMaterial -= OnPlayerBuildingActivated;
        }

        /// <summary>
        /// Check if the player has enough energy
        /// </summary>
        /// <returns></returns>
        public bool CheckEnergySufficiency()
        {
            if (player.resourceBag.GetAmountByResourceId(ResourceData.Energy.id) >= player.resourceBag.GetUsageByResourceId(ResourceData.Energy.id))
                return true; 
            else
                return false;
        }

        /// <summary>
        /// Turn off building until energy is under the max limit for the player
        /// </summary>
        public void RegulateEnergy()
        {
            PlayerBuildingModel[] playerBuildinsgModel = playerBuildings.OrderByDescending(pb => pb.buildingNumber).ToArray();

            for (int i = 0; i < playerBuildinsgModel.Length; i++)
            {
                if (!CheckEnergySufficiency())
                {
                    if (playerBuildinsgModel[i].Level.persistentBagNeeded != null)
                    {
                        playerBuildinsgModel[i].OutOfEnergy();
                    }
                }
            } 
           
        }
        #endregion

        #region Building events

        private void OnPlayerBuildingConstructed(PlayerBuildingModel pPlayerBuilding)
        {
            RegulateEnergy();
        }

        private void OnPlayerBuildingOutOfEnergy(PlayerBuildingModel pPlayerBuilding)
        {
            RegulateEnergy();
        }

        private void OnPlayerBuildingOutOfRawMaterial(PlayerBuildingModel pPlayerBuilding)
        {
            RegulateEnergy();
        }

        private void OnPlayerBuildingActivated(PlayerBuildingModel pPlayerBuilding)
        {
            RegulateEnergy();
        }

        private void OnPlayerBuildingPaused(PlayerBuildingModel pPlayerBuilding)
        {
            RegulateEnergy();
        }


        #endregion
    }
}