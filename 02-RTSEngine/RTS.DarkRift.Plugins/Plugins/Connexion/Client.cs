using RTS.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Plugins
{
    public class Client
    {
        public int ClientId { get; set; }
        public int PlayerId { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }
        public Simulation Simulation { get; set; }
    }
}
