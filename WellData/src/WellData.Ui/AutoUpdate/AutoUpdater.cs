using System;
using System.Configuration;
using System.Linq;
using Squirrel;
using WellData.Ui.Common;

namespace WellData.Ui.AutoUpdate
{
    public interface IAutoUpdater
    {
        void CheckAndUpdate(ViewModel viewModel);
        void HandleUpdateEvents();
    }

    public class AutoUpdater : IAutoUpdater
    {
        private readonly IMessageBoxManager _messageBoxManager;

        public AutoUpdater(
        IMessageBoxManager messageBoxManager)
        {
            _messageBoxManager = messageBoxManager;
        }

        public void HandleUpdateEvents()
        {
#if !DEBUG
            try
            {
                using (var mgr = new UpdateManager(ConfigurationManager.AppSettings["updateUrl"]))
                {
                    // Note, in most of these scenarios, the app exits after this method
                    // completes!
                    var updateManager = mgr;
                    SquirrelAwareApp.HandleEvents(
                      onInitialInstall: v => mgr.CreateShortcutForThisExe(),
                      onAppUpdate: v => mgr.CreateShortcutForThisExe(),
                      onAppUninstall: v => mgr.RemoveShortcutForThisExe());

                }

            }
            catch
            {
                
            }
#endif
        }

        public async void CheckAndUpdate(ViewModel viewModel)
        {

            try
            {
                using (var mgr = new UpdateManager(ConfigurationManager.AppSettings["updateUrl"]))
                {
                    UpdateInfo updates = null;
                    try
                    {
                        updates = await mgr.CheckForUpdate();

                    }
                    catch
                    {
                        //Ignore issues with getting to update url
                    }
                    if (!(updates?.ReleasesToApply?.Any() ?? false)) return;

                    var result =
                     _messageBoxManager.Confirm(
                         "Updates exist.  Would you like to update to the latest version on next execution?",
                         "Application Updates");


                    using (viewModel.SetIsLoading())
                    {
                        using (viewModel.SetIsBusy())
                        {
                            try
                            {
                                await mgr.UpdateApp();
                            }
                            catch (Exception e)
                            {
                                _messageBoxManager.ShowException(e);
                            }

                        }
                    }

                }

            }
            catch
            {
                //Ignore issues with getting to update url
            }
        }
    }
}