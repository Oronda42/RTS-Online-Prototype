using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public interface ISchedulablePlugin
    {
        /// <summary>
        /// Initialization function
        /// </summary>
        /// <param name="args"></param>
        void Init(object[] args);

    }
}
