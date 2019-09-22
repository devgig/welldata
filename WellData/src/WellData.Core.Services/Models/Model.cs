using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WellData.Core.Common;

namespace WellData.Core.Services.Models
{
    public abstract class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (_suppressNotifyPropertyChanged) return;
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        private bool _suppressNotifyPropertyChanged;
        public IDisposable SuppressNotifyPropertyChange()
        {
            _suppressNotifyPropertyChanged = true;
            return new DisposableActionInvoker(() => _suppressNotifyPropertyChanged = false);
        }

    }
}
