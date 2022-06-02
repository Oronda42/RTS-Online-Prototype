using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System.Collections.Generic;
using System.Linq;


namespace RTS.Database
{
    public static class MapElementFactory
    {

        #region Element types

        /// <summary>
        /// Returns all elements type available in the game
        /// </summary>
        /// <returns></returns>
        public static List<MapElementTypeModel> GetAllMapElementTypes(MySqlConnection pConnection)
        {
            string query = string.Format(@"SELECT
                met.id, met.name
                FROM 
                {0} met",
            Constants.TableName.MAP_ELEMENT_TYPE);

            //Object to return
            List<MapElementTypeModel> elementTypes = pConnection.Query<MapElementTypeModel>(query).ToList();

            return elementTypes;
        }

        #endregion

        #region Elements

        /// <summary>
        /// Get all map elements in the game
        /// </summary>
        /// <returns></returns>
        public static List<MapElementModel> GetAllMapElements(MySqlConnection pConnection)
        {
            string query = string.Format(@"SELECT
                me.id, me.name,
                met.id, met.name
                FROM 
                {0} me INNER JOIN {1} met ON (me.map_element_type_id = met.id)",
            Constants.TableName.MAP_ELEMENT,
            Constants.TableName.MAP_ELEMENT_TYPE);

            //map element to be retrieved from database
            MapElementModel currentMapElement = new MapElementModel();

            List<MapElementModel> mapElements = new List<MapElementModel>();
            mapElements = pConnection.Query(
                query,
                new[] {
                    typeof(MapElementModel),
                    typeof(MapElementTypeModel)},
                objects =>
                {
                    currentMapElement = objects[0] as MapElementModel;
                    currentMapElement.Type = objects[1] as MapElementTypeModel;

                    return currentMapElement;
                },
                splitOn: "id")
                .ToList();


            return mapElements;
        }

        #endregion

    }
}
