using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Database
{
    public class ResourceFactory
    {

        /// <summary>
        /// Returns a random resource
        /// </summary>
        /// <returns></returns>
        public static ResourceModel GetRandomResource(MySqlConnection pConnection)
        {
            string query = string.Format(@"SELECT 
                r.id, r.name, r.available_in_player_map AvailableInPlayerMap,
                rt.id, rt.name
                FROM 
                {0} r
                INNER JOIN {1} rt ON (r.resource_type_id = rt.id)
                ORDER BY RAND()",
                Constants.TableName.RESOURCE,
                Constants.TableName.RESOURCE_TYPE);

            //Object to return
            ResourceModel resourcesToReturn = pConnection.Query<ResourceModel>(
                query,
                new[] { typeof(ResourceModel), typeof(ResourceTypeModel) },
                objects =>
                {
                    ResourceModel resource = (ResourceModel)objects[0];
                    ResourceTypeModel resourceType = (ResourceTypeModel)objects[1];

                    //Set the resource type
                    resource.type = resourceType;
                    return resource;
                },
                splitOn: "id").FirstOrDefault();

            return resourcesToReturn;
        }


        /// <summary>
        /// Returns all resource available in the game
        /// </summary>
        /// <returns></returns>
        public static List<ResourceModel> GetAllResources(MySqlConnection pConnection)
        {
            string query = string.Format(@"SELECT 
                r.id, r.name, r.available_in_player_map AvailableInPlayerMap,
                rt.id, rt.name
                FROM 
                {0} r
                INNER JOIN {1} rt ON (r.resource_type_id = rt.id)",
                Constants.TableName.RESOURCE,
                Constants.TableName.RESOURCE_TYPE);

            //Object to return
            List<ResourceModel> resourcesToReturn = pConnection.Query<ResourceModel>(
                query,
                new[] { typeof(ResourceModel), typeof(ResourceTypeModel) },
                objects =>
                {
                    ResourceModel resource = (ResourceModel)objects[0];
                    ResourceTypeModel resourceType = (ResourceTypeModel)objects[1];

                    //Set the resource type
                    resource.type = resourceType;
                    return resource;
                },
                splitOn:"id").ToList();

            return resourcesToReturn;
        }

        /// <summary>
        /// Returns a bag of resource
        /// </summary>
        /// <param name="pCostId"></param>
        /// <returns></returns>
        public static ResourceBagModel GetResourceBag(MySqlConnection pConnection, int pBagId)
        {
            string query = string.Format(@"SELECT 
                rb.resource_id resourceId, rb.amount
                FROM 
                {0} rb
                WHERE rb.id = {1}",
                Constants.TableName.RESOURCE_BAG,
                pBagId);

            //Object to return
            List<ResourceBagSlotModel> resourcesToReturn = pConnection.Query<ResourceBagSlotModel>(query).ToList();
            ResourceBagModel bagCost = new ResourceBagModel
            {
                id = pBagId,
                resources = resourcesToReturn
            };

            return bagCost;
        }

        
    }
}