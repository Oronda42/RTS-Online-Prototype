using System;

namespace RTS.Models
{
    [Serializable]
    public class BuildingStateModel
    {
        #region Properties

        public int id;
        public string name;
        public bool canProduce;
        public bool canActivate;
        public bool canRepair;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="pName"></param>
        /// <param name="pCanProduce"></param>
        /// <param name="pIsActivated"></param>
        /// <param name="pIsRepairable"></param>
        public BuildingStateModel(int pId, string pName, bool pCanProduce, bool pCanActivate, bool pCanRepair)
        {
            id = pId;
            name = pName;
            canProduce = pCanProduce;
            canActivate = pCanActivate;
            canRepair = pCanRepair;
        }

        /// <summary>
        /// Parameter less (needed for dapper)
        /// </summary>
        public BuildingStateModel()
        {

        }

        /// <summary>
        /// just basic state id (for receiving message)
        /// </summary>
        public BuildingStateModel(int pId)
        {
            id = pId;
        }

    }
}
