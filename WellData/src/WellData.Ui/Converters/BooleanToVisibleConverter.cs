using System;
using System.Windows;
using System.Windows.Data;
using WellData.Core.Data.Extensions;

namespace WellData.Ui.Converters
{
    public class BooleanToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value.IsNotNull() && ((bool?)value).HasValue && ((bool?)value).Value.Equals(true)) 
                ? Visibility.Visible 
                : (parameter.IsNotNull() && parameter.ToString().ToUpper().Equals("COLLAPSED")) 
                ? Visibility.Collapsed 
                : Visibility.Hidden;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
