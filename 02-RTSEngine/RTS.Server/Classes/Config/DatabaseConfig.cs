using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public class DatabaseConfig
    {
        public string Host { get; set; }
        public uint Port { get; set; }
        public string DatabaseName { get; set; }
        public string User  { get; set; }
        public string Password { get; set; }
    }
}
