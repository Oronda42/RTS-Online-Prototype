using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Models
{
    public class Module : IInventoriableModel
    {
        public InventoryItemSettingsModel InventorySettings { get; set; }
    }
}
