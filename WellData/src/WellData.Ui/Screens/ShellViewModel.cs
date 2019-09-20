﻿using Caliburn.Micro;
using System.Linq;
using System.Threading.Tasks;
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
            _propertyObserver = new PropertyObserver<ShellViewModel>(this);

            _ = _propertyObserver.OnChangeOf(x => x.SelectedWell).Do((vm) => LoadTanks(vm.SelectedWell).ConfigureAwait(false));

        }

        private async Task LoadTanks(WellModel well)
        {
            if (well == null) return;
            
            using (SetIsBusyWhileExecuting())
            {
                //checks for any dirty models and saves those before reloading 
                var dirty = TankItems.Where(x => x.IsDirty()).ToArray();
                if(dirty.Any())
                    await Task.Run(() => _tankProvider.Save(dirty));
                
                TankItems.Clear();
                var results = await Task.Run(() => _tankProvider.GetByWellId(well.Id));
                await Execute.OnUIThreadAsync(() => TankItems.AddRange(results));
            }
        }

        private async Task LoadWells()
        {
            WellItems.Clear();
            using (SetIsBusyWhileExecuting())
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
            using (SetIsBusyWhileExecuting())
            {
                //not doing anything with the return for now.  Might add something later
                var results = await Task.Run(() => _wellDataImporter.Upload(uploadFile));
                await LoadWells();
            }

        }

    }
}
