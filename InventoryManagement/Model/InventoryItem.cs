using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Model
{
    public class InventoryItem
    {
        public string ItemNumber { get; set; }
        public string ItemDescription { get; set; }
        public string StoresLocation { get; set; }
        public decimal UnitCost { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityCheckedOut { get; set; }
        public int QuantityCheckedIn { get; set; }
        public int QuantityReturned { get; set; }
    }
}
