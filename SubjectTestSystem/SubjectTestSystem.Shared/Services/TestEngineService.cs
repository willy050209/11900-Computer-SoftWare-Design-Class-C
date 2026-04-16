using System;
using System.Collections.Generic;
using System.Linq;
using SubjectTestSystem.Shared.Models;

namespace SubjectTestSystem.Shared.Services;

/// <summary>
/// Core engine for test generation and scoring logic.
/// </summary>
public sealed class TestEngineService : ITestEngineService
{
    /// <inheritdoc />
    public IEnumerable<TestItem> GenerateTest(IEnumerable<QuestionModel> allQuestions, int professionalCount = 60, int commonCount = 20)
    {
        ArgumentNullException.ThrowIfNull(allQuestions);

        // Filter questions that actually have options to prevent crashes
        var questionsList = allQuestions
            .Where(q => q.Options is { Length: > 0 } && q.Answer >= 1 && q.Answer <= q.Options.Length)
            .ToList();

        // 1. Separate questions by category (using Contains for robustness)
        var professionalPool = questionsList.Where(q => q.Category.Contains("專業")).ToList();
        var commonPool = questionsList.Where(q => q.Category.Contains("共同")).ToList();

        if (professionalPool.Count == 0 && commonPool.Count == 0)
        {
            throw new InvalidOperationException("無法在題庫中找到有效的題目，請檢查 questions.json 的類別名稱。");
        }

        // 2. Randomly select questions
        var selectedProfessional = professionalPool.OrderBy(_ => Guid.NewGuid()).Take(Math.Min(professionalCount, professionalPool.Count));
        var selectedCommon = commonPool.OrderBy(_ => Guid.NewGuid()).Take(Math.Min(commonCount, commonPool.Count));

        var finalPool = selectedProfessional.Concat(selectedCommon).OrderBy(_ => Guid.NewGuid()).ToList();

        if (finalPool.Count == 0)
        {
            throw new InvalidOperationException("隨機抽選後的題目數量為 0。");
        }

        // 3. Create TestItems with shuffled options
        return finalPool.Select(CreateTestItem).ToList();
    }

    /// <inheritdoc />
    public double CalculateScore(IEnumerable<TestItem> testItems)
    {
        ArgumentNullException.ThrowIfNull(testItems);
        var items = testItems.ToList();
        if (items.Count == 0) return 0;

        int correctCount = items.Count(i => i.IsCorrect);
        return (double)correctCount / items.Count * 100.0;
    }

    /// <summary>
    /// Pure function to create a TestItem from a QuestionModel, shuffling its options.
    /// </summary>
    private static TestItem CreateTestItem(QuestionModel original)
    {
        // Shuffle and wrap in OptionItem objects
        var shuffledOptions = original.Options
            .Select((text, i) => new OptionItem(i, text))
            .OrderBy(_ => Guid.NewGuid())
            .ToArray();

        // The original.Answer is 1-based index (1-4).
        string correctAnswerText = original.Options[original.Answer - 1];

        // Find the new 0-based index in the shuffled array where the correct text resides.
        // In the UI, the index will refer to the position in the *shuffled* array.
        int newCorrectIndex = -1;
        for (int i = 0; i < shuffledOptions.Length; i++)
        {
            if (shuffledOptions[i].Text == correctAnswerText)
            {
                newCorrectIndex = i;
                break;
            }
        }

        return new TestItem(original, shuffledOptions, newCorrectIndex);
    }
}
