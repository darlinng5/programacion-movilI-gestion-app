using System.Globalization;
using GestionLibros.Models;
using Microsoft.Maui.Graphics;

namespace GestionLibros.Converters
{
    public class ReadingStatusToLabelConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is ReadingStatus status
                ? status switch
                {
                    ReadingStatus.PorLeer => "Por leer",
                    ReadingStatus.Leyendo => "Leyendo",
                    ReadingStatus.Leido => "Leído",
                    _ => status.ToString()
                }
                : string.Empty;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ReadingStatusToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is ReadingStatus status
                ? status switch
                {
                    ReadingStatus.PorLeer => Color.FromArgb("#9CA3AF"),
                    ReadingStatus.Leyendo => Color.FromArgb("#F59E0B"),
                    ReadingStatus.Leido => Color.FromArgb("#22C55E"),
                    _ => Color.FromArgb("#9CA3AF")
                }
                : Color.FromArgb("#9CA3AF");
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
