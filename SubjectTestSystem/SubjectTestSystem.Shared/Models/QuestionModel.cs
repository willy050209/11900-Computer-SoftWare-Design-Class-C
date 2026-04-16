namespace SubjectTestSystem.Shared.Models;

/// <summary>
/// Represents a raw question from the JSON data source.
/// </summary>
public record QuestionModel(
    string Category,
    string WorkItem,
    int Id,
    string Question,
    string[] Options,
    int Answer
);
