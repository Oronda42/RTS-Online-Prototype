using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server.GameServer
{
    /// <summary>
    /// Class that contains all data for the server
    /// </summary>
    public static class GameServerData
    {
        #region Properties


        #endregion

        #region Implementation

        /// <summary>
        /// Use this script to initialize all data
        /// </summary>
        public static void Init(GameServerInitializationPlugin pInitializationPlugin)
        {
            //Initialization of the player map service
            PlayerMapService.Init(pInitializationPlugin.DBConnection.Connection);

        }

        #endregion
    }
}