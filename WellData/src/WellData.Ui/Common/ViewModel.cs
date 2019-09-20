using Caliburn.Micro;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace WellData.Ui.Common
{
    public abstract class ViewModel : Screen
    {

        private string[] propertyNames;

        public virtual void TriggerPropertyChangedWithCanExecute<TProperty>(Expression<Func<TProperty>> property)
        {
            NotifyOfPropertyChange(property.GetMemberInfo().Name);
            TriggerCanExecuteCommands();

        }

        public void TriggerCanExecuteCommands()
        {
            //bumps the Can[propertyname] for Caliburn Micro
            propertyNames = propertyNames ?? GetType().GetProperties()
              .Where(property => property.Name.StartsWith("Can"))
              .Select(x => x.Name).ToArray();

            foreach (var name in propertyNames)
                NotifyOfPropertyChange(name);
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy; set
            {
                isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        public IDisposable SetIsBusy()
        {
            Execute.OnUIThreadAsync(() => { IsBusy = true; });
            return new DisposableActionInvoker(() =>
            {
                Execute.OnUIThreadAsync(() => { IsBusy = false; });
            });
        }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading; set
            {
                isLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        public IDisposable SetIsLoading()
        {
            Execute.OnUIThreadAsync(() => { IsLoading = true; });
            return new DisposableActionInvoker(() =>
            {
                Execute.OnUIThreadAsync(() => { IsLoading = false; });
            });
        }

    }
}
