using System;
using System.Globalization;
using System.Windows.Data;


namespace RunTracker.Converter
{
    public class StringToFloatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            // Om det är en float eller null, returnera dess strängrepresentation.
            return ((float?)value)?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Försök att konvertera tillbaka texten till float, hantera både komma och punkt
            string input = value as string;
            if (string.IsNullOrEmpty(input))
                return null;

            float result;
            if (float.TryParse(input, NumberStyles.Float, culture, out result))
                return result;

            // Om parsningen misslyckas, returnera null
            return null;
        }
    }
}

