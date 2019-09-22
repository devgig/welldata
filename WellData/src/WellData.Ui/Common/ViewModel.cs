using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WellData.Core.Common;

namespace WellData.Ui.Common
{
    public abstract class ViewModel : Screen
    {

        private string[] _propertyNames;

        public virtual void TriggerPropertyChangedWithCanExecute<TProperty>(Expression<Func<TProperty>> property)
        {
            NotifyOfPropertyChange(property.GetMemberInfo().Name);
            TriggerCanExecuteCommands();

        }

        public void TriggerCanExecuteCommands()
        {
            //bumps the Can[propertyname] for Caliburn Micro
            _propertyNames = _propertyNames ?? GetType().GetProperties()
              .Where(property => property.Name.StartsWith("Can"))
              .Select(x => x.Name).ToArray();

            foreach (var name in _propertyNames)
                NotifyOfPropertyChange(name);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy; set
            {
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        public IDisposable SetIsBusy()
        {
            IsBusy = true;
            return new DisposableActionInvoker(() =>
            {
                Execute.OnUIThreadAsync(() => { IsBusy = false; });
            });
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading; set
            {
                _isLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        public IDisposable SetIsLoading()
        {
            IsLoading = true;
            return new DisposableActionInvoker(() =>
            {
                Execute.OnUIThreadAsync(() => { IsLoading = false; });
            });
        }

    }
}
