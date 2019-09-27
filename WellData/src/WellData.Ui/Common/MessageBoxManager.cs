using System;
using System.Windows;

namespace WellData.Ui.Common
{
    public interface IMessageBoxManager
    {
        void ShowException(Exception exception);
        void ShowInformation(string info);
        bool Confirm(string message, string title);

    }
    public class MessageBoxManager : IMessageBoxManager
    {
        public bool Confirm(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes ? true : false;
        }

        public void ShowException(Exception exception)
        {
            MessageBox.Show(exception?.Message, "Error Occurred");
        }

        public void ShowInformation(string info)
        {
            MessageBox.Show(info);
        }
    }
}
