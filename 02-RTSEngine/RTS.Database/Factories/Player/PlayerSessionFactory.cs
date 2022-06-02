using Dapper;
using MySql.Data.MySqlClient;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Database
{
    public static class PlayerSessionFactory
    {

        #region Implementation

        /// <summary>
        /// Create a new session for the player
        /// </summary>
        /// <param name="pTransaction"></param>
        /// <param name="pPlayer"></param>
        /// <returns></returns>
        public static void Create(MySqlConnection pConnection, PlayerSessionModel pSession)
        {
            MySqlCommand insertCmd = pConnection.CreateCommand();

            //Query
            insertCmd.CommandText = string.Format(@"INSERT INTO {0}
                (player_id, token, start, end) 
                VALUES
                (@player_id, @token, @start, @end)",
            Constants.TableName.PLAYER_SESSIONS);

            //Set parameters
            insertCmd.Parameters.Add("@player_id", MySqlDbType.String).Value = pSession.PlayerId;
            insertCmd.Parameters.Add("@token", MySqlDbType.String).Value = pSession.Token;
            insertCmd.Parameters.Add("@start", MySqlDbType.DateTime).Value = pSession.Start;
            insertCmd.Parameters.Add("@end", MySqlDbType.DateTime).Value = pSession.End;

            insertCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update the cession in database at disconnection
        /// </summary>
        /// <param name="pConnection"></param>
        /// <param name="pSession"></param>
        public static void Update(MySqlConnection pConnection, PlayerSessionModel pSession)
        {
            MySqlCommand insertCmd = pConnection.CreateCommand();

            pSession.End = DateTime.Now;

            //Query
            insertCmd.CommandText = string.Format(@"UPDATE {0}
                set start = @start,
                end = @end                
                WHERE 
                (player_id = @player_id AND token = @token)",
            Constants.TableName.PLAYER_SESSIONS);

            //Set parameters
            insertCmd.Parameters.Add("@player_id", MySqlDbType.String).Value = pSession.PlayerId;
            insertCmd.Parameters.Add("@start", MySqlDbType.DateTime).Value = pSession.Start;
            insertCmd.Parameters.Add("@end", MySqlDbType.DateTime).Value = pSession.End;
            insertCmd.Parameters.Add("@token", MySqlDbType.String).Value = pSession.Token;

            insertCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Returns the player session
        /// </summary>
        /// <param name="pConnection"></param>
        /// <param name="pPlayer"></param>
        /// <returns></returns>
        public static PlayerSessionModel GetByPlayerId(MySqlConnection pConnection, int pPlayerId)
        {
            //Get level
            string query = string.Format(@"
            SELECT player_id PlayerId, token, start, end 
            FROM {0} ps INNER JOIN {1} p ON (p.current_token = ps.token and p.id = {2})
            WHERE player_id = {2}",
            Constants.TableName.PLAYER_SESSIONS,
            Constants.TableName.PLAYER,
            pPlayerId); 

            return pConnection.Query<PlayerSessionModel>(query).FirstOrDefault();
        }

        #endregion
    }
}
