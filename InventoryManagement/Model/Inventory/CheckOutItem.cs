using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Model.Inventory
{
    public class CheckOutItem
    {
        private string _item;
        private string _storesLocation;
        private double _quantity;
        private string _transactionDateTime;
        private string _employeeId;
        private string _workOrder;

        public string Item
        {
            get { return _item; }
            set { _item = value; }
        }

        public string StoresLocation
        {
            get { return _storesLocation; }
            set { _storesLocation = value; }
        }

        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value;  }
        }

        public string TransactionDateTime
        {
            get { return _transactionDateTime; }
            set { _transactionDateTime = value; }
        }

        public string EmployeeId
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        public string WorkOrder
        {
            get { return _workOrder; }
            set { _workOrder = value; }
        }
    }
}
