using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RunTracker.Converter
{
    public class StringToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                return timeSpan.ToString(@"hh\:mm\:ss");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && TimeSpan.TryParseExact(stringValue, @"hh\:mm\:ss", culture, out TimeSpan timeSpan))
            {
                return timeSpan;
            }
            return TimeSpan.Zero; // eller annan standardvärde
        }
    }
}
