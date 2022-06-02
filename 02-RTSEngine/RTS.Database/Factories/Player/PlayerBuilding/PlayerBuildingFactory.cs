using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System;
using System.Linq;

namespace RTS.Database
{
    public static class PlayerBuildingFactory
    {
        #region Implementation

        /// <summary>
        /// Create a 
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        /// <param name="pTransaction"></param>
        /// <returns></returns>
        internal static bool CreateWithTransaction(PlayerBuildingModel pPlayerBuilding, MySqlTransaction pTransaction)
        {
            if (pTransaction == null)
                throw new Exception("Transaction is null");

            try
            {
                MySqlCommand insertCmd = pTransaction.Connection.CreateCommand();

                //Query
                insertCmd.CommandText = string.Format(@"INSERT INTO {0}
                (player_id, building_id, building_number, map_extent_id, map_element_instance_id, creation, state_id)
                VALUES 
                (@player_id, @building_id, @building_number, @map_extent_id, @map_element_instance_id, @creation, @state_id)",
                Constants.TableName.PLAYER_BUILDING);

                //Set parameters
                insertCmd.Parameters.Add("@player_id", MySqlDbType.Int32).Value = pPlayerBuilding.Player.id;
                insertCmd.Parameters.Add("@building_id", MySqlDbType.Int32).Value = pPlayerBuilding.Building.id;
                insertCmd.Parameters.Add("@building_number", MySqlDbType.Int32).Value = pPlayerBuilding.buildingNumber;
                insertCmd.Parameters.Add("@map_extent_id", MySqlDbType.String).Value = pPlayerBuilding.mapExtentId;
                insertCmd.Parameters.Add("@map_element_instance_id", MySqlDbType.String).Value = pPlayerBuilding.mapElementInstanceId;

                insertCmd.Parameters.Add("@creation", MySqlDbType.DateTime).Value = pPlayerBuilding.creation;
                insertCmd.Parameters.Add("@state_id", MySqlDbType.Int32).Value = pPlayerBuilding.State.id;

                insertCmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        internal static bool UpdateWithTransaction(PlayerBuildingModel pPlayerBuilding, MySqlTransaction pTransaction)
        {
            if (pTransaction == null)
                throw new Exception("Transaction is null");

            try
            {
                MySqlCommand updateCmd = pTransaction.Connection.CreateCommand();

                //Update building base
                updateCmd.CommandText = string.Format(@"UPDATE {0} pb
                SET pb.state_id = @state_id,
                pb.level_id = @level_id
                WHERE pb.player_id = @player_id AND pb.building_number = @building_number",
                Constants.TableName.PLAYER_BUILDING);

                //Set parameters
                updateCmd.Parameters.Add("@player_id", MySqlDbType.Int32).Value = pPlayerBuilding.Player.id;
                updateCmd.Parameters.Add("@building_number", MySqlDbType.Int32).Value = pPlayerBuilding.buildingNumber;
                updateCmd.Parameters.Add("@state_id", MySqlDbType.Int32).Value = pPlayerBuilding.State.id;
                updateCmd.Parameters.Add("@level_id", MySqlDbType.String).Value = pPlayerBuilding.Level.id;

                //Execute query
                updateCmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        /// <summary>
        /// Delete a building within a SQL transaction
        /// </summary>
        /// <param name="pPlayerId"></param>
        /// <param name="pPosition"></param>
        /// <param name="pBuildingNumber"></param>
        /// <param name="pTransaction"></param>
        internal static void DeleteWithTransaction(int pPlayerId, string pPosition, int pBuildingNumber, MySqlTransaction pTransaction)
        {

            if (pTransaction == null)
                throw new Exception("Transaction is null");

            MySqlCommand deleteCommand = pTransaction.Connection.CreateCommand();
            deleteCommand.Transaction = pTransaction;

            //Query
            deleteCommand.CommandText = string.Format(@"
            DELETE FROM 
            {0}
            WHERE 
            player_id = {1}
            and building_number = {3}",
            Constants.TableName.PLAYER_BUILDING,
            pPlayerId,
            pPosition,
            pBuildingNumber);

            deleteCommand.ExecuteNonQuery();
        }
        
        /// <summary>
        /// Returns true if the building exists, otherwise false
        /// </summary>
        /// <returns></returns>
        public static bool BuildingExists(MySqlConnection pConnection, int pPlayerId, int pBuildingNumber)
        {
            //Query
            string query = string.Format(@"
            SELECT COUNT(*)
            FROM 
            {0} pb
            WHERE 
            pb.player_id = {1}
            and pb.building_number = {2}",
            Constants.TableName.PLAYER_BUILDING,
            pPlayerId,
            pBuildingNumber);
            
            return pConnection.QueryFirstOrDefault<bool>(query);
        }

        /// <summary>
        /// Update the level and remove resources
        /// </summary>
        /// <param name="pPlayerId"></param>
        /// <param name="pBuildingNumber"></param>
        /// <param name="pBuildingLevel"></param>
        public static bool IncrementLevel(MySqlConnection pConnection, int pPlayerId, int pBuildingNumber)
        {
            MySqlTransaction transaction = pConnection.BeginTransaction();
            
            //Get the cost ID
            int costId = GetNextLevelCostId(pConnection, pPlayerId, pBuildingNumber);

            //Check if the player has enough resource for the specified cost
            bool hasEnoughResource = PlayerFactory.HasEnoughResourceForCost(pConnection, pPlayerId, costId);
            bool canLevelUp = CanLevelUp(pConnection, pPlayerId, pBuildingNumber);

            if (hasEnoughResource && canLevelUp)
            {
                try
                {
                    PlayerFactory.ConsumeWithTransaction(pPlayerId, costId, transaction);

                    MySqlCommand updateCmd = transaction.Connection.CreateCommand();

                    //Query
                    updateCmd.CommandText = string.Format(@"
                    UPDATE {0}
                    SET level_id = level_id + 1
                    WHERE player_id = {1}
                    and building_number = {2}",
                    Constants.TableName.PLAYER_BUILDING,
                    pPlayerId,
                    pBuildingNumber);

                    updateCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }


                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the cost id of a level (If 0, no next level available with a cost)
        /// </summary>
        /// <returns></returns>
        public static int GetNextLevelCostId(MySqlConnection pConnection, int pPlayerId, int pBuildingNumber)
        {
            //Query
            string query = string.Format(@"
            select rb.id
            FROM {0} rb
            INNER JOIN {1} blc ON (rb.id = blc.resource_cost_id)
            INNER JOIN {2} pb ON (pb.building_id = blc.building_id)
            WHERE pb.player_id = {3}
            and pb.building_number = {4}
            and blc.level_id = pb.level_id + 1",
            Constants.TableName.RESOURCE_BAG,
            Constants.TableName.BUILDING_LEVEL_COST,
            Constants.TableName.PLAYER_BUILDING,
            pPlayerId,
            pBuildingNumber);

            //Returns the result
            if (pConnection.Query<int>(query).Count() > 0)
                return pConnection.Query<int>(query).ToList()[0];

            return 0;
        }

        /// Can increment the level building
        /// </summary>
        /// <param name="pPlayerId"></param>
        /// <param name="pBuildingNumber"></param>
        /// <returns></returns>
        public static bool CanLevelUp(MySqlConnection pConnection, int pPlayerId, int pBuildingNumber)
        {
            //Query
            string query = string.Format(@"
                SELECT blc.level_id
                FROM {0} blc
                INNER JOIN {1} b ON (b.id = blc.building_id)
                INNER JOIN {2} pb ON (pb.building_id = b.id)
                WHERE pb.player_id = {3}
                AND pb.building_number = {4}
                AND blc.level_id = pb.level_id + 1",
            Constants.TableName.BUILDING_LEVEL_COST,
            Constants.TableName.BUILDING,
            Constants.TableName.PLAYER_BUILDING,
            pPlayerId,
            pBuildingNumber);

            if (pConnection.Query<int>(query).Count() > 0)
                return true;
            else
                return false;
        }

        #endregion
    }
}
