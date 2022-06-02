using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Database
{
    public static class BuildingStateFactory
    {

        #region Properties

        #endregion

        /// <summary>
        /// Returns all buildings producer available in the game
        /// </summary>
        /// <returns></returns>
        public static List<BuildingStateModel> GetAllStates(MySqlConnection pConnection)
        {
            string query = string.Format(@"SELECT
                bs.id, bs.name, bs.can_produce canProduce, bs.can_activate canActivate, bs.can_repair canRepair
                FROM 
                {0} bs",
            Constants.TableName.BUILDING_STATE);

            //Object to return
            List<BuildingStateModel> statesToReturn = null;

            statesToReturn = pConnection.Query<BuildingStateModel>(query).ToList();

            return statesToReturn;
        }
    }
}
