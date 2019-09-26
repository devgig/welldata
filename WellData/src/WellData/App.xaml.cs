using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WellData.Ui.Behaviours;
using WellData.Ui.PInvokeHelpers;

namespace WellData
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex _instanceMutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;
            _instanceMutex = new Mutex(true, Assembly.GetEntryAssembly().FullName, out createdNew);
            if (!createdNew)
            {
                _instanceMutex = null;
                WindowMessageHelper.PostMessage(-1, SingleInstanceBehavior.ShowWindowMessage, 0, 0);
              
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_instanceMutex != null)
                _instanceMutex.ReleaseMutex();
            base.OnExit(e);
        }
    }
}
