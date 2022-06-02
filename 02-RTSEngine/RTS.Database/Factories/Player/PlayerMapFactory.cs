using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Database
{
    public class PlayerMapFactory
    {
        #region Implementation

        /// <summary>
        /// Insert a new map extent
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static void CreateWithTransaction(PlayerMapModel pMap, MySqlTransaction pTransaction)
        {
            if (pTransaction == null)
                throw new Exception("Transaction is null");

            ///////////////////////////
            /// Player map

            //Create insert command    
            MySqlCommand insertCmd = pTransaction.Connection.CreateCommand();

            //Build the query
            insertCmd.CommandText = string.Format(@"
                    INSERT INTO {0} (player_id, name) 
                    SELECT {1},'{2}' FROM DUAL
                    WHERE NOT EXISTS(
                        SELECT 1
                        FROM {0}
                        WHERE player_id = {1}
                    )
                    LIMIT 1;",
            Constants.TableName.PLAYER_MAP,
            pMap.owner.id,
            pMap.name);

            //Execute query
            insertCmd.ExecuteNonQuery();

            ///////////////////////////
            /// Player map extent

            insertCmd = pTransaction.Connection.CreateCommand();

            //Build the query
            insertCmd.CommandText = string.Format(@"
                    INSERT INTO {0} (player_id, map_extent_id, creation) 
                    VALUES
                    ({1},{2},@creation)",
            Constants.TableName.PLAYER_MAP_EXTENT,
            pMap.owner.id,
            pMap.Extents[0].Extent.id);

            insertCmd.Parameters.Add("@creation", MySqlDbType.DateTime).Value = DateTime.Now;

            //Execute query
            insertCmd.ExecuteNonQuery();

            ///////////////////////////
            /// Player map extent elements
            
            for (int i = 0; i < pMap.Extents[0].PlayerElements.Count; i++)
            {
                insertCmd = pTransaction.Connection.CreateCommand();

                //Build the query
                insertCmd.CommandText = string.Format(@"
                    INSERT INTO {0} (player_id, map_extent_id, map_element_id, entity_id, map_element_instance_id) 
                    VALUES
                    ({1},{2},{3},{4},{5})",
                Constants.TableName.PLAYER_MAP_EXTENT_ELEMENT,
                pMap.owner.id,
                pMap.Extents[0].Extent.id,
                pMap.Extents[0].PlayerElements[i].Element.Id,
                pMap.Extents[0].PlayerElements[i].EntityId,
                pMap.Extents[0].PlayerElements[i].mapElementInstanceId);
                
                //Execute query
                insertCmd.ExecuteNonQuery();
            }


        }

        /// <summary>
        /// Return the entire map of the player
        /// </summary>
        /// <param name="pPlayerId"></param>
        /// <returns></returns>
        public static PlayerMapModel GetByPlayerId(MySqlConnection pConnection, int pPlayerId)
        {
            string query = string.Format(@"Select 
                pm.player_id, pm.name,
                me.id, me.name, me.width, me.depth,
                pme.creation
                from 
                {0} pm, {1} me, {2} pme 
                where 
                pm.player_id = pme.player_id 
                and pme.map_extent_id = me.id 
                and pm.player_id = {3}",
                Constants.TableName.PLAYER_MAP,
                Constants.TableName.MAP_EXTENT,
                Constants.TableName.PLAYER_MAP_EXTENT, 
                pPlayerId);

            //Create the map
            PlayerMapModel playerMapToReturn = null;

            pConnection.Query<PlayerMapModel, MapExtentModel, PlayerMapExtentModel, PlayerMapModel>(
                query,
                (playerMap, mapExtent, playerMapExtent) =>
                {
                    if (playerMapToReturn == null)
                    {
                        playerMapToReturn = playerMap;
                        playerMapToReturn.owner = new PlayerModel { id = pPlayerId };
                        playerMapToReturn.Extents = new List<PlayerMapExtentModel>();
                    }

                    //Add the extent
                    playerMapExtent.Map = playerMapToReturn;
                    playerMapExtent.Extent = mapExtent;

                    playerMapToReturn.Extents.Add(playerMapExtent);

                    return null;

                }, splitOn: "id,creation");

            return playerMapToReturn;

        }

        /// <summary>
        /// Returns elements of a map extent
        /// </summary>
        /// <param name="pMapExtentId"></param>
        /// <returns></returns>
        public static List<PlayerMapExtentElementModel> GetPlayerElements(MySqlConnection pConnection, PlayerMapExtentModel pPlayerMapExtent)
        {
            //Build the query
            string query = string.Format(@"SELECT 
            pmee.map_element_id,  
            pmee.entity_id EntityId, pmee.map_element_instance_id mapElementInstanceId
            FROM {0} pmee 
            WHERE pmee.map_extent_id = {1}
            AND pmee.player_id = {2}",
            Constants.TableName.PLAYER_MAP_EXTENT_ELEMENT,
            pPlayerMapExtent.Extent.id,
            pPlayerMapExtent.Map.owner.id);

            //map element to be retrieved from database
            PlayerMapExtentElementModel currentMapExtentElement = new PlayerMapExtentElementModel();

            //Execute Query
            List<PlayerMapExtentElementModel> elements = new List<PlayerMapExtentElementModel>();
            elements = pConnection.Query(
                query,
                new[] {
                    typeof(int),
                    typeof(PlayerMapExtentElementModel),
                },
                objects =>
                {
                    currentMapExtentElement = objects[1] as PlayerMapExtentElementModel;
                    currentMapExtentElement.PlayerExtent = pPlayerMapExtent;

                    MapExtentElementModel mapExtentElement = pPlayerMapExtent.Extent.Elements.Where(e => e.Element.Id == Convert.ToInt32(objects[0])).FirstOrDefault();
                    if (mapExtentElement != null)
                        currentMapExtentElement.Element = mapExtentElement.Element;

                    return currentMapExtentElement;

                },
                splitOn: "EntityId")
                .ToList();

            return elements;
        }

        /// <summary>
        /// Create a map Extent
        /// </summary>
        /// <param name="pMapextentId"></param>
        /// <param name="pPositionOnMap"></param>
        public static void CreateMapExtent(MySqlConnection pConnection, int pPlayerId, int pMapextentId)
        {
            //Query
            string query = string.Format(@"
            INSERT INTO {0} (player_id, map_extent_id, creation) 
            VALUES
            ({1},{2}EntityId,NOW())",
            Constants.TableName.PLAYER_MAP_EXTENT,
            pPlayerId,
            pMapextentId);

            pConnection.Query(query);
        }


        #endregion
    }
}
