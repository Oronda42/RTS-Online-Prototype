using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Database
{
    public static class PlayerBuildingProducerFactory
    {
        #region Implementation

        /// <summary>
        /// Create a building into database
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        /// <returns></returns>
        public static bool Create(MySqlConnection pConnection, PlayerBuildingProducerModel pPlayerBuilding)
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
        /// Create a building into database
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        /// <returns></returns>
        public static bool CreateWithTransaction(PlayerBuildingProducerModel pPlayerBuilding, MySqlTransaction pTransaction)
        {
            try
            {
                MySqlCommand updateCmd = pTransaction.Connection.CreateCommand();

                /////////////////////////
                // Insert into building base

                PlayerBuildingFactory.CreateWithTransaction(pPlayerBuilding, pTransaction);

                ////////////////////////////////////
                // Building Resource Producer

                MySqlCommand insertCmd = pTransaction.Connection.CreateCommand();                

                //Query
                insertCmd.CommandText = string.Format(@"INSERT INTO {0}
                (player_id, building_id, building_number, auto_produce, current_resource_id_produced, start_production, last_production)
                VALUES 
                (@player_id, @building_id, @building_number, @auto_produce, @current_resource_id_produced, @start_production, @last_production)",
                Constants.TableName.PLAYER_BUILDING_PRODUCER);

                //Set parameters
                insertCmd.Parameters.Add("@player_id", MySqlDbType.Int32).Value = pPlayerBuilding.Player.id;
                insertCmd.Parameters.Add("@building_id", MySqlDbType.Int32).Value = pPlayerBuilding.Building.id;
                insertCmd.Parameters.Add("@building_number", MySqlDbType.Int32).Value = pPlayerBuilding.buildingNumber;

                insertCmd.Parameters.Add("@auto_produce", MySqlDbType.UByte).Value = pPlayerBuilding.autoProduce;
                insertCmd.Parameters.Add("@current_resource_id_produced", MySqlDbType.UByte).Value = pPlayerBuilding.Building.defaultResourceIdProduced;
                insertCmd.Parameters.Add("@start_production", MySqlDbType.DateTime).Value = pPlayerBuilding.startProduction;
                insertCmd.Parameters.Add("@last_production", MySqlDbType.DateTime).Value = pPlayerBuilding.lastProduction;

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
        public static List<PlayerBuildingProducerModel> GetAllBuildings(MySqlConnection pConnection, int pPlayerId)
        {
            List<PlayerBuildingProducerModel> buildingsToReturn = new List<PlayerBuildingProducerModel>();
            
            //Query
            string query = string.Format(@"
            SELECT 
            pb.building_number buildingNumber, pb.map_extent_id mapExtentId, pb.map_element_instance_id mapElementInstanceId, pbr.auto_produce autoProduce, pbr.current_resource_id_produced currentResourceIdProduced, pbr.start_production startProduction, pbr.last_production lastProduction, pb.creation,
            b.building_type_id id,
            b.id, b.name, b.construction_time constructionTime,
            bpl.resource_id_produced resourceIdProduced, bpl.level_id id, bpl.seconds_to_produce secondsToProduce, bpl.amount_produced amountProduced,
            bs.id, bs.name, bs.can_produce canProduce, bs.can_activate canActivate, bs.can_repair canRepair
            FROM 
            {0} pb, 
            {1} pbr,
            {2} b,
            {3} brp,
            {4} bpl, 
            {5} bs
            WHERE 
            pb.player_id = pbr.player_id and pb.building_number = pbr.building_number and pb.building_id = pbr.building_id
            and pb.building_id = b.id
            and pb.building_id = brp.building_id
            and pb.building_id = bpl.building_id and bpl.level_id = pb.level_id and bpl.resource_id_produced = pbr.current_resource_id_produced
            and pb.state_id = bs.id
            and pb.player_id = {6}",
            Constants.TableName.PLAYER_BUILDING,                    //pb
            Constants.TableName.PLAYER_BUILDING_PRODUCER,           //pbr
            Constants.TableName.BUILDING,                           //b
            Constants.TableName.BUILDING_PRODUCER,                  //brp
            Constants.TableName.BUILDING_PRODUCER_LEVEL,            //bpl
            Constants.TableName.BUILDING_STATE,                     //bs
            pPlayerId);

            //If more than 5 types : https://stackoverflow.com/questions/10202584/using-dapper-to-map-more-than-5-types
            pConnection.Query<PlayerBuildingProducerModel, BuildingTypeModel, BuildingProducerModel, BuildingProducerLevelModel, BuildingStateModel, List<PlayerBuildingProducerModel>>(
                query,
                (playerResourceBuilding, buildingType, resourceBuilding, resourceBuildingLevel, stateBuilding) =>
                {
                    //Add buildings to the list
                    playerResourceBuilding.Player = new PlayerModel { id = pPlayerId };
                    playerResourceBuilding.State = stateBuilding;
                    playerResourceBuilding.Building = resourceBuilding;
                    playerResourceBuilding.Level = resourceBuildingLevel;
                    playerResourceBuilding.Building.buildingType = buildingType;

                    buildingsToReturn.Add(playerResourceBuilding);

                    return null;

                }, splitOn: "id,id,resourceIdProduced,id");


            return buildingsToReturn;
        }

        /// <summary>
        /// Update or create the player building
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        public static void UpdateOrCreate(MySqlConnection pConnection, PlayerBuildingProducerModel pPlayerBuilding)
        {
            if (PlayerBuildingFactory.BuildingExists(pConnection, pPlayerBuilding.Player.id, pPlayerBuilding.buildingNumber))
                Update(pConnection, pPlayerBuilding);
            else
                Create(pConnection, pPlayerBuilding);
        }

        /// <summary>
        /// Update or create the player building within a transaction
        /// </summary>
        /// <param name="pPlayerBuilding"></param>
        public static void UpdateOrCreateWithTransaction(PlayerBuildingProducerModel pPlayerBuilding, MySqlTransaction pTransaction)
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
        public static void Update(MySqlConnection pConnection, PlayerBuildingProducerModel pPlayerBuilding)
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
        public static void UpdateWithTransaction(PlayerBuildingProducerModel pPlayerBuilding, MySqlTransaction pTransaction)
        {
            if (pTransaction == null)
                throw new Exception("Transaction is null");

            //Update player building base
            PlayerBuildingFactory.UpdateWithTransaction(pPlayerBuilding, pTransaction);

            MySqlCommand updateCmd = pTransaction.Connection.CreateCommand();
            
            //Update producer building
            updateCmd.CommandText = string.Format(@"UPDATE
                {0} pbp
                SET pbp.auto_produce = {1},
                pbp.start_production = @start_production,
                pbp.last_production = @last_production
                WHERE pbp.player_id = {4} 
                AND pbp.building_number = {5}",
            Constants.TableName.PLAYER_BUILDING_PRODUCER,
            pPlayerBuilding.autoProduce,
            pPlayerBuilding.startProduction,
            pPlayerBuilding.lastProduction,
            pPlayerBuilding.Player.id,
            pPlayerBuilding.buildingNumber
            );

            //Start production data
            if (pPlayerBuilding.startProduction == null || pPlayerBuilding.startProduction == DateTime.MinValue)
                updateCmd.Parameters.Add("@start_production", MySqlDbType.DateTime).Value = (object)DBNull.Value;
            else
                updateCmd.Parameters.Add("@start_production", MySqlDbType.DateTime).Value = pPlayerBuilding.startProduction;

            //Last production data
            if (pPlayerBuilding.lastProduction == null || pPlayerBuilding.lastProduction == DateTime.MinValue)
                updateCmd.Parameters.Add("@last_production", MySqlDbType.DateTime).Value = (object)DBNull.Value;
            else
                updateCmd.Parameters.Add("@last_production", MySqlDbType.DateTime).Value = pPlayerBuilding.lastProduction;

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
                Constants.TableName.PLAYER_BUILDING_PRODUCER,
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