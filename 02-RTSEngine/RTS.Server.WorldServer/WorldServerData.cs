using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS.Models.Server;
using RTS.Database;
using MySql.Data.MySqlClient;

namespace RTS.Server.WorldServer
{
    /// <summary>
    /// Class that contains all data for the server
    /// </summary>
    internal static class WorldServerData
    {
        #region Properties
        
        /// <summary>
        /// List of all zone managed by a game server
        /// </summary>
        public static List<GameServerZoneModel> GameServerZoneList;

        #endregion

        #region Implementation

        /// <summary>
        /// Use this script to initialize all data
        /// </summary>
        public static void Init(WorldServerInitializationPlugin pInitializationPlugin)
        {
            GameServerZoneList = new List<GameServerZoneModel>();
            GameServerZoneList = WorldServerFactory.GetAllGameServerZone(pInitializationPlugin.DBConnection.Connection, pInitializationPlugin.InstanceInformation.ServerInstanceModel.Environment);
        }

        /// <summary>
        /// Returns the game server for a specific zone
        /// </summary>
        /// <param name="pWorldZoneId"></param>
        /// <returns></returns>
        public static GameServerZoneModel GetGameServerNameForZone(int pWorldZoneId)
        {
            GameServerZoneModel gameServerInformation = GameServerZoneList.Where(gs => gs.WorldZoneId == pWorldZoneId).FirstOrDefault();

            if(gameServerInformation == null)
            {
                LoggingEvent log = new LoggingEvent(LogLevel.ERROR, "WorldZone " + pWorldZoneId + " doesn't exists", null);
                DispatcherThread.Instance.EnqueueEvent(log);
            }

            return gameServerInformation;
        }

        #endregion
    }
}
