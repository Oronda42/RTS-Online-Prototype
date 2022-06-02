using Dapper;
using MySql.Data.MySqlClient;
using RTS.Configuration;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Database
{
    public static class PlayerFactory
    {
        #region Properties


        #endregion

        #region Implementation

        /// <summary>
        /// Returns a player with the specified if
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static PlayerModel GetById(MySqlConnection pConnection, int pId)
        {
            //Build the query
            string query = string.Format(@"
                SELECT id, device_id DeviceId, nickname, password, creation, current_token CurrentToken
                FROM {0} p 
                WHERE p.id = {1}", Constants.TableName.PLAYER, pId);

            //Execute Query
            PlayerModel playerModel = pConnection.QueryFirstOrDefault<PlayerModel>(query);

            //Return the player
            return playerModel;
        }

        public static List<PlayerModel> GetAll(int pPlayerId, MySqlConnection pConnection)
        {
            List<PlayerModel> playersToReturn = new List<PlayerModel>();

            //Query
            string query = string.Format(@"
            SELECT
            id, nickname
            FROM 
            {0} p
            WHERE 
            p.id != {1}",
           Constants.TableName.PLAYER, pPlayerId);

            pConnection.Query(
                           query,
                           new[] {typeof(PlayerModel),},
                           objects =>
                           {
                               PlayerModel model = objects[0] as PlayerModel;
                               playersToReturn.Add(model);
                               return model;
                           });

            return playersToReturn;
        }

        /// <summary>
        /// Returns a player with the specified if
        /// </summary>
        /// <param name="pDeviceId"></param>
        /// <returns></returns>
        public static PlayerModel GetWithDeviceId(MySqlConnection pConnection, string pDeviceId)
        {
            //Build the query
            string query = string.Format(@"
                SELECT id, device_id DeviceId, nickname, password, creation, current_token CurrentToken
                FROM {0} p 
                WHERE p.device_id = '{1}'", 
                Constants.TableName.PLAYER, 
                pDeviceId);

            //Execute Query
            PlayerModel playerModel = pConnection.QueryFirstOrDefault<PlayerModel>(query);

            //Return the player
            return playerModel;
        }

        /// <summary>
        /// Use for Update Token at connection and deconnection
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="playerToken"></param>
        /// <param name="databaseConnection"></param>
        public static void UpdatePlayerTokenWithID(int pPlayerId, string pToken, MySqlConnection pConnection)
        {
            if (pConnection == null)
                throw new Exception("Connection is null");

            MySqlCommand updateCmd = pConnection.CreateCommand();

            //Update player base with new informations
            updateCmd.CommandText = string.Format(@"UPDATE
            {0} p
            SET p.current_token = '{1}'
            WHERE p.id = {2}",
            Constants.TableName.PLAYER,
            pToken,
            pPlayerId
            );

            //Execute query
            updateCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Create a player with a specified device id 
        /// </summary>
        /// <param name="pDeviceId"></param>
        public static PlayerModel CreateWithDeviceId(MySqlConnection pConnection, string pDeviceId)
        {
            MySqlTransaction transaction = pConnection.BeginTransaction();
             
            try
            {
                ///////////////////
                /// Create player
                MySqlCommand insertCmd = pConnection.CreateCommand();
                insertCmd.Transaction = transaction;

                //Query
                insertCmd.CommandText = string.Format(@"INSERT INTO {0}
                (device_id, creation)
                VALUES
                (@device_id, @creation)",
                Constants.TableName.PLAYER);

                //Set parameters
                insertCmd.Parameters.Add("@device_id", MySqlDbType.String).Value = pDeviceId;
                insertCmd.Parameters.Add("@creation", MySqlDbType.DateTime).Value = DateTime.Now;

                //Execute query
                insertCmd.ExecuteNonQuery();

                ///////////////////
                /// Get the player
                PlayerModel playerToCreate = GetWithDeviceId(pConnection, pDeviceId);

                if (playerToCreate == null)
                    throw new Exception(string.Format("Error when creating the player in {0}", Constants.TableName.PLAYER_BUILDING));

                ///////////////////
                /// Player bag
                playerToCreate.resourceBag = GetDefaultResource(pConnection);

                ///////////////////
                /// Create player market
                PlayerMarketModel playerMarket = PlayerMarketFactory.CreatePlayerMarket(transaction, playerToCreate);

                ///////////////////
                /// Create player map
                PlayerMapModel playerMap = new PlayerMapModel
                {
                    owner = playerToCreate,
                    Extents = new List<PlayerMapExtentModel>(),
                };

                ///////////////////
                /// Create player map extent 

                //Get extent model
                MapExtentModel firstMapExtent = MapExtentFactory.GetById(pConnection, 1);
                firstMapExtent.Elements = MapExtentFactory.GetElements(pConnection, firstMapExtent);

                //Create the player extent
                PlayerMapExtentModel playerMapExtent = new PlayerMapExtentModel
                {
                    Map = playerMap,
                    Extent = firstMapExtent,
                    Creation = DateTime.Now
                };

                ///////////////////
                /// Randomize map elements

                // Specific player elements
                playerMapExtent.PlayerElements = new List<PlayerMapExtentElementModel>();

                //Get all elements in database
                List<MapElementModel> elements = MapElementFactory.GetAllMapElements(pConnection);

                //Get all resource in database
                List<ResourceModel> resources = ResourceFactory.GetAllResources(pConnection);

                //For each random elements (with no entity id)
                for (int i = 0; i < firstMapExtent.Elements.Count; i++)
                {
                    if (firstMapExtent.Elements[i].Element.Type.Id == (int)TypeOfMapElement.RESOURCE)
                    {
                        ResourceModel randomResource = resources.Where(r => r.AvailableInPlayerMap == true).OrderBy(me => Guid.NewGuid()).FirstOrDefault();

                        //If there is a specific entity for this type
                        if (randomResource != null)
                        {
                            playerMapExtent.PlayerElements.Add(new PlayerMapExtentElementModel
                            {
                                Element = firstMapExtent.Elements[i].Element,
                                PlayerExtent = playerMapExtent,
                                EntityId = randomResource.id,
                                mapElementInstanceId = firstMapExtent.Elements[i].InstanceId
                            });
                        }

                    }
                }

                playerMap.Extents.Add(playerMapExtent);

                PlayerMapFactory.CreateWithTransaction(playerMap, transaction);


                ///////////////////
                /// Player Building center

                //Get map element for the player center
                MapExtentElementModel mapElementCenter = firstMapExtent.Elements.Where(e => e.Element.Type.Id == (int)TypeOfMapElement.PLAYER_BUILDING_CENTER).First();

                PlayerBuildingCenterModel playerCenter = PlayerBuildingCenterModel.CreateDefault(DateTime.Now, playerToCreate);
                playerCenter.mapExtentId = mapElementCenter.Extent.id;
                playerCenter.mapElementInstanceId = mapElementCenter.InstanceId;


                PlayerBuildingCenterFactory.CreateWithTransaction(playerCenter, transaction);
                //Add the bag to the player and update the database
                playerToCreate.resourceBag.AddBag(playerCenter.Level.persistentBag);


                /////////////////////////////////
                /// Create PlayerCity

                ////Ask Server For cities
                //List<PlayerCityModel> neighboors = PlayerCityFactory.GetAllCities(pConnection);

                //PlayerCityModel playerCityModel = PlayerCityModel.CreateDefault(DateTime.Now, playerToCreate, neighboors, LocaleSettings.NEIGHBOORS_RADIUS);

                //PlayerCityFactory.CreateWithTransaction(playerCityModel, transaction);

                ///////////////////////////////// 
                /// Resource bag
                //Create the bag into the database
                CreateResourceBagWithTransaction(transaction, playerToCreate);


                transaction.Commit();

                return playerToCreate;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                transaction.Rollback();
                return null;
            }
        }

        /// <summary>
        /// Update player's informations in database
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="nickname"></param>
        public static void UpdatePlayerInformations(MySqlConnection pConnection, PlayerModel pPlayer)
        {
            if (pConnection == null)
                throw new Exception("Connection is null");

            MySqlCommand updateCmd = pConnection.CreateCommand();

            //Update player base with new informations
            updateCmd.CommandText = string.Format(@"UPDATE
            {0} p
            SET p.nickname = '{1}', 
            p.current_token = '{2}'
            WHERE p.id = {3}",
            Constants.TableName.PLAYER,
            pPlayer.nickname,
            pPlayer.CurrentToken,
            pPlayer.id
            );

            //Execute query
            updateCmd.ExecuteNonQuery();
        }
        #endregion

        #region Player Resources


        public static void CreateResourceBagWithTransaction(MySqlTransaction pTransaction, PlayerModel pPlayer)
        {
            if (pTransaction == null)
                throw new Exception("Transaction is null");

            if (pPlayer.resourceBag.resources == null)
                throw new Exception("player resource bag is null");

            MySqlCommand sqlCmd = pTransaction.Connection.CreateCommand();

            //Update building base
            sqlCmd.CommandText = string.Format(@"DELETE FROM {0} WHERE player_id = {1}",
                Constants.TableName.PLAYER_RESOURCE_BAG,
                pPlayer.id
            );


            //Update each resources          
            for (int i = 0; i < pPlayer.resourceBag.resources.Count; i++)
            {
                sqlCmd = pTransaction.Connection.CreateCommand();

                //Update building base
                sqlCmd.CommandText = string.Format(@"
                INSERT INTO {0}
                (player_id, resource_id, amount, maximum, used)
                values({1}, {2}, {3}, {4}, {5})",
                Constants.TableName.PLAYER_RESOURCE_BAG,
                pPlayer.id,
                pPlayer.resourceBag.resources[i].resourceId,
                pPlayer.resourceBag.resources[i].amount,
                pPlayer.resourceBag.resources[i].maximum,
                pPlayer.resourceBag.resources[i].used
                );

                //Execute query
                sqlCmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Return the new default empty bag
        /// </summary>
        /// <returns></returns>
        public static ResourceBagModel GetDefaultResource(MySqlConnection pConnection)
        {
            string query = string.Format(@"
                SELECT r.id resourceId, prbd.amount, rt.default_maximum maximum, 0 as used
                FROM {0} prbd
                INNER JOIN {1} r ON (prbd.resource_id = r.id)
                INNER JOIN {2} rt ON (r.resource_type_id = rt.id)",
                Constants.TableName.PLAYER_RESOURCE_BAG_DEFAULT,
                Constants.TableName.RESOURCE,
                Constants.TableName.RESOURCE_TYPE);

            //Object to return
            ResourceBagModel resourceBagToReturn = new ResourceBagModel();
            resourceBagToReturn.id = -1;
            resourceBagToReturn.resources = pConnection.Query<ResourceBagSlotModel>(query).ToList();

            return resourceBagToReturn;
        }

        /// <summary>
        /// Returns all consumables resources for a player
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <returns></returns>
        public static ResourceBagModel GetResources(MySqlConnection pConnection, PlayerModel pPlayer)
        {
            string query = string.Format(@"Select
                r.id, r.name,
                prb.amount, prb.maximum, prb.used          
                from
                {0} r, {1} prb
                where
                r.id = prb.resource_id
                and prb.player_id = {2}",
                Constants.TableName.RESOURCE,
                Constants.TableName.PLAYER_RESOURCE_BAG,
                pPlayer.id);

            //Object to return
            ResourceBagModel resourceBagToReturn = null;

            pConnection.Query<ResourceModel, ResourceBagSlotModel, ResourceBagModel>(
                query,
                (resource, slot) =>
                {
                    if (resourceBagToReturn == null)
                    {
                        resourceBagToReturn = new ResourceBagModel();
                        resourceBagToReturn.resources = new List<ResourceBagSlotModel>();
                    }

                    //Add the resource
                    resourceBagToReturn.resources.Add(new ResourceBagSlotModel(resource.id, slot.amount, slot.maximum, slot.used));
                    return resourceBagToReturn;

                }, splitOn: "amount");

            return resourceBagToReturn;
        }

        /// <summary>
        /// Update the resource bag with a transaction
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <param name="pTransaction"></param>
        public static void UpdateResourceBagWithTransaction(PlayerModel pPlayer, MySqlTransaction pTransaction)
        {
            if (pTransaction == null)
                throw new Exception("Transaction is null");

            MySqlCommand updateCmd = pTransaction.Connection.CreateCommand();

            //Update each resources          
            for (int i = 0; i < pPlayer.resourceBag.resources.Count; i++)
            {
                //Update building base
                updateCmd.CommandText = string.Format(@"UPDATE
                {0} prb
                SET prb.amount = {1},
                prb.used = {2}
                WHERE prb.player_id = {3}
                AND prb.resource_id = {4}",
                Constants.TableName.PLAYER_RESOURCE_BAG,
                pPlayer.resourceBag.resources[i].amount,
                pPlayer.resourceBag.resources[i].used,
                pPlayer.id,
                pPlayer.resourceBag.resources[i].resourceId
                );

                //Execute query
                updateCmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update the resource bag
        /// </summary>
        /// <param name="pPlayer"></param>
        /// <param name="pTransaction"></param>
        public static void UpdateResourceBag(MySqlConnection pConnection, PlayerModel pPlayer)
        {
            //Update each resources          
            for (int i = 0; i < pPlayer.resourceBag.resources.Count; i++)
            {
                //Update building base
                string query = string.Format(@"UPDATE
                {0} prb
                SET prb.amount = {1},
                prb.used = {2}
                WHERE prb.player_id = {3}
                AND prb.resource_id = {4}",
                Constants.TableName.PLAYER_RESOURCE_BAG,
                pPlayer.resourceBag.resources[i].amount,
                pPlayer.resourceBag.resources[i].used,
                pPlayer.id,
                pPlayer.resourceBag.resources[i].resourceId
                );

                pConnection.Query(query);
            }

        }

        /// <summary>
        /// Returns true if the player has enough ressources for the specified cost. Otherwise false;
        /// </summary>
        /// <param name="pPlayerId"></param>
        /// <param name="pCostId"></param>
        /// <returns></returns>
        public static bool HasEnoughResourceForCost(MySqlConnection pConnection, int pPlayerId, int pCostId)
        {
            //Query
            string query = string.Format(@"
            SELECT prb.resource_id
            FROM
            {0} prb,
            {1} rb
            WHERE
            prb.resource_id = rb.resource_id
            and prb.amount < rb.amount
            and prb.player_id = {2}
            and rb.id = {3}",
            Constants.TableName.PLAYER_RESOURCE_BAG,
            Constants.TableName.RESOURCE_BAG,
            pPlayerId,
            pCostId);

            if (pConnection.Query<int>(query).Count() > 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Consume the specified cost id
        /// </summary>
        /// <param name="pCostId"></param>
        /// <returns></returns>
        public static bool ConsumeWithTransaction(int pPlayerId, int pCostId, MySqlTransaction pTransaction)
        {
            if (pTransaction == null)
                throw new Exception("Transaction is null");

            // If the player has no enough resource
            if (!HasEnoughResourceForCost(pTransaction.Connection, pPlayerId, pCostId))
                return false;

            MySqlCommand updateCmd = pTransaction.Connection.CreateCommand();

            //Query
            updateCmd.CommandText = string.Format(@"UPDATE
                {0} prb
                JOIN {1} rb ON rb.resource_id = prb.resource_id
                SET prb.amount = prb.amount - rb.amount
                WHERE prb.player_id = {2}
                AND rb.id = {3}",
            Constants.TableName.PLAYER_RESOURCE_BAG,
            Constants.TableName.RESOURCE_BAG,
            pPlayerId,
            pCostId
            );

            //Execute query
            updateCmd.ExecuteNonQuery();

            return true;
        }

        /// <summary>
        /// Consume the specified cost id
        /// </summary>
        /// <param name="pCostId"></param>
        /// <returns></returns>
        public static bool UseWithTransaction(int pPlayerId, int pCostId, MySqlTransaction pTransaction)
        {
            if (pTransaction == null)
                throw new Exception("Transaction is null");

            // If the player has no enough resource
            if (!HasEnoughResourceForCost(pTransaction.Connection, pPlayerId, pCostId))
                return false;

            MySqlCommand updateCmd = pTransaction.Connection.CreateCommand();

            //Query
            updateCmd.CommandText = string.Format(@"UPDATE
                {0} prb
                JOIN {1} rb ON rb.resource_id = prb.resource_id
                SET prb.used = prb.used + rb.amount                
                WHERE prb.player_id = {2}
                AND rb.id = {3}",
            Constants.TableName.PLAYER_RESOURCE_BAG,
            Constants.TableName.RESOURCE_BAG,
            pPlayerId,
            pCostId
            );

            //Execute query
            updateCmd.ExecuteNonQuery();

            return true;
        }

        /// <summary>
        /// Consume the specified cost id
        /// </summary>
        /// <param name="pCostId"></param>
        /// <returns></returns>
        public static bool UnuseWithTransaction(int pPlayerId, int pCostId, MySqlTransaction pTransaction)
        {
            if (pTransaction == null)
                throw new Exception("Transaction is null");

            MySqlCommand updateCmd = pTransaction.Connection.CreateCommand();

            //Query
            updateCmd.CommandText = string.Format(@"UPDATE
                {0} prb
                JOIN {1} rb ON rb.resource_id = prb.resource_id
                SET prb.used = prb.used - rb.amount
                WHERE prb.player_id = {2}
                AND rb.id = {3}",
            Constants.TableName.PLAYER_RESOURCE_BAG,
            Constants.TableName.RESOURCE_BAG,
            pPlayerId,
            pCostId
            );

            //Execute query
            updateCmd.ExecuteNonQuery();

            return true;
        }

        #endregion
    }
}