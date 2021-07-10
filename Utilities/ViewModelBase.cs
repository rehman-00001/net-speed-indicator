using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Serilog;

namespace net_speed_indicator.Utilities
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            Log.Information("{0}::SetProperty() - PropertyName: {1}, value: {2}", GetType().Name, propertyName, value);
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
