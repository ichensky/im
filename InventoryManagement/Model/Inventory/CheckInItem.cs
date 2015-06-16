using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Model.Inventory
{
    public class CheckInItem
    {
        private string _item;
        private string _storesLocation;
        private double _quantity;
        private string _approver;
        private double _unitCost;

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
            set { _quantity = value; }
        }

        public string Approver
        {
            get { return _approver; }
            set { _approver = value; }
        }

        public double UnitCost
        {
            get { return _unitCost; }
            set { _unitCost = value; }
        }
    }
}
