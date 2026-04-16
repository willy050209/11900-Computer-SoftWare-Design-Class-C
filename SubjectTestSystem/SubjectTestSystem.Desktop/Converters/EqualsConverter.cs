using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SubjectTestSystem.Desktop.Converters;

public class EqualsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null) return false;

        // If both are same type, use standard Equals
        if (value.GetType() == parameter.GetType())
        {
            return value.Equals(parameter);
        }

        // If one is Enum and other is string (common in XAML)
        if (value.GetType().IsEnum)
        {
            return value.ToString() == parameter.ToString();
        }

        return value.Equals(parameter);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.Equals(true) == true ? parameter : null;
    }
}
