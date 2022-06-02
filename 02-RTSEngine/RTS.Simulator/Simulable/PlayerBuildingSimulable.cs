using MySql.Data.MySqlClient;
using RTS.Database;
using RTS.Models;
using System;

namespace RTS.Simulator
{
    public class PlayerBuildingSimulable : ISimulable
    {
        #region Properties

        public PlayerBuildingModel Model { private set; get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pModel"></param>
        public PlayerBuildingSimulable(PlayerBuildingModel pModel)
        {
            Model = pModel;
        }

        #endregion

        #region ITimeSimulable Implementation

        public DateTime GetNextActionDateTime()
        {
            return Model.GetNextActionTime();
        }

        public void SimulateOverTime(Simulation pSimulation, ref PlayerModel pPlayer)
        {
            //Perform action and calculate next action date time
            Model.Tick(pSimulation.timeCursor);
        }

        public void UpdateDatabase(ref MySqlTransaction pTransaction)
        {
            switch (Model)
            {
                case PlayerBuildingPassiveModel passive :
                    PlayerBuildingPassiveFactory.UpdateOrCreateWithTransaction(passive, pTransaction);
                    break;
                case PlayerBuildingProducerModel producer:
                    PlayerBuildingProducerFactory.UpdateOrCreateWithTransaction(producer, pTransaction);
                    break;
                default:
                    Console.WriteLine("[RTS.Simulation] ERROR : Building type not handled");
                    break;
            }
            
        }

        public override bool Equals(object pOther)
        {
            if(pOther is PlayerBuildingModel)
            {
                if (Model.buildingNumber == ((PlayerBuildingModel)pOther).buildingNumber)
                    return true;
            }

            return false;
        }
        #endregion
    }
}
