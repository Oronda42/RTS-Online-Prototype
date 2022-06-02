using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Database
{
    public class PlayerBuildingPassiveFactory
    {
        #region Implementation

        /// <summary>
        /// Create a building into database
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        /// <returns></returns>
        public static bool Create(MySqlConnection pConnection, PlayerBuildingPassiveModel pPlayerBuilding)
        {
            MySqlTransaction transaction = pConnection.BeginTransaction();
            if (CreateWithTransaction(pPlayerBuilding, transaction))
            {
                transaction.Commit();
                return true;                
            }
            else
            {
                transaction.Rollback();
                return false;
            }            
        }

        /// <summary>
        /// Create a building into database within a SQL transaction
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        /// <returns></returns>
        public static bool CreateWithTransaction(PlayerBuildingPassiveModel pPlayerBuilding, MySqlTransaction pTransaction)
        {
            try
            {
                MySqlCommand updateCmd = pTransaction.Connection.CreateCommand();

                /////////////////////////
                // Insert into building base

                PlayerBuildingFactory.CreateWithTransaction(pPlayerBuilding, pTransaction);

                ////////////////////////////////////
                // Building Resource Passive

                MySqlCommand insertCmd = pTransaction.Connection.CreateCommand();
                 
                //Query
                insertCmd.CommandText = string.Format(@"INSERT INTO {0}
                (player_id, building_id, building_number, start_consumption, last_consumption)
                VALUES 
                (@player_id, @building_id, @building_number, @start_consumption, @last_consumption)",
                Constants.TableName.PLAYER_BUILDING_PASSIVE);

                //Set parameters
                insertCmd.Parameters.Add("@player_id", MySqlDbType.Int32).Value = pPlayerBuilding.Player.id;
                insertCmd.Parameters.Add("@building_id", MySqlDbType.Int32).Value = pPlayerBuilding.Building.id;
                insertCmd.Parameters.Add("@building_number", MySqlDbType.Int32).Value = pPlayerBuilding.buildingNumber;

                insertCmd.Parameters.Add("@start_consumption", MySqlDbType.DateTime).Value = pPlayerBuilding.startConsumption;
                insertCmd.Parameters.Add("@last_consumption", MySqlDbType.DateTime).Value = pPlayerBuilding.lastConsumption;

                //Execute query
                insertCmd.ExecuteNonQuery();

                return true;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Get all produced buildings from the database for a specified player
        /// </summary>
        /// <param name="pPlayerId"></param>
        /// <returns></returns>
        public static List<PlayerBuildingPassiveModel> GetAllBuildings(MySqlConnection pConnection, int pPlayerId)
        {
            List<PlayerBuildingPassiveModel> buildingsToReturn = new List<PlayerBuildingPassiveModel>();
            
            //Query
            string query = string.Format(@"
            SELECT 
            pb.creation, pb.building_number buildingNumber, pb.map_extent_id mapExtentId, pb.map_element_instance_id mapElementInstanceId, pbp.last_consumption lastConsumption, pbp.start_consumption startConsumption,
            b.building_type_id id,
            b.id, 
            bpl.level_id id, 
            bs.id 
            FROM 
            {0} pb, 
            {1} pbp,
            {2} b,
            {3} brp,
            {4} bpl, 
            {5} bs
            WHERE 
            pb.player_id = pbp.player_id and pb.building_number = pbp.building_number and pb.building_id = pbp.building_id
            and pb.building_id = b.id
            and pb.building_id = brp.building_id
            and pb.building_id = bpl.building_id and bpl.level_id = pb.level_id
            and pb.state_id = bs.id
            and pb.player_id = {6}",
            Constants.TableName.PLAYER_BUILDING,                    //pb
            Constants.TableName.PLAYER_BUILDING_PASSIVE,            //pbp
            Constants.TableName.BUILDING,                           //b
            Constants.TableName.BUILDING_PASSIVE,                   //bp
            Constants.TableName.BUILDING_PASSIVE_LEVEL,             //bpl
            Constants.TableName.BUILDING_STATE,                     //bs
            pPlayerId);

            //If more than 5 types : https://stackoverflow.com/questions/10202584/using-dapper-to-map-more-than-5-types
            pConnection.Query<PlayerBuildingPassiveModel, BuildingTypeModel, BuildingPassiveModel, BuildingPassiveLevelModel, BuildingStateModel, List<PlayerBuildingPassiveModel>>(
                query,
                (playerBuilding, buildingType, building, buildingLevel, stateBuilding) =>
                {
                    //Add buildings to the list
                    playerBuilding.Player = new PlayerModel { id = pPlayerId };
                    playerBuilding.State = stateBuilding;
                    playerBuilding.Building = building;
                    playerBuilding.Level = buildingLevel;
                    playerBuilding.Building.buildingType = buildingType;

                    buildingsToReturn.Add(playerBuilding);

                    return null;

                }, splitOn: "id,id,id,id");


            return buildingsToReturn;
        }

        /// <summary>
        /// Update or create the player building
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        public static void UpdateOrCreate(MySqlConnection pConnection, PlayerBuildingPassiveModel pPlayerBuilding)
        {
            if (PlayerBuildingFactory.BuildingExists(pConnection, pPlayerBuilding.Player.id, pPlayerBuilding.buildingNumber))
                Update(pConnection, pPlayerBuilding);
            else
                Create(pConnection, pPlayerBuilding);
        }

        /// <summary>
        /// Update or create the player building
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        public static void UpdateOrCreateWithTransaction(PlayerBuildingPassiveModel pPlayerBuilding, MySqlTransaction pTransaction)
        {
            if (PlayerBuildingFactory.BuildingExists(pTransaction.Connection, pPlayerBuilding.Player.id, pPlayerBuilding.buildingNumber))
                UpdateWithTransaction(pPlayerBuilding, pTransaction);
            else
                CreateWithTransaction(pPlayerBuilding, pTransaction);
        }

        /// <summary>
        /// Update within a single transaction
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        public static void Update(MySqlConnection pConnection, PlayerBuildingPassiveModel pPlayerBuilding)
        {
            MySqlTransaction transaction = pConnection.BeginTransaction();
            UpdateWithTransaction(pPlayerBuilding, transaction);
            transaction.Commit();
        }

        /// <summary>
        /// Update data on database from an object
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        /// <param name="pTransaction"></param>
        public static void UpdateWithTransaction(PlayerBuildingPassiveModel pPlayerBuilding, MySqlTransaction pTransaction)
        {
            if (pTransaction == null)
                throw new Exception("Transaction is null");

            //Update player building base
            PlayerBuildingFactory.UpdateWithTransaction(pPlayerBuilding, pTransaction);

            MySqlCommand updateCmd = pTransaction.Connection.CreateCommand();

            //Update producer building
            updateCmd.CommandText = string.Format(@"UPDATE
                {0} pbp
                SET pbp.last_consumption = @last_consumption,
                pbp.start_consumption = @start_consumption
                WHERE pbp.player_id = {1} 
                AND pbp.building_number = {2}",
            Constants.TableName.PLAYER_BUILDING_PASSIVE,
            pPlayerBuilding.Player.id,
            pPlayerBuilding.buildingNumber
            );

            //Start consumption data
            if (pPlayerBuilding.startConsumption == null || pPlayerBuilding.startConsumption == DateTime.MinValue)
                updateCmd.Parameters.Add("@start_consumption", MySqlDbType.DateTime).Value = (object)DBNull.Value;
            else
                updateCmd.Parameters.Add("@start_consumption", MySqlDbType.DateTime).Value = pPlayerBuilding.startConsumption;

            //Last consumption data
            if (pPlayerBuilding.lastConsumption == null || pPlayerBuilding.lastConsumption == DateTime.MinValue)
                updateCmd.Parameters.Add("@last_consumption", MySqlDbType.DateTime).Value = (object)DBNull.Value;
            else
                updateCmd.Parameters.Add("@last_consumption", MySqlDbType.DateTime).Value = pPlayerBuilding.lastConsumption;

            //Execute query
            updateCmd.ExecuteNonQuery();

        }

        /// <summary>
        /// Delete a building
        /// </summary>
        /// <param name="pPlayerId"></param>
        /// <param name="pPosition"></param>
        /// <param name="pBuildingNumber"></param>
        /// <returns></returns>
        public static bool Delete(MySqlConnection pConnection, int pPlayerId, string pPosition, int pBuildingNumber)
        {
            MySqlTransaction pTransaction = pConnection.BeginTransaction();

            try
            {
                PlayerBuildingFactory.DeleteWithTransaction(pPlayerId, pPosition, pBuildingNumber, pTransaction);

                MySqlCommand deleteCmd = pConnection.CreateCommand();
                deleteCmd.Transaction = pTransaction;

                //Query
                deleteCmd.CommandText = string.Format(@"
                DELETE FROM 
                {0}
                WHERE 
                player_id = {1}
                and building_number = {2}",
                Constants.TableName.PLAYER_BUILDING_PASSIVE,
                pPlayerId,
                pBuildingNumber);

                deleteCmd.ExecuteNonQuery();

                pTransaction.Commit();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                pTransaction.Rollback();
                return false;
            }
        }


        #endregion
    }
}
