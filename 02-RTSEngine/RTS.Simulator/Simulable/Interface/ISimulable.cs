using MySql.Data.MySqlClient;
using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Simulator
{
    public interface ISimulable
    {
        /// <summary>
        /// Returns the next action in the time
        /// </summary>
        /// <returns></returns>
        DateTime GetNextActionDateTime();

        /// <summary>
        /// Simulate the action of the object over time
        /// </summary>
        void SimulateOverTime(Simulation pSimulation, ref PlayerModel pPlayer);

        /// <summary>
        /// Update data within the server
        /// </summary>
        void UpdateDatabase(ref MySqlTransaction pTransaction);

        /// <summary>
        /// Returns true if the other object equal this one
        /// </summary>
        /// <param name="pOther"></param>
        /// <returns></returns>
        bool Equals(object pOther);
        
    }
}
