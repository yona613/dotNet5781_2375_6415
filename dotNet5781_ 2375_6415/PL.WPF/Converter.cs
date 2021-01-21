using System;
using System.Globalization;
using System.Media;
using System.Windows.Data;
using System.Windows.Media;

namespace PL.WPF
{
    /// <summary>
    /// Class implementing conversion from time span to color
    /// </summary>
    internal class TimeSpanConverter : IValueConverter
    {
        /// <summary>
        /// Function to convert 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((TimeSpan)value <= new TimeSpan(0,3,0))
            {
                PlaySound(@"button-42.wav");
                return Brushes.Red;
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static void PlaySound(string path)
        {
            var sp = new SoundPlayer(path);
            sp.Load();
            sp.Play();
        }
    }
}
