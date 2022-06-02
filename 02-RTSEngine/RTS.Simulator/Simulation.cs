using MySql.Data.MySqlClient;
using RTS.Configuration;
using RTS.Database;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Simulator
{
    public class Simulation
    {
        #region Properties

        ///// <summary>
        ///// Represents the player connexion to the SQL server
        ///// </summary>
        //public MySqlConnection SQLConnexion
        //{
        //    set { sqlConnexion = value; }
        //    get
        //    {
        //        if(sqlConnexion == null)
        //            sqlConnexion = DatabaseConnector.GetNewConnection();

        //        if (sqlConnexion.State != System.Data.ConnectionState.Open)
        //        {
        //            sqlConnexion.Close();
        //            sqlConnexion = DatabaseConnector.GetNewConnection();
        //        }

        //        return sqlConnexion;
        //    }
        //}
        //private MySqlConnection sqlConnexion;


        /// <summary>
        /// Player on who the simulation is based
        /// </summary>
        public PlayerModel Player { get => player; private set => player = value; }
        private PlayerModel player;

        /// <summary>
        /// Map of the player
        /// </summary>
        public PlayerMapModel PlayerMap { private set; get; }

        /// <summary>
        /// Buildings of the player
        /// </summary>
        public PlayerBuildingManagerModel BuildingManager { private set; get; }

        /// <summary>
        /// Player market data
        /// </summary>
        public PlayerMarketModel PlayerMarket { private set; get; }

        /// <summary>
        /// End time of the simulator
        /// </summary>
        DateTime end;

        /// <summary>
        /// Current cursor of the time simulator
        /// </summary>
        public DateTime timeCursor;

        /// <summary>
        /// List of simulable object that the simulator handle
        /// </summary>
        List<ISimulable> simulableObjects;

        /// <summary>
        /// Flags that indicates if the simulator is running or not
        /// </summary>
        bool isSimulating = false;

        #endregion

        #region Constructor

        public Simulation(MySqlConnection pDatabaseConnexion, int pPlayerId)
        {
            Console.WriteLine("[RTS.Simulation] INFO : Simulation created for player " + pPlayerId);

            simulableObjects = new List<ISimulable>();

            Player = PlayerFactory.GetById(pDatabaseConnexion, pPlayerId);
            Player.resourceBag = PlayerFactory.GetResources(pDatabaseConnexion, Player);

            //Get Map for player
            PlayerMap = PlayerMapFactory.GetByPlayerId(pDatabaseConnexion, pPlayerId);

            //Get Player Market
            PlayerMarket = PlayerMarketFactory.GetPlayerMarket(pDatabaseConnexion, Player);
            PlayerMarket.Player = Player;
            PlayerMarket.Level = MarketData.GetMarket().Levels[PlayerMarket.Level.Id - 1]; 

            //Get buildings
            BuildingManager = new PlayerBuildingManagerModel(player);
            GetAllBuildings(pDatabaseConnexion);

            //// Reset all persistent resources, and recalculate them
            //Player.resourceBag.ResetAllResourceByTypeId((int)TypeOfResource.PERSISTENT);
            //Player.resourceBag.AddBag(BuildingManager.GetProvidedPersistentBag());
        }

        #endregion

        #region Implementations

        /// <summary>
        /// Add a building to the player simulation
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        public void AddBuilding(PlayerBuildingModel pPlayerBuilding)
        {
            PlayerBuildingSimulable playerBuildingSimulable = new PlayerBuildingSimulable(pPlayerBuilding);

            simulableObjects.Add(playerBuildingSimulable);
            BuildingManager.Add(playerBuildingSimulable.Model);
        }

        /// <summary>
        /// Add a building to the player simulation
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        public void RemoveBuilding(int pBuildingNumber)
        {
            PlayerBuildingModel building = BuildingManager.GetPlayerBuildingByNumber(pBuildingNumber);
            int timeSimulableIndex = simulableObjects.FindIndex(s => s.Equals(building));

            if (building != null)
            {
                building.Destroy();
                BuildingManager.RemoveByNumber(building.buildingNumber);
                simulableObjects.RemoveAt(timeSimulableIndex);
                Console.WriteLine("[RTS.Simulation] INFO : Building removed from the simulation");
            }
            else
            {
                throw new Exception("[RTS.Simulation] ERROR : Bulding or building has not been found");
            }
        }


        private void GetAllBuildings(MySqlConnection pDatabaseConnection)
        {
            Console.WriteLine("[RTS.Simulation] INFO : Retrieve all buildings player " + Player.id);

            /////////////////
            /// Simulate Buildings Producer
            List<PlayerBuildingProducerModel> buildingProducerList = PlayerBuildingProducerFactory.GetAllBuildings(pDatabaseConnection, Player.id);

            for (int i = 0; i < buildingProducerList.Count(); i++)
            {
                //Set data
                buildingProducerList[i].Player = Player;
                buildingProducerList[i].Building = (BuildingProducerModel)BuildingData.GetBuildingById(buildingProducerList[i].Building.id);
                buildingProducerList[i].Level = buildingProducerList[i].Building.Levels[buildingProducerList[i].Level.id - 1];

                AddBuilding(buildingProducerList[i]);
            }

            /////////////////
            /// Simulate Buildings Passive
            List<PlayerBuildingPassiveModel> buildingPassiveList = PlayerBuildingPassiveFactory.GetAllBuildings(pDatabaseConnection, Player.id);

            for (int i = 0; i < buildingPassiveList.Count(); i++)
            {
                //Set data
                buildingPassiveList[i].Player = Player;
                buildingPassiveList[i].Building = (BuildingPassiveModel)BuildingData.GetBuildingById(buildingPassiveList[i].Building.id);
                buildingPassiveList[i].Level = buildingPassiveList[i].Building.Levels[buildingPassiveList[i].Level.id - 1];

                AddBuilding(buildingPassiveList[i]);
            }
        }

        /// <summary>
        /// Save informations into the database
        /// </summary>
        public void Persist(MySqlConnection pDatabaseConnection)
        {
            /////////////////////////
            /// Update all simulables

            //Get the connection & begin transaction
            MySqlTransaction transaction = pDatabaseConnection.BeginTransaction();

            try
            {
                //Update all simulables
                for (int i = 0; i < simulableObjects.Count; i++)
                {
                    simulableObjects[i].UpdateDatabase(ref transaction);
                }

                //Update resources
                PlayerFactory.UpdateResourceBagWithTransaction(Player, transaction);

                //Commit the entire transaction
                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                transaction.Rollback();
            }
        }

        /// <summary>
        /// Simulate game production over time
        /// </summary>
        public void Simulate()
        {

            if (!isSimulating)
            {
                /////////////////////////
                /// Prepare simulation

                Console.WriteLine("[RTS.Simulation] INFO : Simulation started for player " + Player.id);
                Console.WriteLine("[RTS.Simulation] INFO : Current hour id " + DateTime.Now);


                //Initialize variable (start lesser than end to enter while loop)
                end = DateTime.Now;

                ISimulable simulable = GetNextTimeSimulable();
                if (simulable == null)
                {
                    /////////////////////////
                    /// End the simulation
                    timeCursor = end;
                }
                else
                {
                    /////////////////////////
                    /// Start simulation

                    timeCursor = simulable.GetNextActionDateTime();

                    //Simulation loop
                    while (timeCursor < end)
                    {
                        //Make the simulation of the simulable object
                        simulable.SimulateOverTime(this, ref player);

                        //Get next object to simulate and update time cursor
                        simulable = GetNextTimeSimulable();
                        timeCursor = simulable.GetNextActionDateTime();
                    }

                }

                isSimulating = false;
            }
        }

        /// <summary>
        /// The the next object to simulate
        /// </summary>
        /// <returns></returns>
        ISimulable GetNextTimeSimulable()
        {
            return simulableObjects.OrderBy(its => its.GetNextActionDateTime()).FirstOrDefault();
        }

        #endregion
    }
}
