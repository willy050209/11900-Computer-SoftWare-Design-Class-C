using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SubjectTestSystem.Shared.Models;
using SubjectTestSystem.Shared.Services;

namespace SubjectTestSystem.Desktop.Services;

/// <summary>
/// Repository to load questions from a local JSON file.
/// </summary>
public sealed class FileQuestionRepository : IQuestionRepository
{
    private const string FileName = "questions.json";
    private readonly string _filePath;

    public FileQuestionRepository()
    {
        // Use AppContext.BaseDirectory for reliable path resolution in different publishing modes
        _filePath = Path.Combine(AppContext.BaseDirectory, FileName);
        
        // Fallback for development (checking root project dir)
        if (!File.Exists(_filePath))
        {
            var devPath = Path.Combine(Directory.GetCurrentDirectory(), FileName);
            if (File.Exists(devPath)) _filePath = devPath;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<QuestionModel>> GetAllQuestionsAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException($"The questions file was not found at {_filePath}");
        }

        using var stream = File.OpenRead(_filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var questions = await JsonSerializer.DeserializeAsync<IEnumerable<QuestionModel>>(stream, options, ct);
        return questions ?? [];
    }
}
