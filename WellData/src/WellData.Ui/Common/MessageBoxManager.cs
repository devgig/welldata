using System;
using System.Windows;

namespace WellData.Ui.Common
{
    public interface IMessageBoxManager
    {
        void ShowException(Exception exception);
        void ShowInformation(string info);

    }
    public class MessageBoxManager : IMessageBoxManager
    {
        public void ShowException(Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Error Occurred");
        }

        public void ShowInformation(string info)
        {
            MessageBox.Show(info);
        }
    }
}
