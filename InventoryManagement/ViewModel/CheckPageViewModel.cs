using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.Web.Http;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using InventoryManagement.Common;
using InventoryManagement.Data;
using InventoryManagement.Model.Inventory;
using InventoryManagement.Model.StoresInformation;
using WinRTXamlToolkit.IO.Serialization;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace InventoryManagement.ViewModel
{
    public class CheckPageViewModel : ViewModelBase
    {
        private CheckPageState _checkPageStateType;

        private string _employeeId;
        private string _employeeName;

        private string _workOrderId;
        private WorkOrder _workOrder;
        private Func<string, string, bool> _searchFunction;

        private string _itemNumber;
        
        private Dictionary<string, List<string>> LocalWorkOrderIds;
        private Dictionary<string, Item> LocalItems;

        private SettingsViewModel _settingsViewModel;
        private NotificationViewModel _notificationViewModel;

        public string WorkOrderId 
        {
            get { return _workOrderId; }
            set { _workOrderId = value; RaisePropertyChanged("WorkOrderId"); }
        }

        public string ItemNumber
        {
            get { return _itemNumber; }
            set { _itemNumber = value; RaisePropertyChanged("ItemNumber"); }
        }

        public List<string> WorkOrderIds { get; private set; }
        public ObservableCollection<Item> Items { get; set; }
        
        public Func<string, string, bool> SearchFunction
        {
            get { return _searchFunction; }
            set
            {
                if (_searchFunction != value)
                {
                    _searchFunction = value;
                    RaisePropertyChanged("SearchFunction");
                }
            }
        }
        
        public DateTime CheckDate
        {
            get { return DateTime.Now; }
        }
        public string CheckDateString
        {
            get { return string.Format("{0}/{1}/{2}", CheckDate.Month, CheckDate.Day, CheckDate.Year); }
        }

        public string TransactionDateTimeString
        {
            get { return string.Format("{0}-{1}-{2}", CheckDate.Year, CheckDate.Month, CheckDate.Day); }
        }

        public string EmployeeId
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        public string EmployeeName
        {
            get { return _employeeName; }
            set { _employeeName = value; }
        }
        
        public StoresInformation StoresInformation
        {
            get
            {
                if (_settingsViewModel == null)
                    _settingsViewModel =
                        (SimpleIoc.Default.GetInstance(typeof (SettingsViewModel)) as SettingsViewModel);

                return _settingsViewModel.StoresInformation;
            }
        }

        public NotificationViewModel NotificationDialog
        {
            get
            {
                if (_notificationViewModel == null)
                    _notificationViewModel =
                        (SimpleIoc.Default.GetInstance(typeof (NotificationViewModel)) as NotificationViewModel);

                return _notificationViewModel;
            }
        }

        public RelayCommand OnCheckClickCommand { get; set; }
        public RelayCommand OnReviewCheckClickCommand { get; set; }

        public CheckPageState CheckPageStateType
        {
            get { return _checkPageStateType; }
            set { _checkPageStateType = value; RaisePropertyChanged("CheckPageStateType"); }
        }

        public CheckPageViewModel()
        {
            this.CheckPageStateType = new CheckPageState();
            SearchFunction = (itemInList, typedText) => itemInList.ToLower().Contains(typedText.ToLower());
            LocalWorkOrderIds = new Dictionary<string, List<string>>();
            LocalItems = new Dictionary<string, Item>();
            Items = new ObservableCollection<Item>();
            EmployeeId = "psinha"; // TODO: GET from API
            EmployeeName = "Prashant Sinha"; // TODO: GET from API

            OnCheckClickCommand = new RelayCommand(() => CheckItems(), null);
            OnReviewCheckClickCommand = new RelayCommand(() => ReviewCheckItem());
        }

        public object ReviewCheckItem()
        {
            var str = "";
            if (Items == null || Items.Count == 0)
                str += "Please add Items\n";
            
            if (CheckPageStateType.Mode != CheckPageState.CheckMode.CheckIn && (WorkOrderId == null || WorkOrderIds == null || WorkOrderId.Length < 2 || WorkOrderIds.Count == 0))
                str += "Please add Work order\n";

            if (str.Length > 0)
            {
                NotificationDialog.ShowNoficationDialog(NotificationViewModel.NotificationType.Error, str);
                return null;
            }
            
            NavigationHelper.NavigateToPage(typeof (ReviewCheckItem));
            if (CheckPageStateType.Mode == CheckPageState.CheckMode.CheckOut)
                CheckPageStateType.SetCheckPageState(CheckPageState.CheckMode.CheckOutReview);
            else if (CheckPageStateType.Mode == CheckPageState.CheckMode.CheckIn)
                CheckPageStateType.SetCheckPageState(CheckPageState.CheckMode.CheckInReview);

            return null;
        }

        public bool LocalWorkOrderIdsContainsKey(ref string key)
        {
            if (LocalWorkOrderIds.ContainsKey(_workOrderId))
            {
                key = _workOrderId;
                return true;
            }

            var w = (_workOrderId.Length == 2) ? _workOrderId :_workOrderId.Remove(2);

            foreach (var item in LocalWorkOrderIds)
            {
                if (item.Key.IndexOf(w) == 0 && item.Value != null)
                {
                    key = _workOrderId;
                    LocalWorkOrderIds.Add(key, new List<string>(item.Value.Where(x => x.IndexOf(_workOrderId) == 0)));
                             
                    return true;
                }
            }
       

            return false;
        }

        public Task UpdateWorkOrders(string str)
        {
            _workOrderId = str;
            if (_workOrderId == null)
                return null;
            _workOrderId = str.Trim();
            if (!Regex.IsMatch(_workOrderId, "^[a-zA-Z0-9]+$"))
                return null;

            if (_workOrderId.Length > 1)
            {
                string key = null;
                if (LocalWorkOrderIdsContainsKey(ref key))
                {
                    return Task.Run(() => WorkOrderIds = LocalWorkOrderIds[key]);
                }
                else
                    return GetWorkOrders(_workOrderId);
            }
            else
                WorkOrderIds = null;
            return null;
        }

        public async Task GetWorkOrders(string workorder)
        {
            var url = "http://spsight.cloudapp.net/assetmanager/inventory/workorder/" + workorder;
            var uri = new Uri(url);

            try
            {
                using (var client = new HttpClient())
                {
                    string str = await client.GetStringAsync(uri);
                    var o = JsonArray.Parse(str);

                    WorkOrderIds = (o.Count > 0) ? new List<string>() : null;

                    //var list = new List<WorkOrder>();
                    foreach (var itemValue in o)
                    {
                        var itemObject = itemValue.GetObject();
                        WorkOrderIds.Add(itemObject["workOrderNumber"].GetString());

                        //list.Add(new WorkOrder()
                        //{
                        //    Account = Decimal.Parse(itemObject["Account"].GetString()),
                        //    Description = itemObject["Description"].GetString(),
                        //    WorkClass = itemObject["WorkClass"].GetString(),
                        //    WorkOrderNumber = itemObject["WorkOrderNumber"].GetString(),
                        //    WOTask = itemObject["WOTask"].GetString(),
                        //    WOType = itemObject["WOType"].GetString()
                        //});
                    }
                    LocalWorkOrderIds.Add(workorder, WorkOrderIds);
                }
            }
            catch (Exception ex)
            {
                NotificationDialog.ShowNoficationDialog(NotificationViewModel.NotificationType.Warning, "Can't load WorkOrders.\n" + ex.Message);
            
            }
            
        }

        public Task UpdateItems(string str)
        {
            _itemNumber = str;
            if (_itemNumber == null)
                return null;
            _itemNumber = str.Trim(); 
            if (!Regex.IsMatch(_itemNumber, "^[0-9]+$"))
                return null;

            if (Items.Any(x => x.ItemNumber == _itemNumber))
                return null;

            if (LocalItems.ContainsKey(_itemNumber))
            {
                Items.Add(LocalItems[_itemNumber]);
                return null;
            }

            return GetItems(_itemNumber);
        }

        public void RemoveItem(string str)
        {
            var item = Items.First(x => x.ItemNumber == str);
            Items.Remove(item);
        }

        public async Task GetItems(string itemNumber)
        {
            var url = "http://spsight.cloudapp.net/assetmanager/inventory/item/" + itemNumber;
            var uri = new Uri(url);
            try
            {
                using (var client = new HttpClient())
                {
                    string str = await client.GetStringAsync(uri);
                    var o = JsonArray.Parse(str);

                    if (o.Count > 0)
                    {
                        foreach (var itemValue in o)
                        {
                            var itemObject = itemValue.GetObject();

                            var storesLocation = itemObject["storesLocation"].GetString();

                            if (storesLocation.Equals(StoresInformation.CurrentLocation, StringComparison.OrdinalIgnoreCase))
                            {
                                var item = new Item()
                                {
                                    ItemNumber = itemObject["itemNumber"].GetString().Trim(),
                                    Description = itemObject["description"].GetString().Trim(),
                                    //StoresLocation = itemObject["storesLocation"].GetString(),
                                    //ItemClass = itemObject["itemClass"].GetString(),
                                    UnitCost = itemObject["unitCost"].GetNumber(),
                                    //ItemType = itemObject["itemType"].GetString(),
                                    //ImageUrl = "https://bn1301files.storage.live.com/y2mWeerUlR7iUh6I2XrT9JMrVX_lSv21AIX4e1vOzBP9qJzqaXRXLspi3ID4f8NGL3LjuX8gLj7ii697kCaOK8ljxscrzDQ5CWF1xl0vHYLMB631hJ_Go0N_5OIEtcae9xv/scotch-blue.jpg" //itemObject["ImageUrl"].GetString()
                                    Quantity = 1
                                };
                                Items.Add(item);
                                LocalItems.Add(itemNumber, item);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationDialog.ShowNoficationDialog(NotificationViewModel.NotificationType.Warning, "Can't load items.\n" + ex.Message);
            }
            
        }

        private object CheckItems()
        {
            if (Items != null && Items.Count > 0)
            {
                switch (CheckPageStateType.Mode)
                {
                    case CheckPageState.CheckMode.CheckOutReview:
                        CheckOut();
                        break;
                    case CheckPageState.CheckMode.CheckInReview:
                        CheckIn();
                        break;
                }

                
            }
                    
            return null;
        }

        private void CheckIn()
        {
            var transactionDateTime = TransactionDateTimeString;

            var list = new List<CheckInItem>();

            foreach (var item in Items)
            {
                list.Add(new CheckInItem()
                {
                    Item = item.ItemNumber,
                    StoresLocation = StoresInformation.CurrentLocation,
                    Quantity = item.Quantity,
                    Approver = EmployeeId,
                    UnitCost = item.UnitCost
                });
            }

            PostItems(list.SerializeAsJson(), "http://spsight.cloudapp.net/assetmanager/inventory/checkin"); 
        }


        private void CheckOut()
        {
            var transactionDateTime = TransactionDateTimeString;

            var list = new List<CheckOutItem>();

            foreach (var item in Items)
            {
                list.Add(new CheckOutItem()
                {
                    Item = item.ItemNumber,
                    StoresLocation = StoresInformation.CurrentLocation,
                    Quantity = item.Quantity,
                    TransactionDateTime = transactionDateTime,
                    EmployeeId = EmployeeId,
                    WorkOrder = WorkOrderId
                });
            }

            PostItems(list.SerializeAsJson(), "http://spsight.cloudapp.net/assetmanager/inventory/Inventory/checkout");
        }

        private async Task PostItems(string str, string url)
        {
            var uri = new Uri(url);

            try
            {
                using (var client = new HttpClient())
                {
                    var r = await client.PostAsync(uri, new HttpStringContent(str));

                    if (r.IsSuccessStatusCode)
                    {
                        Clear(false);
                        NavigationHelper.NavigateToPage(typeof(GroupedItemsPage));
                    }
                    else
                    {
                        NotificationDialog.ShowNoficationDialog(NotificationViewModel.NotificationType.Error, "Something gone wrong...\n" + r.ReasonPhrase);
                    }
                }
            }
            catch (Exception)
            {
                NotificationDialog.ShowNoficationDialog(NotificationViewModel.NotificationType.Error, "Something gone wrong..\nYou can try to Check again.");
            }
        }

        public void Clear(bool isDeep = true)
        {
            ItemNumber = null;
            Items.Clear();
            if (isDeep)
            {
                WorkOrderId = string.Empty;
                LocalItems.Clear();                
            }
            
        }
    }
}
