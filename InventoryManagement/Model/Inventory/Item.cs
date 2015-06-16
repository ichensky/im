using System;
using Windows.UI.Xaml.Media;
using InventoryManagement.Common;

namespace InventoryManagement.Model.Inventory
{
    public class Item : BindableBase
    {
        private string _itemNumber;
        private string _storesLocation;
        private string _description;
        private string _itemClass;
        private double _unitCost;
        private string _itemType;
        private string _imageUrl;

        private double _quantity;
        
        public string ItemNumber
        {
            get { return _itemNumber; }
            set { _itemNumber = value; }
        }

        public string StoresLocation
        {
            get { return _storesLocation; }
            set { _storesLocation = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string ItemClass
        {
            get { return _itemClass; }
            set { _itemClass = value; }
        }

        public double UnitCost
        {
            get { return _unitCost; }
            set { _unitCost = value; this.RaisePropertyChanged("UnitCost"); }
        }

        public string ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; this.RaisePropertyChanged("Quantity"); }
        }
    }
}
