// filepath: Models/TestModels.cs
namespace Task1Tester.Models;

/// <summary>
/// Represents the overall result of the automation test.
/// </summary>
public record TestResult(
    bool PdfHeaderValid,
    bool PdfContentValid,
    bool CodeValid,
    ImmutableArray<Violation> Violations);

/// <summary>
/// Represents a specific rule violation in either code or PDF.
/// </summary>
public record Violation(string Category, string Message);

/// <summary>
/// Required Header Information for validation.
/// </summary>
public record HeaderInfo(string Name, string TestNo, string SeatNo);
