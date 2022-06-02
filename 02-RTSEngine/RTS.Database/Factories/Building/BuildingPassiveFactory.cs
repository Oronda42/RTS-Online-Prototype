using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Database
{
    public static class BuildingPassiveFactory
    {
        #region Implementation

        /// <summary>
        /// Returns all buildings producer available in the game
        /// </summary>
        /// <returns></returns>
        public static List<BuildingPassiveModel> GetAllBuildings(MySqlConnection pConnection)
        {
            string query = string.Format(@"SELECT
                b.id, b.name, b.construction_time constructionTime,
                rb_creation.id, 
                rb_creation.resource_id resourceId, rb_creation.amount, 
                b.building_type_id id
                FROM 
                {0} b,
                {1} bp,
                {2} rb_creation
                WHERE
                b.id = bp.building_id
                AND b.resource_cost_id = rb_creation.id
                ORDER BY b.id, rb_creation.resource_id",
            Constants.TableName.BUILDING,
            Constants.TableName.BUILDING_PASSIVE,
            Constants.TableName.RESOURCE_BAG);

            //Object to return
            List<BuildingPassiveModel> buildingsToReturn = null;

            //Building to be retrieved from database
            BuildingPassiveModel currentBuildingParsing = new BuildingPassiveModel();


            pConnection.Query<BuildingPassiveModel, ResourceBagModel, ResourceBagSlotModel, BuildingTypeModel, List<BuildingPassiveModel>>(
                query,
                (buildingPassive, resourceBag, costLine, buildingType) =>
                {
                    //First result parsing, initialization
                    if (buildingsToReturn == null)
                        buildingsToReturn = new List<BuildingPassiveModel>();

                    //Try to get the building in the list
                    currentBuildingParsing = buildingsToReturn.FirstOrDefault(b => b.id == buildingPassive.id);


                    if (currentBuildingParsing == null)
                    {
                        currentBuildingParsing = buildingPassive;
                        currentBuildingParsing.cost = resourceBag;
                        currentBuildingParsing.cost.resources.Add(costLine);
                        currentBuildingParsing.buildingType = buildingType;

                        //Add to the list to return
                        buildingsToReturn.Add(currentBuildingParsing);
                    }
                    else
                    {
                        currentBuildingParsing.cost.resources.Add(costLine);
                    }
                    return null;
                },
                splitOn: "id,resourceId,id"
                );

            //Get levels data
            for (int i = 0; i < buildingsToReturn.Count; i++)
            {
                buildingsToReturn[i].Levels = BuildingPassiveFactory.GetBuildingPassiveLevels(pConnection, buildingsToReturn[i].id);
            }           

            return buildingsToReturn;
        }

        /// <summary>
        /// Return the complete list of levels for a passive building
        /// </summary>
        /// <param name="pBuildingId"></param>
        /// <returns></returns>
        public static List<BuildingPassiveLevelModel> GetBuildingPassiveLevels(MySqlConnection pConnection, int pBuildingId)
        {
            string query = string.Format(@"SELECT
                bpl.level_id id, bpl.seconds_to_consume secondsToConsume,
                bpl.persistent_resource_bag_id_needed, 
                bpl.persistent_resource_bag_id, 
                bpl.consumption_resource_bag_id,
                bl.resource_cost_id
                FROM
                {0} bpl INNER JOIN {1} bl ON (bpl.building_id = bl.building_id AND bpl.level_id = bl.level_id)
                WHERE bpl.building_id = {2}",
            Constants.TableName.BUILDING_PASSIVE_LEVEL,
            Constants.TableName.BUILDING_LEVEL_COST,
            pBuildingId);

            //Object to return
            List<BuildingPassiveLevelModel> levels = new List<BuildingPassiveLevelModel>(); ;
            BuildingPassiveLevelModel levelParsing = null;

            pConnection.Query<BuildingPassiveLevelModel, int, int, int, int, List<BuildingPassiveLevelModel>>(
                query,
                (level, persistentBagIdNeeded, persistentBagId, consumptionBagId, resourceCostBagId) =>
                {
                    levelParsing = level;
                    levelParsing.persistentBagNeeded = new ResourceBagModel { id = persistentBagIdNeeded };
                    levelParsing.persistentBag = new ResourceBagModel { id = persistentBagId };
                    levelParsing.consumptionBag = new ResourceBagModel { id = consumptionBagId };
                    levelParsing.cost = new ResourceBagModel { id = resourceCostBagId };
                    levels.Add(levelParsing);

                    return null;
                },
                splitOn: "persistent_resource_bag_id_needed,persistent_resource_bag_id,consumption_resource_bag_id,resource_cost_id"
                );

            // Get resources bags
            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].persistentBagNeeded = ResourceFactory.GetResourceBag(pConnection, levels[i].persistentBagNeeded.id);
                levels[i].persistentBag = ResourceFactory.GetResourceBag(pConnection, levels[i].persistentBag.id);
                levels[i].consumptionBag = ResourceFactory.GetResourceBag(pConnection, levels[i].consumptionBag.id);
                levels[i].cost = ResourceFactory.GetResourceBag(pConnection, levels[i].cost.id);
            }

            return levels;
        }


        #endregion
    }
}
