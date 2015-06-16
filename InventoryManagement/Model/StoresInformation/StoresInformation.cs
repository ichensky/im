using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Model.StoresInformation
{
    public class StoresInformation
    {
        private string _currentLocation;

        public string CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; }
        }
    }
}
