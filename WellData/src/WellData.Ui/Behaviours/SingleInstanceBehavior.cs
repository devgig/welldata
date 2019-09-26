using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;
using WellData.Ui.PInvokeHelpers;

namespace WellData.Ui.Behaviours
{
    public class SingleInstanceBehavior : Behavior<Window>
    {
        static SingleInstanceBehavior()
        {
            ShowWindowMessage = WindowMessageHelper.RegisterWindowMessage(SHOWWINDOW_MESSAGE);
        }
        const string SHOWWINDOW_MESSAGE = "WellDataShowWindow";
        public static int ShowWindowMessage;

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += (s, e) =>
            {
                WireUpWndProc();
            };
        }

        protected override void OnDetaching()
        {
            RemoveWndProc();

            base.OnDetaching();
        }
        private HwndSourceHook _hook;

        private void WireUpWndProc()
        {
            var source = HwndSource.FromVisual(AssociatedObject) as HwndSource;

            if (source != null)
            {
                _hook = new HwndSourceHook(WndProc);
                source.AddHook(_hook);
            }
        }

        private void RemoveWndProc()
        {
            var source = HwndSource.FromVisual(AssociatedObject) as HwndSource;

            if (source != null)
            {
                source.RemoveHook(_hook);
            }
        }
        private IntPtr WndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
        {
            IntPtr result = IntPtr.Zero;

            if (msg == ShowWindowMessage)
                ShowWindow();

            return result;
        }

        private void ShowWindow()
        {
            AssociatedObject.WindowState = WindowState.Minimized;
            AssociatedObject.WindowState = WindowState.Normal;
        }
    }
}
