using System;
using System.Globalization;
using Avalonia.Data.Converters;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SubjectTestSystem.Shared.Models;

namespace SubjectTestSystem.Desktop.Converters;

public class IndexToDisplayConverter : IMultiValueConverter, IValueConverter
{
    // For IValueConverter (direct int index)
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int index)
        {
            return (index + 1).ToString();
        }
        return "0";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    // For IMultiValueConverter (collection and item)
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count >= 2 && values[0] is IEnumerable items && values[1] is TestItem item)
        {
            int foundIndex = 0;
            foreach (var current in items)
            {
                if (ReferenceEquals(current, item))
                {
                    return (foundIndex + 1).ToString();
                }
                foundIndex++;
            }
        }
        
        if (values.Count >= 1 && values[0] is int index)
        {
            return (index + 1).ToString();
        }

        return "0";
    }
}
