using InventoryManagement.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Model.Inventory
{
    public class ReturnItem : BindableBase
    {
        private string _account;
        private string _area;
        private string _department;
        private string _description;
        private string _employeeId;
        private string _item;
        private string _project;
        private double _quantityCheckedOut;
        private double _quantityReturned;
        private string _workOrder;

        private double _quantity;
        
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        public string Area
        {
            get { return _area; }
            set { _area = value; }
        }

        public string Department
        {
            get { return _department; }
            set { _department = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string EmployeeId
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        public string Item
        {
            get { return _item; }
            set { _item = value; }
        }

        public string Project
        {
            get { return _project; }
            set { _project = value; }
        }

        public double QuantityCheckedOut
        {
            get { return _quantityCheckedOut; }
            set { _quantityCheckedOut = value; }
        }

        public double QuantityReturned
        {
            get { return _quantityReturned; }
            set { _quantityReturned = value; }
        }

        public string WorkOrder
        {
            get { return _workOrder; }
            set { _workOrder = value; }
        }


        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; this.RaisePropertyChanged("Quantity"); }
        }

    }
}
