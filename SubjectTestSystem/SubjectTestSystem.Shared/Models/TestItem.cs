using CommunityToolkit.Mvvm.ComponentModel;

namespace SubjectTestSystem.Shared.Models;

/// <summary>
/// Represents a single option for a question.
/// </summary>
public record OptionItem(int Index, string Text);

/// <summary>
/// Represents a question in an active test session with shuffled options.
/// </summary>
public partial class TestItem : ObservableObject
{
    public QuestionModel OriginalQuestion { get; }
    public OptionItem[] ShuffledOptions { get; }
    public int CorrectAnswerIndex { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedOption))]
    [NotifyPropertyChangedFor(nameof(IsCorrect))]
    [NotifyPropertyChangedFor(nameof(IsAnswered))]
    private int _selectedOptionIndex = -1;

    public OptionItem? SelectedOption => (SelectedOptionIndex >= 0 && SelectedOptionIndex < ShuffledOptions.Length)
        ? ShuffledOptions[SelectedOptionIndex]
        : null;

    public bool IsCorrect => SelectedOptionIndex == CorrectAnswerIndex;

    public bool IsAnswered => SelectedOptionIndex != -1;

    [ObservableProperty]
    private bool _isMarked;

    public void SelectOption(int index)
    {
        SelectedOptionIndex = index;
    }

    public TestItem(QuestionModel originalQuestion, OptionItem[] shuffledOptions, int correctAnswerIndex)
    {
        OriginalQuestion = originalQuestion;
        ShuffledOptions = shuffledOptions;
        CorrectAnswerIndex = correctAnswerIndex;
    }
}
