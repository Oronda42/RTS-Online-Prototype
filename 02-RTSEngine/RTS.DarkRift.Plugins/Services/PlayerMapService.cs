using MySql.Data.MySqlClient;
using RTS.Configuration;
using RTS.Database;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Plugins
{
    public static class PlayerMapService
    {
        #region Properties

        /// <summary>
        /// List of player map extents
        /// </summary>
        public static List<MapExtentModel> MapExtents;


        /// <summary>
        /// List of all elements
        /// </summary>
        public static List<MapElementModel> MapElements;

        #endregion

        public static void Init()
        {
            MySqlConnection connection = DatabaseConnector.GetNewConnection();
            
            MapExtents = MapExtentFactory.GetAllMapExtent(connection);

            foreach(MapExtentModel me in MapExtents)
            {
                me.Elements = MapExtentFactory.GetElements(connection, me); 
                Console.WriteLine("[RTS] INFO : Map extent N°" + me.id + " totally loaded");
            }

            Console.WriteLine("[RTS] INFO : PlayerMapService initialized successfully"); 

            connection.Close();
        }

        /// <summary>
        /// Returns a MapElementModel with an entity id specified
        /// </summary>
        /// <param name="pTypeId"></param>
        /// <returns></returns>
        public static int GetRandomElementEntityByTypeId(int pTypeId)
        {
            switch (pTypeId)
            {
                case (int)TypeOfMapElement.RESOURCE:
                    return ResourceData.GetAllResources().OrderBy(me => Guid.NewGuid()).FirstOrDefault().id;

                default:
                    return 0;
            }

        }
    }
}
