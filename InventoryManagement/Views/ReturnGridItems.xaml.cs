using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using InventoryManagement.DataModel;
using InventoryManagement.Model.Inventory;

namespace InventoryManagement.Views
{
    public sealed partial class ReturnGridItems : UserControl
    {
        public class RemoveItemEventArgs : EventArgs
        {
            public string Item { get; set; }
        }
        
        public event EventHandler RemoveItem;

        public ReturnGridItems()
        {
            this.InitializeComponent();
        }

        private void ButtonRemoveItem_OnClick(object sender, RoutedEventArgs e)
        {
            RemoveItem.Invoke(sender, new RemoveItemEventArgs() { Item = (sender as Button).Tag.ToString()});
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var t = sender as TextBox;
            var str = string.Empty;
            var flag = false;

            foreach (var c in t.Text)
            {
                if (char.IsDigit(c))
                {
                    str += c;
                }
                else if (!flag && c == '.')
                {
                    str += c;
                    flag = true;
                }
            }
            t.Text = str;
        }
    }
}
