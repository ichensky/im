using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

namespace InventoryManagement.ViewModel
{
    public class SettingsDialogViewModel : ViewModelBase
    {
        private Popup _popup;
        private bool _isNotForcedEditingSettings;

        private SettingsViewModel _settingsViewModel;

        public RelayCommand OnSaveClickCommand { get; set; }

        public RelayCommand OnCancelClickCommand { get; set; }
        
        public bool IsNotForcedEditingSettings
        {
            get { return _isNotForcedEditingSettings; }
            set { _isNotForcedEditingSettings = value; }
        }

        public SettingsViewModel SettingsViewModel
        {
            get
            {
                if (_settingsViewModel == null)
                {
                    _settingsViewModel = (SimpleIoc.Default.GetInstance(typeof (SettingsViewModel))) as SettingsViewModel;
                }
                
                return _settingsViewModel;
            }
        }

        public SettingsDialogViewModel()
        {
            OnSaveClickCommand = new RelayCommand(() => SaveSettings(), null);
            OnCancelClickCommand = new RelayCommand(() => CancelEditSettings(), null);
        }

        public void ShowEditSettingsDialog()
        {
            _popup = new Popup();
            _popup.Child = new SettingsDialog();
            _popup.IsOpen = true;
        }

        private object CancelEditSettings()
        {
            _popup.IsOpen = false;

            return null;
        }

        private object SaveSettings()
        {
            var s = SettingsViewModel.SaveSettings(); 
            if (s.Item1)
            {
                _popup.IsOpen = false;
            }
            
            // Show error s.Item2

            return null;
        }
    }
}
