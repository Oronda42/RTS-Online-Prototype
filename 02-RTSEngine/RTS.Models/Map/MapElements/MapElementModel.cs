using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RTS.Models
{
    [Serializable]
    public class MapElementModel
    {
        #region Properties

        public int Id;
        public string Name;
        public MapElementTypeModel Type;
       
        #endregion
    }
}
