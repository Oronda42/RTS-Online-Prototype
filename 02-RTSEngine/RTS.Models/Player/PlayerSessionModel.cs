using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    public class PlayerSessionModel
    {
        #region Properties

        public int PlayerId { get; set; }
        public string Token { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        #endregion
    }
}
