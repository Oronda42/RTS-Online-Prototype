using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Database
{
    public static class BuildingProducerFactory
    {
        #region Properties


        #endregion

        #region Implementation

        /// <summary>
        /// Returns all buildings producer available in the game
        /// </summary>
        /// <returns></returns>
        public static List<BuildingProducerModel> GetAllBuildings(MySqlConnection pConnection)
        {
            string query = string.Format(@"SELECT
                b.id, b.name, b.construction_time constructionTime, bp.default_resource_id_produced defaultResourceIdProduced,
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
            Constants.TableName.BUILDING_PRODUCER,
            Constants.TableName.RESOURCE_BAG);

            //Object to return
            List<BuildingProducerModel> buildingsToReturn = null;

            //Building to be retrieved from database
            BuildingProducerModel currentBuildingParsing = new BuildingProducerModel();


            pConnection.Query<BuildingProducerModel, ResourceBagModel, ResourceBagSlotModel, BuildingTypeModel, List<BuildingProducerModel>>(
                query,
                (buildingProducer, resourceBag, costLine, buildingType) =>
                {
                    //First result parsing, initialization
                    if (buildingsToReturn == null)
                        buildingsToReturn = new List<BuildingProducerModel>();

                    //Try to get the building in the list
                    currentBuildingParsing = buildingsToReturn.FirstOrDefault(b => b.id == buildingProducer.id);


                    if (currentBuildingParsing == null)
                    {
                        currentBuildingParsing = buildingProducer;
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
                buildingsToReturn[i].Levels = BuildingProducerFactory.GetBuildingProducerLevels(pConnection, buildingsToReturn[i].id);
            }

            return buildingsToReturn;
        }

        /// <summary>
        /// Return the complete list of production levels 
        /// </summary>
        /// <param name="pBuildingId"></param>
        /// <returns></returns>
        public static List<BuildingProducerLevelModel> GetBuildingProducerLevels(MySqlConnection pConnection, int pBuildingId)
        {
            string query = string.Format(@"SELECT
                bpl.resource_id_produced resourceIdProduced, bpl.level_id id, bpl.seconds_to_produce secondsToProduce, bpl.amount_produced amountProduced,
                bl.resource_cost_id, 
                bpl.persistent_resource_bag_id_needed,
                bpl.consumption_resource_bag_id
                FROM
                {0} bpl INNER JOIN {1} bl ON (bpl.building_id = bl.building_id AND bpl.level_id = bl.level_id)
                WHERE bpl.building_id = {2}",
            Constants.TableName.BUILDING_PRODUCER_LEVEL,
            Constants.TableName.BUILDING_LEVEL_COST,
            pBuildingId);

            //Object to return
            List<BuildingProducerLevelModel> levels = new List<BuildingProducerLevelModel>(); ;
            BuildingProducerLevelModel levelParsing = null;

            pConnection.Query<BuildingProducerLevelModel, int, int, int, List<BuildingProducerLevelModel>>(
                query,
                (level, costBagId, persistentBagIdNeeded, consumptionBag) =>
                {
                    levelParsing = level;
                    levelParsing.persistentBagNeeded = new ResourceBagModel { id = persistentBagIdNeeded };
                    levelParsing.consumptionBag = new ResourceBagModel { id = consumptionBag };
                    levelParsing.cost = new ResourceBagModel { id = costBagId };
                    levels.Add(levelParsing);

                    return null;
                },
                splitOn: "resource_cost_id,persistent_resource_bag_id_needed,consumption_resource_bag_id"
                );

            // Get resources bags
            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].persistentBagNeeded = ResourceFactory.GetResourceBag(pConnection, levels[i].persistentBagNeeded.id);
                levels[i].consumptionBag = ResourceFactory.GetResourceBag(pConnection, levels[i].consumptionBag.id);
                levels[i].cost = ResourceFactory.GetResourceBag(pConnection, levels[i].cost.id);
            }

            return levels;
        }

        #endregion
    }
}