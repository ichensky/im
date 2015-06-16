using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Model
{
    public class Checkout
    {
        public List<InventoryItem> InventoryItems { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeStoresLocation { get; set; }
        public DateTime CheckoutDate { get; set; }
        public List<string> ChargeTo { get; set; }
    }
}
