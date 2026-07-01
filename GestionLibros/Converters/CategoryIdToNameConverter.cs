using System.Collections.Generic;
using System.Globalization;

namespace GestionLibros.Converters
{
    public class CategoryIdToNameConverter : IValueConverter
    {
        public static Dictionary<int, string> CategoryNames { get; set; } = new Dictionary<int, string>();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int categoryId && CategoryNames.TryGetValue(categoryId, out var name))
            {
                return name;
            }

            return "Sin categoría";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
