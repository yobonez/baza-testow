using System.Globalization;

namespace TestyMAUI.Converters;

public class BoolInverterConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool boolValue ? !boolValue : throw new ArgumentException("Value must be a boolean.");

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool boolValue ? !boolValue : throw new ArgumentException("Value must be a boolean.");
}
