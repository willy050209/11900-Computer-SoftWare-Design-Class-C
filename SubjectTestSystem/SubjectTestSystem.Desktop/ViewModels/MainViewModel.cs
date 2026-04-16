using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SubjectTestSystem.Shared.Models;
using SubjectTestSystem.Shared.Services;

using Avalonia.Threading;

namespace SubjectTestSystem.Desktop.ViewModels;

public enum AppState
{
    Home,
    Testing,
    Result,
    Review
}

public partial class MainViewModel : ObservableObject
{
    private readonly IQuestionRepository _repository;
    private readonly ITestEngineService _testEngine;
    private readonly DispatcherTimer _timer;
    private DateTime _startTime;

    [ObservableProperty]
    private AppState _currentState = AppState.Home;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentQuestion))]
    [NotifyCanExecuteChangedFor(nameof(GoNextCommand))]
    private ObservableCollection<TestItem> _testItems = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentQuestion))]
    [NotifyCanExecuteChangedFor(nameof(GoPreviousCommand))]
    [NotifyCanExecuteChangedFor(nameof(GoNextCommand))]
    private int _currentQuestionIndex = 0;

    public TestItem? CurrentQuestion => TestItems.Count > CurrentQuestionIndex ? TestItems[CurrentQuestionIndex] : null;

    [ObservableProperty]
    private double _score;

    [ObservableProperty]
    private int _correctCount;

    [ObservableProperty]
    private int _incorrectCount;

    [ObservableProperty]
    private string _elapsedTime = "00:00:00";

    public MainViewModel(IQuestionRepository repository, ITestEngineService testEngine)
    {
        _repository = repository;
        _testEngine = testEngine;
        
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += (s, e) => UpdateElapsedTime();
    }

    private void UpdateElapsedTime()
    {
        if (CurrentState == AppState.Testing)
        {
            var duration = DateTime.Now - _startTime;
            ElapsedTime = duration.ToString(@"hh\:mm\:ss");
        }
    }

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _showSubmitConfirmation;

    [ObservableProperty]
    private int _unansweredCount;

    [ObservableProperty]
    private int _markedCount;

    [RelayCommand]
    private async Task StartTestAsync()
    {
        ErrorMessage = null;
        try
        {
            var allQuestions = (await _repository.GetAllQuestionsAsync()).ToList();
            if (!allQuestions.Any())
            {
                ErrorMessage = "題庫檔案為空或載入失敗。";
                return;
            }

            var selectedItems = _testEngine.GenerateTest(allQuestions).ToList();
            if (!selectedItems.Any())
            {
                ErrorMessage = "無法從題庫中抽選出有效題目。";
                return;
            }

            TestItems = new ObservableCollection<TestItem>(selectedItems);
            CurrentQuestionIndex = 0;
            CurrentState = AppState.Testing;
            
            _startTime = DateTime.Now;
            ElapsedTime = "00:00:00";
            _timer.Start();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"發生錯誤: {ex.Message}";
            Console.WriteLine(ex);
        }
    }

    [RelayCommand(CanExecute = nameof(CanGoPrevious))]
    private void GoPrevious() => CurrentQuestionIndex--;

    private bool CanGoPrevious() => CurrentQuestionIndex > 0;

    [RelayCommand(CanExecute = nameof(CanGoNext))]
    private void GoNext() => CurrentQuestionIndex++;

    private bool CanGoNext() => CurrentQuestionIndex < TestItems.Count - 1;

    [RelayCommand]
    private void ToggleMark()
    {
        if (CurrentQuestion != null)
        {
            CurrentQuestion.IsMarked = !CurrentQuestion.IsMarked;
        }
    }

    [RelayCommand]
    private void RequestSubmit()
    {
        UnansweredCount = TestItems.Count(i => !i.IsAnswered);
        MarkedCount = TestItems.Count(i => i.IsMarked);
        ShowSubmitConfirmation = true;
    }

    [RelayCommand]
    private void CancelSubmit()
    {
        ShowSubmitConfirmation = false;
    }

    [RelayCommand]
    private void ConfirmSubmit()
    {
        ShowSubmitConfirmation = false;
        _timer.Stop();
        
        CorrectCount = TestItems.Count(i => i.IsCorrect);
        IncorrectCount = TestItems.Count(i => !i.IsCorrect);
        
        Score = _testEngine.CalculateScore(TestItems);
        CurrentState = AppState.Result;
    }

    [RelayCommand]
    private void ReviewTest()
    {
        CurrentQuestionIndex = 0;
        CurrentState = AppState.Review;
    }

    [RelayCommand]
    private void BackToHome()
    {
        _timer.Stop();
        CurrentState = AppState.Home;
        TestItems.Clear();
    }
}
