using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using SubjectTestSystem.Shared.Models;

namespace SubjectTestSystem.Desktop.Converters;

public class ReviewOptionColorConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        // Expected values: [OptionItem currentOption, TestItem currentQuestion]
        if (values.Count >= 2 && values[0] is OptionItem option && values[1] is TestItem question)
        {
            int optionIndexInShuffled = Array.IndexOf(question.ShuffledOptions, option);
            
            bool isCorrect = optionIndexInShuffled == question.CorrectAnswerIndex;
            bool isSelected = optionIndexInShuffled == question.SelectedOptionIndex;

            if (isCorrect)
            {
                return Brush.Parse("#E8F5E9"); // Light Green background for correct answer
            }
            
            if (isSelected && !isCorrect)
            {
                return Brush.Parse("#FFEBEE"); // Light Red background for wrongly selected answer
            }
        }

        return Brushes.Transparent;
    }
}
