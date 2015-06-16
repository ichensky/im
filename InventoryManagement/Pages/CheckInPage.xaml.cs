using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using InventoryManagement.Common;
using InventoryManagement.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232
using InventoryManagement.Helpers;
using InventoryManagement.Model.Inventory;
using InventoryManagement.ViewModel;
using InventoryManagement.Views;

namespace InventoryManagement.Pages
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class CheckInPage : Page
    {
        private NavigationHelper navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public CheckInPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.ListViewInvetoryItems.RemoveItem += (sender, args) =>
            {
                (this.DataContext as CheckPageViewModel).RemoveItem((args as CheckGridItems.RemoveItemEventArgs).Item);
            };
        }
        
        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var itemId = e.Parameter as string;
            if (e.NavigationMode == NavigationMode.New)
            {
                (this.DataContext as CheckPageViewModel).Clear();
            }
            switch (itemId)
            {
                case "CheckOut":
                    (this.DataContext as CheckPageViewModel).CheckPageStateType.SetCheckPageState(CheckPageState.CheckMode.CheckOut);
                    break;
                case "CheckIn":
                    (this.DataContext as CheckPageViewModel).CheckPageStateType.SetCheckPageState(CheckPageState.CheckMode.CheckIn);
                    break;
            }
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void SearchBoxInventoryItem_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            (this.DataContext as CheckPageViewModel).UpdateItems(SearchBoxInventoryItem.QueryText);
        }

    }
}