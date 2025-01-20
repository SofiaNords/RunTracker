using MongoDB.Bson;
using RunTracker.Model;
using System.Globalization;
using System.Windows.Data;

namespace RunTracker.Converter
{
    public class RunTypeToObjectIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var runType = value as RunType;
            return runType?.Id; // Om RunType har en Id egenskap som är av typen ObjectId
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var objectId = value as ObjectId?;
            if (objectId.HasValue)
            {
                // Skapa en ny RunType baserat på ObjectId, om det behövs
                return new RunType { Id = objectId.Value };
            }
            return null;
        }
    }

}
