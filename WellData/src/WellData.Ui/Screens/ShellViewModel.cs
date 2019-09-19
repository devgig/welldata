using Caliburn.Micro;
using System.Linq;
using System.Threading.Tasks;
using WellData.Core.Services.Data;
using WellData.Core.Services.Models;
using WellData.Ui.Common;

namespace WellData.Ui.Screens
{
    public class ShellViewModel : Screen
    {
        private readonly IMessageBoxManager _messageBoxManager;
        private readonly IWellDataImporter _wellDataImporter;
        private readonly IWellProvider _wellProvider;
        private readonly ITankProvider _tankProvider;
        private readonly PropertyObserver<ShellViewModel> _propertyObserver;
        private WellModel selectedWell;

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

            _propertyObserver.OnChangeOf(x => x.SelectedWell).Do((vm) => LoadTanks(vm.SelectedWell));

        }

        private void LoadTanks(WellModel well)
        {
            if (well == null) return;
            TankItems.Clear();
            TankItems.AddRange(_tankProvider.GetByWellId(well.Id));
        }

        private void LoadWells()
        {
            WellItems.Clear();
            var wells = _wellProvider.GetAll().ToArray();
            WellItems.AddRange(wells);
        }

        public BindableCollection<WellModel> WellItems { get; set; }
        public BindableCollection<TankModel> TankItems { get; set; }

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
                Filter = @"CSV (Comma delimited) files(*.csv)|*.csv|Excel files(*.xlsx)|*.xlsx",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            var uploadFile = openFileDialog.ShowDialog().GetValueOrDefault() ? openFileDialog.FileName : string.Empty;
            if (string.IsNullOrEmpty(uploadFile))
            {
                _messageBoxManager.ShowInformation("No file selected.");
            }
            else
            {
                var results = await _wellDataImporter.Upload(uploadFile);

                //_messageBoxManager.ShowInformation($"{results} records uploaded.");
                Execute.OnUIThread(() => LoadWells());
            }

        }

    }
}
