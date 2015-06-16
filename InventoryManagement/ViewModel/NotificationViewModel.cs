using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace InventoryManagement.ViewModel
{
    public class NotificationViewModel : ViewModelBase
    {
        public enum NotificationType
        {
            Error,   
            Info,   
            Warning, 
        }

        private Popup _popup;
        private DispatcherTimer _dispatcherTimer;
        private string _notificationMessage;

        public RelayCommand OnCloseClickCommand { get; set; }

        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set { _notificationMessage = value; RaisePropertyChanged("NotificationMessage"); }
        }

        public DispatcherTimer DispatcherTimer
        {
            get { return _dispatcherTimer ?? (_dispatcherTimer = new DispatcherTimer()); }
            set { _dispatcherTimer = value; }
        }

        public Popup Popup
        {
            get { return _popup ?? (_popup = new Popup {Child = new NotificationDialog()}); }
            set { _popup = value; }
        }

        public NotificationViewModel()
        {
            OnCloseClickCommand = new RelayCommand(() => CloseNofificationDialog(), null);
        }

        public object ShowNoficationDialog(NotificationType notificationType, string notificationMessage)
        {
            NotificationMessage = notificationMessage;

            if (Popup.IsOpen)
            {
                DispatcherTimer.Start();
            }
            else
            {
                Popup.IsOpen = true;
                DispatcherTimer.Tick += TimerOnTick;
                DispatcherTimer.Interval = new TimeSpan(0, 0, 5);
                DispatcherTimer.Start();
            }
            

            return null;
        }

        private void TimerOnTick(object sender, object o)
        {
            if (Popup.IsOpen)
            {
                Popup.IsOpen = false;
                DispatcherTimer.Stop();
            }
        }

        private object CloseNofificationDialog()
        {
            Popup.IsOpen = false;
            DispatcherTimer.Stop();
            return null;
        }
    }
}
