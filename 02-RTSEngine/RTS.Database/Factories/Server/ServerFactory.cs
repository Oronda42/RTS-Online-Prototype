using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Database
{
    public static class ServerFactory
    {
        #region Properties


        #endregion

        #region Implementation

        /// <summary>
        /// Use to fill database with this new server reference
        /// </summary>
        /// <param name="pConnection"></param>
        /// <param name="pListener"></param>
        /// <param name="pServerName"></param>
        /// <param name="pEnvironment"></param>
        /// <param name="pToken"></param>
        /// <returns></returns>
        public static bool CreateOrUpdateServerInstance(MySqlConnection pConnection, ServerInstanceModel pInstance)
        {
            try
            {
                MySqlCommand insertOrUpdateCmd = pConnection.CreateCommand();

                insertOrUpdateCmd.CommandText = string.Format(@"SELECT * FROM
                {0} t
                WHERE (t.name = '{1}' AND t.environment = '{2}')",
                Constants.TableName.SERVER_INSTANCE,
                pInstance.Name,
                pInstance.Environment);

                //Already exist
                Object affectedRow = insertOrUpdateCmd.ExecuteScalar();
                if (affectedRow != null)
                {
                    insertOrUpdateCmd.CommandText = string.Format(@"UPDATE
                    {0} t 
                    SET t.name = '{1}', 
                    t.environment = '{2}',
                    t.host = '{3}',
                    t.port = '{4}',
                    t.token = '{5}'
                    WHERE (t.name = '{1}' AND t.environment = '{2}')
                    ",
                    Constants.TableName.SERVER_INSTANCE,
                    pInstance.Name,
                    pInstance.Environment,
                    pInstance.Host,
                    pInstance.Port,
                    pInstance.Token
                    );

                }
                //Don't exist
                else
                {
                    insertOrUpdateCmd.CommandText = string.Format(@"INSERT INTO
                    {0} SET
                    name = '{1}', 
                    environment = '{2}',
                    host = '{3}',
                    port = '{4}',
                    token = '{5}'
                    ",
                    Constants.TableName.SERVER_INSTANCE,
                    pInstance.Name,
                    pInstance.Environment,
                    pInstance.Host,
                    pInstance.Port,
                    pInstance.Token
                    );
                }
                                                
                insertOrUpdateCmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Use to delete the server reference un database
        /// </summary>
        /// <param name="pConnection"></param>
        /// <param name="pServerName"></param>
        /// <param name="pEnvironment"></param>
        /// <returns></returns>
        public static bool DeleteServerInstance(MySqlConnection pConnection, ServerInstanceModel pInstance)
        {
            try
            {
                MySqlCommand DeleteCmd = pConnection.CreateCommand();

                //Delete line in database
                DeleteCmd.CommandText = string.Format(@"DELETE FROM
                {0}
                WHERE 
                ({0}.name = '{1}' 
                AND {0}.environment = '{2}')",
                Constants.TableName.SERVER_INSTANCE,
                pInstance.Name,
                pInstance.Environment
                );

                //Execute query
                DeleteCmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns an instance
        /// </summary>
        /// <param name="pConnection"></param>
        /// <param name="pName"></param>
        /// <param name="pEnvironment"></param>
        /// <returns></returns>
        public static ServerInstanceModel GetServerInstance(MySqlConnection pConnection, string pName, string pEnvironment)
        {
            try
            {
                //Select line in database
                String query = string.Format(@"SELECT * FROM
                {0}
                WHERE 
                ({0}.name = '{1}' 
                AND {0}.environment = '{2}')",
                Constants.TableName.SERVER_INSTANCE,
                pName,
                pEnvironment
                );

                //Execute query
                ServerInstanceModel serverInstanceModel = pConnection.QueryFirstOrDefault<ServerInstanceModel>(query);

                return serverInstanceModel;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Returns an instance
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pEnvironment"></param>
        /// <returns></returns>
        public static ServerInstanceModel GetServerInstanceByToken(MySqlConnection pConnection, string pToken)
        {
            try
            {
                //Select line in database
                String query = string.Format(@"
                SELECT si.name, si.environment, si.host, si.port, si.token 
                FROM {0} si
                WHERE 
                si.token = '{1}'",
                Constants.TableName.SERVER_INSTANCE,
                pToken
                );
                
                //Execute query
                ServerInstanceModel serverInstanceModel = pConnection.QueryFirstOrDefault<ServerInstanceModel>(query);

                return serverInstanceModel;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        #endregion
    }
}
