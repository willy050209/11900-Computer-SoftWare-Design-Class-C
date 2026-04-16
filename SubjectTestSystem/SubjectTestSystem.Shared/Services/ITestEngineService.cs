using SubjectTestSystem.Shared.Models;

namespace SubjectTestSystem.Shared.Services;

/// <summary>
/// Service interface for test generation and evaluation.
/// </summary>
public interface ITestEngineService
{
    /// <summary>
    /// Generates a new test session with randomized questions and shuffled options.
    /// </summary>
    /// <param name="allQuestions">The source questions pool.</param>
    /// <param name="professionalCount">Number of professional questions to include.</param>
    /// <param name="commonCount">Number of common questions to include.</param>
    /// <returns>A collection of TestItems.</returns>
    IEnumerable<TestItem> GenerateTest(IEnumerable<QuestionModel> allQuestions, int professionalCount = 60, int commonCount = 20);

    /// <summary>
    /// Calculates the score for a completed test session.
    /// </summary>
    /// <param name="testItems">The test items with user answers.</param>
    /// <returns>Total score (0-100).</returns>
    double CalculateScore(IEnumerable<TestItem> testItems);
}
