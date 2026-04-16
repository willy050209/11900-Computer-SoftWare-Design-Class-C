using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SubjectTestSystem.Desktop.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool b = value is bool v && v;
        if (parameter is string s && s == "invert")
        {
            b = !b;
        }
        return b;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
