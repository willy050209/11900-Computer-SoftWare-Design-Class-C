using System;
using System.Globalization;
using Avalonia.Data.Converters;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SubjectTestSystem.Shared.Models;

namespace SubjectTestSystem.Desktop.Converters;

public class IndexToDisplayConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int index)
        {
            return (index + 1).ToString();
        }

        if (value is IEnumerable items && parameter is TestItem item)
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
        
        return "0";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
