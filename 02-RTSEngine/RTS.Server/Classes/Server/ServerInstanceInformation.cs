using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkRift.Client;
using RTS.Models.Server;

namespace RTS.Server
{
    public class ServerInstanceInformation
   {
        #region Properties

        /// <summary>
        /// Connection to another server
        /// </summary>
        public DarkRiftClient ClientConnection { get; set; }

        /// <summary>
        /// Name of the server
        /// </summary>
        public ServerInstanceModel ServerInstanceModel { get; set; }


        #endregion

        #region Implementation

        /// <summary>
        /// Init the server information
        /// </summary>
        /// <param name="pClientConnection"></param>
        /// <param name="pName"></param>
        /// <param name="pEnvironment"></param>
        /// <param name="pClientToken"></param>
        public void Init(DarkRiftClient pClientConnection, ServerInstanceModel pInstance)
        {
            ClientConnection = pClientConnection;
            ServerInstanceModel = pInstance;
        }
        
        #endregion

    }
}
