/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:InventoryManagement"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace InventoryManagement.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CheckPageViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<SettingsDialogViewModel>();
            SimpleIoc.Default.Register<NotificationViewModel>();
            SimpleIoc.Default.Register<ReturnPageViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public CheckPageViewModel CheckPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CheckPageViewModel>();
            }
        }
        public SettingsViewModel Settings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }

        public SettingsDialogViewModel SettingsDialog
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsDialogViewModel>();
            }
        }

        public NotificationViewModel NotificationViewModelDialog
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NotificationViewModel>();
            }
        }

        public ReturnPageViewModel ReturnPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ReturnPageViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}