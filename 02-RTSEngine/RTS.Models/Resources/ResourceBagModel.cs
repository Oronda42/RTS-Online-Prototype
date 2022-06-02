using RTS.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;


namespace RTS.Models
{
    #region Delegates
    public delegate void ResourceEvent(ResourceBagSlotModel pSlot);
    #endregion

    [Serializable]
    public class ResourceBagModel
    {
        #region Events
        public event ResourceEvent OnResourceChanged;
        #endregion

        #region Properties

        /// <summary>
        /// Id of the bag
        /// </summary>
        public int id;

        /// <summary>
        /// List of availables resources for the player
        /// </summary>
        public List<ResourceBagSlotModel> resources;

        #endregion

        #region Constructor

        public ResourceBagModel()
        {
            id = -1;
            resources = new List<ResourceBagSlotModel>();
        }

        public ResourceBagModel(int pId, List<ResourceBagSlotModel> pResources)
        {
            id = pId;
            resources = pResources;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Returns a bag with missing resource from pBagToCompare compared to pBag 
        /// </summary>
        /// <param name="pBag"></param>
        /// <param name="pBagToCompare"></param>
        /// <returns></returns>
        public static ResourceBagModel GetMissingResourceBag(ResourceBagModel pBag, ResourceBagModel pBagToCompare)
        {
            ResourceBagModel bagToReturn = new ResourceBagModel
            {
                id = -1,
                resources = new List<ResourceBagSlotModel>()
            };

            for (int i = 0; i < pBagToCompare.resources.Count; i++)
            {
                //If amount to compare above amount of the bag
                if (pBagToCompare.resources[i].amount > pBag.GetAmountByResourceId(pBagToCompare.resources[i].resourceId))
                {
                    bagToReturn.resources.Add(pBagToCompare.resources[i]);
                }
            }

            return bagToReturn;
        }

        /// <summary>
        /// Returns the amount of the resource
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public int GetUsageByResourceId(int pResourceId)
        {
            ResourceBagSlotModel slot = resources.Where(s => s.resourceId == pResourceId).FirstOrDefault();
            return slot.used;
        }

        /// <summary>
        /// Returns the amount of the resource
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public int GetAmountByResourceId(int pResourceId)
        {
            ResourceBagSlotModel slot = resources.Where(s => s.resourceId == pResourceId).FirstOrDefault();
            return slot.amount;
        }

        /// <summary>
        /// Returns the available amount for a resource
        /// </summary>
        /// <param name="pResourceId"></param>
        /// <returns></returns>
        public int GetAvailableAmountByResourceId(int pResourceId)
        {
            //Get the resource
            ResourceModel resourceDefinition = ResourceData.GetResourceById(pResourceId);

            if (resourceDefinition == null)
                throw new Exception("The resource doesn't exists");

            switch (resourceDefinition.type.id)
            {
                case (int)TypeOfResource.PERSISTENT:
                    ResourceBagSlotModel resourceBagSlot = GetSlotByResourceId(pResourceId);
                    return resourceBagSlot.amount - resourceBagSlot.used;

                default:
                    return GetAmountByResourceId(pResourceId);
            }
        }

        /// <summary>
        /// Return the resource bag slot
        /// </summary>
        /// <param name="pResourceId"></param>
        /// <returns></returns>
        public ResourceBagSlotModel GetSlotByResourceId(int pResourceId)
        {
            ResourceBagSlotModel slot = resources.Where(s => s.resourceId == pResourceId).FirstOrDefault();
            return slot;
        }

        /// <summary>
        /// add a ressource
        /// </summary>
        /// <param name="pRessource"></param>
        /// <param name="pAmmount"></param>
        public void Add(int pResourceId, int pAmount)
        {
            if (resources != null)
            {
                ResourceBagSlotModel slot = GetSlotByResourceId(pResourceId);

                if (slot != null)
                {
                    slot.amount += pAmount;

                    //If the amount is not above the maximum and if the maximum is defined
                    if (slot.amount > slot.maximum && slot.maximum > 0)
                        slot.amount = slot.maximum;

                    if (slot.amount <= 0)
                        slot.amount = 0;

                    FireOnResourceChanged(slot);
                }
            }
        }

        /// <summary>
        /// add a ressource
        /// </summary>
        /// <param name="pRessource"></param>
        /// <param name="pAmmount"></param>
        public void CreateOrUpdateSlot(ResourceBagSlotModel pResourceSlot)
        {
            if (resources != null)
            {
                ResourceBagSlotModel slot = GetSlotByResourceId(pResourceSlot.resourceId);

                if (slot != null)
                {
                    resources.Remove(slot);
                    resources.Add(pResourceSlot);
                    FireOnResourceChanged(slot);
                }
                else
                {
                    resources.Add(pResourceSlot);
                    FireOnResourceChanged(pResourceSlot);
                }
            }
        }

        /// <summary>
        /// Add an entire bag into this bag
        /// </summary>
        /// <param name="pBag"></param>
        public void AddBag(ResourceBagModel pBag)
        {
            if (pBag == null)
                return;

            for (int i = 0; i < pBag.resources.Count; i++)
            {
                CreateOrUpdateSlot(pBag.resources[i]);
            }            
        }

        /// <summary>
        /// Add an entire bag into this bag
        /// </summary>
        /// <param name="pBag"></param>
        public void RemoveBag(ResourceBagModel pBag)
        {
            for (int i = 0; i < pBag.resources.Count; i++)
                Add(pBag.resources[i].resourceId, -pBag.resources[i].amount);
        }

        /// <summary>
        /// Create or update with a specified bag
        /// </summary>
        /// <param name="pBag"></param>
        public void CreateOrUpdate(ResourceBagModel pBag)
        {
            for (int i = 0; i < pBag.resources.Count; i++)
                CreateOrUpdateSlot(pBag.resources[i]);
        }

        /// <summary>
        /// Check if player have enough Resource
        /// </summary>
        /// <param name="pResourceId"></param>
        /// <param name="pCost"></param>
        /// <returns></returns>
        public bool HasEnoughResource(int pResourceId, int pCost)
        {
            if (GetAvailableAmountByResourceId(pResourceId) >= pCost)
                return true;
            else return false;
        }

        /// <summary>
        /// Check if player have enough Resource
        /// </summary>
        /// <param name="pResourceId"></param>
        /// <param name="pCost"></param>
        /// <returns></returns>
        public bool HasEnoughResource(ResourceBagModel pCost)
        {
            if (pCost == null)
                return true;

            for (int i = 0; i < pCost.resources.Count; i++)
            {
                if (!HasEnoughResource(pCost.resources[i].resourceId, pCost.resources[i].amount))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Consume indacted resources, return the missing Resource if not enough resource
        /// </summary>
        /// <param name="pBagToConsume"></param>
        /// <returns></returns>
        public bool Consume(ResourceBagModel pBagToConsume)
        {
            if (pBagToConsume == null)
                return true;

            if (!HasEnoughResource(pBagToConsume))
                return false;

            for (int i = 0; i < pBagToConsume.resources.Count; i++)
                Add(pBagToConsume.resources[i].resourceId, -pBagToConsume.resources[i].amount);

            return true;
        }

        /// <summary>
        /// Use a peristent resource
        /// </summary>
        /// <param name="pBagToUse"></param>
        /// <returns></returns>
        public bool Unuse(ResourceBagModel pBagToUnuse)
        {
            for (int i = 0; i < pBagToUnuse.resources.Count; i++)
            {
                Use(pBagToUnuse.resources[i].resourceId, -pBagToUnuse.resources[i].amount);
            }

            return true;
        }

        /// <summary>
        /// Use a peristent resource
        /// </summary>
        /// <param name="pBagToUse"></param>
        /// <returns></returns>
        public bool Use(ResourceBagModel pBagToUse)
        {
            if (pBagToUse == null)
                return true;

            if (!HasEnoughResource(pBagToUse))
                return false;

            for (int i = 0; i < pBagToUse.resources.Count; i++)
            {
                Use(pBagToUse.resources[i].resourceId, pBagToUse.resources[i].amount);
            }

            return true;
        }

        /// <summary>
        /// Use a persistent resource
        /// </summary>
        /// <param name="pRessourceId"></param>
        /// <param name="pAmount"></param>
        public bool Use(int pRessourceId, int pAmount)
        {
            if (resources != null)
            {
                ResourceBagSlotModel slot = GetSlotByResourceId(pRessourceId);

                if (slot != null)
                {
                    if (slot.used + pAmount > slot.amount)
                        return false;
                    else
                    {
                        slot.used += pAmount;

                        if (slot.used < 0)
                            slot.used = 0;

                        FireOnResourceChanged(slot);
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Reset a resource slot
        /// </summary>
        /// <param name="pRessourceId"></param>
        public void Reset(int pRessourceId)
        {
            ResourceBagSlotModel resourceSlot = GetSlotByResourceId(pRessourceId);
            if (resourceSlot != null)
            {
                resourceSlot.amount = 0;
                resourceSlot.used = 0;
            }
        }

        /// <summary>
        /// Reset all resource of a given type
        /// </summary>
        /// <param name="pResourceTypeId"></param>
        public void ResetAllResourceByTypeId(int pResourceTypeId)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if (ResourceData.GetResourceById(resources[i].resourceId).type.id == pResourceTypeId)
                    Reset(resources[i].resourceId);
            }
        }


        #endregion

        #region FireEvent

        private void FireOnResourceChanged(ResourceBagSlotModel pSlot)
        { 
            OnResourceChanged?.Invoke(pSlot);
        }

        #endregion
    }
}
