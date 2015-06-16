using Windows.UI.Xaml;
using InventoryManagement.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Model.Inventory
{
    public class CheckPageState : BindableBase
    {
        public enum CheckMode
        {
            CheckOut,
            CheckOutReview,
            CheckIn,
            CheckInReview
        }

        private CheckMode _mode;
        private Visibility _isRemoveItemVisible;
        private bool _isItemQuantityReadOnly;
        private Visibility _isItemUnitCostVisible;
        private Visibility _isItemUnitCostNewVisible;
        private bool _isItemUnitCostNewReadOnly;

        private string _appName;
        private string _checkButtonName;

        public CheckMode Mode
        {
            get { return _mode; }
            private set { _mode = value; }
        }

        public Visibility IsRemoveItemVisible
        {
            get { return _isRemoveItemVisible; }
            set { _isRemoveItemVisible = value; RaisePropertyChanged("IsRemoveItemVisible"); }
        }

        public bool IsItemQuantityReadOnly
        {
            get { return _isItemQuantityReadOnly; }
            set { _isItemQuantityReadOnly = value; RaisePropertyChanged("IsItemQuantityReadOnly"); }
        }

        public Visibility IsItemUnitCostVisible
        {
            get { return _isItemUnitCostVisible; }
            set { _isItemUnitCostVisible = value; RaisePropertyChanged("IsItemUnitCostVisible"); }
        }

        public Visibility IsItemUnitCostNewVisible
        {
            get { return _isItemUnitCostNewVisible; }
            set { _isItemUnitCostNewVisible = value; RaisePropertyChanged("IsItemUnitCostNewVisible"); }
        }

        public bool IsItemUnitCostNewReadOnly
        {
            get { return _isItemUnitCostNewReadOnly; }
            set { _isItemUnitCostNewReadOnly = value; RaisePropertyChanged("IsItemUnitCostNewReadOnly"); }
        }

        public string AppName
        {
            get { return _appName; }
            set { _appName = value; RaisePropertyChanged("AppName"); }
        }

        public string CheckButtonName
        {
            get { return _checkButtonName; }
            set { _checkButtonName = value; }
        }


        public void SetCheckPageState(CheckMode mode)
        {
            Mode = mode;
            switch (mode)
            {
                case CheckMode.CheckOut:
                    IsRemoveItemVisible = Visibility.Visible;
                    IsItemQuantityReadOnly = false;
                    IsItemUnitCostNewVisible = Visibility.Collapsed;
                    IsItemUnitCostVisible = Visibility.Visible;
                    AppName = "Checkout Item";
                    CheckButtonName = "Review Checkout";
                    break;
                case CheckMode.CheckOutReview:
                    IsRemoveItemVisible = Visibility.Collapsed;
                    IsItemQuantityReadOnly = true;
                    IsItemUnitCostNewVisible = Visibility.Collapsed;
                    IsItemUnitCostVisible = Visibility.Collapsed;
                    AppName = "Review Checkout Item";
                    CheckButtonName = "Complete Checkout";
                    break;
                case CheckMode.CheckIn:
                    IsRemoveItemVisible = Visibility.Visible;
                    IsItemQuantityReadOnly = false;
                    IsItemUnitCostNewVisible = Visibility.Visible;
                    IsItemUnitCostVisible = Visibility.Collapsed;
                    IsItemUnitCostNewReadOnly = false;
                    AppName = "Check In";
                    CheckButtonName = "Review Checkin";
                    break;
                case CheckMode.CheckInReview:
                    IsRemoveItemVisible = Visibility.Collapsed;
                    IsItemQuantityReadOnly = true;
                    IsItemUnitCostNewVisible = Visibility.Visible;
                    IsItemUnitCostVisible = Visibility.Collapsed;
                    IsItemUnitCostNewReadOnly = false;
                    AppName = "Review Checkin";
                    CheckButtonName = "Complete Checkin";
                    break;
            }
        }
    }
}
