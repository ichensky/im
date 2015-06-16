using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using InventoryManagement.DataModel;
using InventoryManagement.Helpers;
using InventoryManagement.Model.StoresInformation;

namespace InventoryManagement.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private List<string> _storesLocations;
        private Settings<StoresInformation> _settingsStoresInformation;
        private string _currentStoresLocation;

        private StoresInformation _storesInformation;

        public List<string> StoresLocations
        {
            get { return _storesLocations; }
            set { _storesLocations = value; }
        }

        public Settings<StoresInformation> SettingsStoresInformation
        {
            get { return _settingsStoresInformation; }
            set { _settingsStoresInformation = value; }
        }
        
        public StoresInformation StoresInformation
        {
            get { return _storesInformation; }
            set { _storesInformation = value;  }
        }

        public string CurrentStoresLocation
        {
            get { return StoresInformation.CurrentLocation; }
            set { StoresInformation.CurrentLocation = value; }
        }


        public SettingsViewModel()
        {
            StoresInformation = new StoresInformation();
            SettingsStoresInformation = new Settings<StoresInformation>();
            StoresLocations = new List<string>() { "DM", "RC" };
            LoadSettings();
        }

        public async void LoadSettings()
        {
            try
            {
                var x = SettingsStoresInformation.LoadAsync(LocalStorage.StoresInformationKey);
                await x.ContinueWith(a =>
                {
                    if (x.Result != null)
                    {
                        StoresInformation = x.Result;
                    }
                });

            }
            catch (Exception)
            {
            }
        }

        public Tuple<bool, string> SaveSettings()
        {
            if (string.IsNullOrEmpty(StoresInformation.CurrentLocation))
                return new Tuple<bool, string>(false, "Stores location is not selected.");

            SettingsStoresInformation.SaveAsync(LocalStorage.StoresInformationKey, new StoresInformation() { CurrentLocation = StoresInformation.CurrentLocation });

            return new Tuple<bool, string>(true, null);
            
        }
    }
}
