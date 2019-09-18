using Caliburn.Micro;
using System.Threading.Tasks;
using WellData.Core.Services.Data;
using WellData.Ui.Common;

namespace WellData.Ui.Screens
{
    public class ShellViewModel : Screen
    {
        private readonly IMessageBoxManager _messageBoxManager;
        private readonly IWellDataImporter _wellDataImporter;

        public ShellViewModel(
            IMessageBoxManager messageBoxManager,
            IWellDataImporter wellDataImporter)
        {
            _messageBoxManager = messageBoxManager;
            _wellDataImporter = wellDataImporter;
        }

        public async Task ImportFileCommand()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = @"Excel file (*.xlsx)|*.xlsx",
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
                var finished = await _wellDataImporter.Upload(uploadFile);
                if (finished)
                {
                    _messageBoxManager.ShowInformation("File uploaded");
                }
            }

        }

    }
}
