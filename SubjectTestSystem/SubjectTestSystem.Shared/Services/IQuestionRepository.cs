using SubjectTestSystem.Shared.Models;

namespace SubjectTestSystem.Shared.Services;

/// <summary>
/// Service interface for retrieving questions.
/// </summary>
public interface IQuestionRepository
{
    /// <summary>
    /// Loads all available questions asynchronously.
    /// </summary>
    /// <returns>A collection of all questions.</returns>
    Task<IEnumerable<QuestionModel>> GetAllQuestionsAsync(CancellationToken ct = default);
}
