using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Database
{
    public static class PlayerCityFactory
    {
        internal static bool CreateWithTransaction(PlayerCityModel pPlayerCityModel, MySqlTransaction pTransaction)
        {
            if (pTransaction == null)
                throw new Exception("Transaction is null");

            try
            {
                MySqlCommand insertCmd = pTransaction.Connection.CreateCommand();

                //Query
                insertCmd.CommandText = string.Format(@"INSERT INTO {0}
                (player_id, position, level_id)
                VALUES 
                (@player_id, @position, @level_id)",
                Constants.TableName.PLAYER_CITY);

                //Set parameters
                insertCmd.Parameters.Add("@player_id", MySqlDbType.Int32).Value = pPlayerCityModel.playerId;
                insertCmd.Parameters.Add("@position", MySqlDbType.String).Value = pPlayerCityModel.position;
                insertCmd.Parameters.Add("@level_id", MySqlDbType.Int32).Value = pPlayerCityModel.level.id;

                insertCmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public static List<PlayerCityModel> GetAllNeighboors(int pPlayerId, MySqlConnection pConnection)
        {
            List<PlayerCityModel> citiesToReturn = new List<PlayerCityModel>();

            //Query
            string query = string.Format(@"
            SELECT 
            pc.player_id playerId, pc.position position,
            cl.id id
            FROM 
            {0} pc, 
            {1} cl
            WHERE 
            pc.level_id = cl.id
            AND pc.player_id != {2}",
            Constants.TableName.PLAYER_CITY,        //pc
            Constants.TableName.CITY_LEVEL,         //cl
            pPlayerId);

            pConnection.Query<PlayerCityModel, PlayerCityLevelModel, PlayerCityModel>(
                    query,
                    (playerCityModel, playerCityLevelModel) =>
                    {
                        //Add buildings to the list
                        playerCityModel.level = playerCityLevelModel;
                        citiesToReturn.Add(playerCityModel);

                        return playerCityModel;

                    }, splitOn: "id");

            return citiesToReturn;
        }

        public static List<PlayerCityModel> GetAllCities(MySqlConnection pConnection)
        {
            List<PlayerCityModel> citiesToReturn = new List<PlayerCityModel>();

            //Query
            string query = string.Format(@"
            SELECT 
            pc.player_id playerId, pc.position position,
            cl.id id
            FROM 
            {0} pc, 
            {1} cl
            WHERE 
            pc.level_id = cl.id",
            Constants.TableName.PLAYER_CITY,         //pc
            Constants.TableName.CITY_LEVEL);         //cl

            pConnection.Query<PlayerCityModel, PlayerCityLevelModel, PlayerCityModel>(
                    query,
                    (playerCityModel, playerCityLevelModel) =>
                    {
                        //Add buildings to the list
                        playerCityModel.level = playerCityLevelModel;
                        citiesToReturn.Add(playerCityModel);

                        return playerCityModel;

                    }, splitOn: "id");

            return citiesToReturn;
        }

        public static PlayerCityModel GetCity(int pPlayerId, MySqlConnection pConnection)
        {
            //Query
            string query = string.Format(@"
            SELECT
            pc.player_id playerId, pc.position position,
            cl.id id
            FROM
            {0}
            pc INNER JOIN {1}
            cl ON(pc.level_id = cl.id)
            WHERE
            pc.player_id = {2}",
            Constants.TableName.PLAYER_CITY,
            Constants.TableName.CITY_LEVEL,        //cl
            pPlayerId);

            return pConnection.Query<PlayerCityModel>(
               query,
               new[] {
                    typeof(PlayerCityModel),
                    typeof(PlayerCityLevelModel),
               },
               objects =>
               {
                   PlayerCityModel playerCityModel = (PlayerCityModel)objects[0];
                   playerCityModel.level = (PlayerCityLevelModel)objects[1];
                   return playerCityModel;

               }, splitOn: "id").FirstOrDefault();
        }
    }
}
    

  
