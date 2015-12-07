using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MileageTracker.Types
{
    public abstract class BaseInfo : DependencyObject, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region OnPropertyChanged
        public void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion //OnPropertyChanged

        #region OnStatusChanged
        public event EventHandler<StatusChangedArgs> StatusChanged;
        public void OnStatusChanged(Int32 Status, String Message)
        {
            if (StatusChanged != null) StatusChanged(this, new StatusChangedArgs() { Status = Status, Message = Message });
        }
        #endregion //OnStatusChanged

    }

    public class StatusChangedArgs : EventArgs
    {
        public Int32 Status { get; set; }
        public String Message { get; set; }
    }
}
