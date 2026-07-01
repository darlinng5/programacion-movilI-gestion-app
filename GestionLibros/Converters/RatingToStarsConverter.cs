using System.Globalization;

namespace GestionLibros.Converters
{
    public class RatingToStarsConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int rating = value is int i ? Math.Clamp(i, 0, 5) : 0;
            return new string('★', rating) + new string('☆', 5 - rating);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
