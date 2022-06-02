using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models.Server
{
    [Serializable]
    public class ServerInstanceModel
    {
        #region Properties

        public string Name { get; set; }
        public string Environment { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Token { get; set; }

        #endregion
    }
}
