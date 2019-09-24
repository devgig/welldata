using WellData.Core.Services.Models;
using WellData.Ui.Common;

namespace WellData.Ui.Screens
{
    public class AddWellViewModel : ViewModel
    {
        private readonly IRecieveNotifyOnAdd _notify;
        private readonly IWellProvider _wellProvider;


        public AddWellViewModel(IRecieveNotifyOnAdd notify, IWellProvider wellProvider)
        {
            _wellProvider = wellProvider;
            _notify = notify;
        }

       
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        public void NotifyCommand()
        {
            _notify.Notify();
        }
    }
}
