using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SubjectTestSystem.Desktop.Converters;

public class IndexToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int selectedIndex && parameter is int index)
        {
            return selectedIndex == index;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isChecked && isChecked && parameter is int index)
        {
            return index;
        }
        return null;
    }
}
