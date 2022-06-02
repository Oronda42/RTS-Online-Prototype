using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System;
using System.Collections.Generic;

namespace RTS.Database
{
    public static class PlayerBuildingCenterFactory
    {

        /// <summary>
        /// Insert a new map extent
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static void CreateWithTransaction(PlayerBuildingCenterModel pPlayerBuilding, MySqlTransaction pTransaction)
        {
            if (pTransaction == null)
                throw new Exception("Transaction is null");

            MySqlCommand insertCmd = pTransaction.Connection.CreateCommand();

            /////////////////////////
            // Insert into building base

            PlayerBuildingFactory.CreateWithTransaction(pPlayerBuilding, pTransaction);

           

            ////////////////////////////////////
            // Building Center

            //Query
            insertCmd.CommandText = string.Format(@"INSERT INTO {0}
                (player_id, building_id, building_number)
                VALUES 
                (@player_id, @building_id, @building_number)",
            Constants.TableName.PLAYER_BUILDING_CENTER);

            //Set parameters
            insertCmd.Parameters.Add("@player_id", MySqlDbType.Int32).Value = pPlayerBuilding.Player.id;
            insertCmd.Parameters.Add("@building_id", MySqlDbType.Int32).Value = pPlayerBuilding.Building.id;
            insertCmd.Parameters.Add("@building_number", MySqlDbType.Int32).Value = pPlayerBuilding.buildingNumber;

            //Execute query
            insertCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get a player center
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <returns></returns>
        public static PlayerBuildingCenterModel Get(MySqlConnection pConnection, int pPlayerId)
        {
            string query = string.Format(@"SELECT
                pb.building_number buildingNumber, pb.creation, pb.map_extent_id mapExtentId, pb.map_element_instance_id mapElementInstanceId, 
                pb.building_id, 
                pb.level_id,
                pb.state_id
                FROM 
                {0} pb INNER JOIN {1} pbc ON (pb.player_id = pbc.player_id and pb.building_number = pbc.building_number)
                WHERE 
                pb.player_id = {2}",
                Constants.TableName.PLAYER_BUILDING,
                Constants.TableName.PLAYER_BUILDING_CENTER, 
                pPlayerId);
             
            //Create the map
            PlayerBuildingCenterModel playerCenter = new PlayerBuildingCenterModel();

            pConnection.Query<PlayerBuildingCenterModel>(query,
                new[] {
                    typeof(PlayerBuildingCenterModel),
                    typeof(int),
                    typeof(int),
                    typeof(int)},
                objects =>
                {
                    playerCenter = objects[0] as PlayerBuildingCenterModel;
                    playerCenter.Player = new PlayerModel { id = pPlayerId };
                    playerCenter.Building = new BuildingCenterModel { id = Convert.ToInt32(objects[1]) };
                    playerCenter.Level = new BuildingCenterLevelModel { id = Convert.ToInt32(objects[2]) };
                    playerCenter.State = new BuildingStateModel { id = Convert.ToInt32(objects[3]) };

                    return playerCenter;
                },
                splitOn: "building_id, level_id, state_id");

            playerCenter.Building = BuildingCenterFactory.GetById(pConnection, playerCenter.Building.id);
            playerCenter.Level = playerCenter.Building.Levels[playerCenter.Level.id - 1];

            return playerCenter;
        }
    }
}
