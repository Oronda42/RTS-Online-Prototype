using Dapper;
using MySql.Data.MySqlClient;
using RTS.Database;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Database
{
    public class MapExtentFactory
    {

        #region Implementation


        /// <summary>
        /// Returns a map extent with the specified if
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static List<MapExtentModel> GetAllMapExtent(MySqlConnection pConnection)
        {
            //Build the query
            string query = string.Format(@"SELECT 
            me.id, me.name, me.width, me.depth 
            FROM {0} me",
            Constants.TableName.MAP_EXTENT);

            //Execute Query
            List<MapExtentModel> mapExtents = pConnection.Query<MapExtentModel>(query).ToList();

            //Return map extents
            return mapExtents;
        }


        /// <summary>
        /// Returns a map extent with the specified if
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static MapExtentModel GetById(MySqlConnection pConnection, int pId)
        {
            //Build the query
            string query = string.Format(@"SELECT 
            me.id, me.name, me.width, me.depth 
            FROM {0} me 
            WHERE me.id = {1}", 
            Constants.TableName.MAP_EXTENT, 
            pId);

            //Execute Query
            MapExtentModel mapExtent = pConnection.QueryFirstOrDefault<MapExtentModel>(query);

            //Return the map extent
            return mapExtent;
        }

        /// <summary>
        /// Returns elements of a map extent
        /// </summary>
        /// <param name="pMapExtentId"></param>
        /// <returns></returns>
        public static List<MapExtentElementModel> GetElements(MySqlConnection pConnection, MapExtentModel pMapExtent)
        {
            //Build the query
            string query = string.Format(@"SELECT 
            mee.position, mee.entity_id EntityId, mee.instance_id InstanceId,
            me.id, me.name,
            met.id, met.name 
            FROM {0} mee INNER JOIN {1} me ON (mee.map_element_id = me.id)
            INNER JOIN {2} met ON (me.map_element_type_id = met.id)
            WHERE mee.map_extent_id = {3}",
            Constants.TableName.MAP_EXTENT_ELEMENT,
            Constants.TableName.MAP_ELEMENT,
            Constants.TableName.MAP_ELEMENT_TYPE,
            pMapExtent.id);

            //map element to be retrieved from database
            MapExtentElementModel currentMapExtentElement = new MapExtentElementModel();

            //Execute Query
            List<MapExtentElementModel> elements = new List<MapExtentElementModel>();
            elements = pConnection.Query(
                query,
                new[] {
                    typeof(MapExtentElementModel),
                    typeof(MapElementModel),
                    typeof(MapElementTypeModel)},
                objects =>
                {
                    currentMapExtentElement = objects[0] as MapExtentElementModel;
                    currentMapExtentElement.Element = objects[1] as MapElementModel;
                    currentMapExtentElement.Element.Type = objects[2] as MapElementTypeModel;
                    currentMapExtentElement.Extent = pMapExtent;

                    return currentMapExtentElement;

                },
                splitOn: "id,id")
                .ToList();

            return elements;
        }



        ///// <summary>
        ///// Returns a map extent with the specified if
        ///// </summary>
        ///// <param name="pId"></param>
        ///// <returns></returns>
        //public static MapExtentModel GetRandomExtent()
        //{
        //    //Get the connection
        //    MySqlConnection connection = DatabaseConnector.GetConnection(true);

        //    //Build the query
        //    string query = string.Format("SELECT * FROM {0} me ORDER BY RAND() LIMIT 1;", MAP_EXTENT_TABLE_NAME);

        //    //Execute Query
        //    MapExtentModel mapExtent = connection.QueryFirstOrDefault<MapExtentModel>(query);

        //    //Return the player
        //    return mapExtent;
        //}

        #endregion
    }
}