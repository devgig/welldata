using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Threading.Tasks;
using WellData.Core.Extensions;
using WellData.Core.Services.Data;
using WellData.Core.Services.Models;
using WellData.Ui.Common;

namespace WellData.Ui.Screens
{
    public class ShellViewModel : ViewModel
    {
        private readonly IMessageBoxManager _messageBoxManager;
        private readonly IWellDataImporter _wellDataImporter;
        private readonly IWellProvider _wellProvider;
        private readonly ITankProvider _tankProvider;
        private readonly PropertyObserver<ShellViewModel> _propertyObserver;

        public ShellViewModel(
            IMessageBoxManager messageBoxManager,
            IWellDataImporter wellDataImporter,
            IWellProvider wellProvider,
            ITankProvider tankProvider)
        {
            _messageBoxManager = messageBoxManager;
            _wellDataImporter = wellDataImporter;
            _wellProvider = wellProvider;
            _tankProvider = tankProvider;
            WellItems = new BindableCollection<WellModel>();
            TankItems = new BindableCollection<TankModel>();
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
            _propertyObserver = new PropertyObserver<ShellViewModel>(this);

            _ = _propertyObserver.OnChangeOf(x => x.SelectedWell).Do((vm) => LoadTanks(vm.SelectedWell).ConfigureAwait(false));

        }

        private async Task LoadTanks(WellModel well)
        {
            if (well == null) return;

            using (SetIsBusy())
            {
                //checks for any dirty models and saves those before reloading 
                var dirty = TankItems.Where(x => x.IsDirty()).ToArray();
                if (dirty.Any())
                {
                    var tanks = await Task.Run(() => _tankProvider.Save(dirty));
                    await Task.Factory.StartNew(() => MessageQueue.Enqueue($"{tanks} record(s) updated."));
                }

                Execute.OnUIThread(() => TankItems.Clear());
                var results = await Task.Run(() => _tankProvider.GetByWellId(well.Id));
                await Execute.OnUIThreadAsync(() => TankItems.AddRange(results));
            }
        }

        private async Task LoadWells()
        {
            Execute.OnUIThread(() => WellItems.Clear());
            using (SetIsBusy())
            {
                //dirty data checking is not needed for this exercise but would be need if data was going to be saved.
                var wells = await Task.Run(() => _wellProvider.GetAll());
                await Execute.OnUIThreadAsync(() => WellItems.AddRange(wells.ToArray()));
            }
        }

        public BindableCollection<WellModel> WellItems { get; set; }
        public BindableCollection<TankModel> TankItems { get; set; }

        private WellModel selectedWell;


        public WellModel SelectedWell
        {
            get => selectedWell; set
            {
                selectedWell = value;
                NotifyOfPropertyChange(() => SelectedWell);
            }
        }

        private SnackbarMessageQueue messageQueue;
        public SnackbarMessageQueue MessageQueue
        {
            get => messageQueue; set
            {
                messageQueue = value;
                NotifyOfPropertyChange(() => MessageQueue);
            }
        }


        public async Task ImportFileCommand()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = @"Excel files(*.xlsx)|*.xlsx|CSV (Comma delimited) files(*.csv)|*.csv",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            var uploadFile = openFileDialog.ShowDialog().GetValueOrDefault() ? openFileDialog.FileName : string.Empty;
            if (uploadFile.IsNullOrEmpty())
            {
                await Task.Factory.StartNew(() => MessageQueue.Enqueue("No file selected."));
            }
            else
            {
                using (SetIsLoading())
                {
                    using (SetIsBusy())
                    {
                        //not doing anything with the return for now.  Might add something later
                        var results = await Task.Run(() => _wellDataImporter.Upload(uploadFile));
                        await Task.Factory.StartNew(() => MessageQueue.Enqueue($"{results} record(s) loaded."));
                        await LoadWells();
                    }
                }
            }


        }

    }
}
