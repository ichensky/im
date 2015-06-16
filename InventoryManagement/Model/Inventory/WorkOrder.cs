namespace InventoryManagement.Model.Inventory
{
    public class WorkOrder
    {
        private string _workOrderNumber;
        private string _wOTask;
        private string _description;
        private string _wOType;
        private string _workClass;
        private decimal _account;

        public string WorkOrderNumber
        {
            get { return _workOrderNumber; }
            set { _workOrderNumber = value; }
        }

        public string WOTask
        {
            get { return _wOTask; }
            set { _wOTask = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string WOType
        {
            get { return _wOType; }
            set { _wOType = value; }
        }

        public string WorkClass
        {
            get { return _workClass; }
            set { _workClass = value; }
        }

        public decimal Account
        {
            get { return _account; }
            set { _account = value; }
        }
    }
}
