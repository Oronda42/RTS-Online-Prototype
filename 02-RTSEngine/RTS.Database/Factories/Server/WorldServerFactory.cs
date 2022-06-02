using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models.Server;

namespace RTS.Database
{
    public static class WorldServerFactory
    {

        #region Implementation

        /// <summary>
        /// Returns all zone for all servers in an environment
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static List<GameServerZoneModel> GetAllGameServerZone(MySqlConnection pConnection, string pEnvironment)
        {
            //Build the query
            string query = string.Format(@"SELECT 
            gsz.server_name ServerName, gsz.environment, gsz.world_zone_id WorldZoneId
            FROM {0} gsz
            WHERE gsz.environment = '{1}'",
            Constants.TableName.GAME_SERVER_ZONE,
            pEnvironment);

            //Execute Query
            List<GameServerZoneModel> gameServerZoneList = pConnection.Query<GameServerZoneModel>(query).ToList();

            //Return map extents
            return gameServerZoneList;
        }

        #endregion
    }
}
