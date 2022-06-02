using RTS.Models;
using RTS.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public class ClientInformation
    {
        #region Properties

        public int ClientId { get; set; }
        public int PlayerId { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }

        /// <summary>
        /// Contain PlayerID, Token, Start and End Session
        /// </summary>
        public PlayerSessionModel PlayerSession { get; set; }

        /// <summary>
        /// Simulation of the player. Can be null if non necessary
        /// </summary>
        public Simulation Simulation { get; set; }

        #endregion
    }
}
