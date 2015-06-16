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
using WinRTXamlToolkit.IO.Serialization;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace InventoryManagement.ViewModel
{
    public class ReturnPageViewModel : ViewModelBase
    {
        private CheckPageState _checkPageStateType;

        private string _employeeId;
        private string _employeeName;

        private string _workOrderId;
        private WorkOrder _workOrder;
        private Func<string, string, bool> _searchFunction;

        private string _itemNumber;
        
        private Dictionary<string, List<string>> LocalWorkOrderIds;

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
        public ObservableCollection<ReturnItem> Items { get; set; }
        
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


        public ReturnPageViewModel()
        {
            SearchFunction = (itemInList, typedText) => itemInList.ToLower().Contains(typedText.ToLower());
            LocalWorkOrderIds = new Dictionary<string, List<string>>();
            Items = new ObservableCollection<ReturnItem>();
            EmployeeId = "jdoe"; // TODO: GET from API
            EmployeeName = "John Doe"; // TODO: GET from API
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

            if (Items.Any(x => x.Item == _itemNumber))
                return null;
            
            return GetItems(_itemNumber);
        }

        public void RemoveItem(string str)
        {
            var item = Items.First(x => x.Item == str);
            Items.Remove(item);
        }

        public async Task GetItems(string itemNumber)
        {
            var url = string.Format("http://spsight.cloudapp.net/assetmanager/inventory/returnitem/{0}/employee/{1}", itemNumber, EmployeeId);
            var uri = new Uri(url);
            try
            {
                using (var client = new HttpClient())
                {
                    string str = await client.GetStringAsync(uri);
                    var o = JsonObject.Parse(str)["items"].GetArray();

                    if (o.Count > 0)
                    {
                        foreach (var itemValue in o)
                        {
                            var itemObject = itemValue.GetObject();

                            var item = new ReturnItem()
                            {
                                //Account = itemObject["account"].GetString().Trim(),
                                //Area = itemObject["area"].GetString().Trim(),
                                //Department = itemObject["department"].GetString().Trim(),
                                Description = itemObject["description"].GetString().Trim(),
                                //EmployeeId = itemObject["employeeId"].GetString().Trim(),
                                Item = itemObject["item"].GetString().Trim(),
                                //Project = itemObject["project"].GetProjectType(),
                                QuantityCheckedOut = itemObject["quantityCheckedOut"].GetNumber(),
                                QuantityReturned = itemObject["quantityReturned"].GetNumber(),
                                WorkOrder = itemObject["workOrder"].GetString().Trim(),
                            };
                            Items.Add(item); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationDialog.ShowNoficationDialog(NotificationViewModel.NotificationType.Warning, "Can't load items.\n" + ex.Message);
            }
            
        }

        public void Clear(bool isDeep = true)
        {
            ItemNumber = null;
            Items.Clear();
            if (isDeep)
            {
                WorkOrderId = string.Empty;          
            }
            
        }
    }
}
