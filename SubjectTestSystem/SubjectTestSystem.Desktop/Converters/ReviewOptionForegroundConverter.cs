using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using SubjectTestSystem.Shared.Models;

namespace SubjectTestSystem.Desktop.Converters;

public class ReviewOptionForegroundConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count >= 2 && values[0] is OptionItem option && values[1] is TestItem question)
        {
            int optionIndexInShuffled = Array.IndexOf(question.ShuffledOptions, option);
            
            bool isCorrect = optionIndexInShuffled == question.CorrectAnswerIndex;
            bool isSelected = optionIndexInShuffled == question.SelectedOptionIndex;

            if (isCorrect)
            {
                return Brush.Parse("#2E7D32"); // Dark Green for correct text
            }
            
            if (isSelected && !isCorrect)
            {
                return Brush.Parse("#C62828"); // Dark Red for wrong selection text
            }
        }

        return null; // Use default
    }
}
