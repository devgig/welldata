using System;
using System.Diagnostics.Contracts;
using WellData.Core.Data.Extensions;

namespace WellData.Ui.Common
{
    public class DisposableActionInvoker : IDisposable
    {
        readonly Action _onDispose;

        public DisposableActionInvoker(Action onDispose)
        {
            Contract.Requires(onDispose.IsNotNull());
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            _onDispose();
        }
    }
}
