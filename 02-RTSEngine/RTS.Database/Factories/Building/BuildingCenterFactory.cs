using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Database
{
    public static class BuildingCenterFactory
    {
        #region Implementation

        /// <summary>
        /// Returns all buildings producer available in the game
        /// </summary>
        /// <returns></returns>
        public static BuildingCenterModel GetById(MySqlConnection pConnection, int pId)
        {
            string query = string.Format(@"SELECT
                b.id, b.name, b.construction_time constructionTime,
                b.resource_cost_id, 
                b.building_type_id id
                FROM 
                {0} b,
                {1} bc
                WHERE
                b.id = bc.building_id
                AND b.id = {2}
                ORDER BY b.id",
            Constants.TableName.BUILDING,
            Constants.TableName.BUILDING_CENTER,
            pId);

            //Building to be retrieved from database
            BuildingCenterModel currentBuildingParsing = new BuildingCenterModel();

            pConnection.Query(
                query,
                new[] {
                    typeof(BuildingCenterModel),
                    typeof(int),
                    typeof(BuildingTypeModel) },
                objects =>
                {
                    currentBuildingParsing = objects[0] as BuildingCenterModel;
                    currentBuildingParsing.cost = new ResourceBagModel { id = Convert.ToInt32(objects[1]) };
                    currentBuildingParsing.buildingType = objects[2] as BuildingTypeModel;

                    return currentBuildingParsing;
                },
                splitOn: "resource_cost_id, id");

            currentBuildingParsing.cost = ResourceFactory.GetResourceBag(pConnection, currentBuildingParsing.cost.id);
            currentBuildingParsing.Levels = GetBuildingCenterLevels(pConnection, currentBuildingParsing.id);
            
            return currentBuildingParsing;
        }


        /// <summary>
        /// Returns all buildings producer available in the game
        /// </summary>
        /// <returns></returns>
        public static List<BuildingCenterModel> GetAllBuildings(MySqlConnection pConnection)
        {
            string query = string.Format(@"SELECT
                b.id, b.name, b.construction_time constructionTime,
                b.resource_cost_id, 
                b.building_type_id id
                FROM 
                {0} b,
                {1} bc
                WHERE
                b.id = bc.building_id
                ORDER BY b.id",
            Constants.TableName.BUILDING,
            Constants.TableName.BUILDING_CENTER);

            //Object to return
            List<BuildingCenterModel> buildingsToReturn = null;

            //Building to be retrieved from database
            BuildingCenterModel currentBuildingParsing = new BuildingCenterModel();

            buildingsToReturn = pConnection.Query(
                query,
                new[] {
                    typeof(BuildingCenterModel),
                    typeof(int),
                    typeof(BuildingTypeModel) },
                objects =>
                {
                    currentBuildingParsing = objects[0] as BuildingCenterModel;
                    currentBuildingParsing.cost = new ResourceBagModel { id = Convert.ToInt32(objects[1]) };
                    currentBuildingParsing.buildingType = objects[2] as BuildingTypeModel;

                    return currentBuildingParsing;
                },
                splitOn: "resource_cost_id, id").ToList();


            //Get levels data
            for (int i = 0; i < buildingsToReturn.Count; i++)
            {
                buildingsToReturn[i].cost = ResourceFactory.GetResourceBag(pConnection, buildingsToReturn[i].cost.id);
                buildingsToReturn[i].Levels = GetBuildingCenterLevels(pConnection, buildingsToReturn[i].id);
            }

            return buildingsToReturn;
        }

        /// <summary>
        /// Return the complete list of levels for a passive building
        /// </summary>
        /// <param name="pBuildingId"></param>
        /// <returns></returns>
        public static List<BuildingCenterLevelModel> GetBuildingCenterLevels(MySqlConnection pConnection, int pBuildingId)
        {
            string query = string.Format(@"SELECT
                bcl.level_id id,
                bcl.persistent_resource_bag_id, 
                bl.resource_cost_id
                FROM
                {0} bcl INNER JOIN {1} bl ON (bcl.building_id = bl.building_id AND bcl.level_id = bl.level_id)
                WHERE bcl.building_id = {2}",
            Constants.TableName.BUILDING_CENTER_LEVEL,
            Constants.TableName.BUILDING_LEVEL_COST,
            pBuildingId);

            //Object to return
            List<BuildingCenterLevelModel> levels = new List<BuildingCenterLevelModel>(); ;
            BuildingCenterLevelModel levelParsing = null;

            levels = pConnection.Query(
                query,
                new[] {
                    typeof(BuildingCenterLevelModel),
                    typeof(int),
                    typeof(int) },
                objects =>
                {
                    levelParsing = objects[0] as BuildingCenterLevelModel;
                    levelParsing.persistentBag = new ResourceBagModel { id = Convert.ToInt32(objects[1]) };
                    levelParsing.cost = new ResourceBagModel { id = Convert.ToInt32(objects[2]) };

                    return levelParsing;
                },
                splitOn: "persistent_resource_bag_id,resource_cost_id"
                ).ToList();

            // Get resources bags
            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].persistentBag = ResourceFactory.GetResourceBag(pConnection, levels[i].persistentBag.id);
                levels[i].cost = ResourceFactory.GetResourceBag(pConnection, levels[i].cost.id);
            }

            return levels;
        }


        #endregion
    }
}
