using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models.Server
{
    public class GameServerZoneModel
    {
        #region Properties

        public string ServerName { get; set; }
        public string Environment { get; set; }
        public int WorldZoneId { get; set; }

        #endregion
    }
}
