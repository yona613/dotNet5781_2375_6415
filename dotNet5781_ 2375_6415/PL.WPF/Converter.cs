using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PL.WPF
{
    internal class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((TimeSpan)value <= new TimeSpan(0,3,0))
            {
                return Brushes.Red;
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
