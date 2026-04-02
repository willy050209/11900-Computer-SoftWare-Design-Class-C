namespace IdCardChecker.Services;

using IdCardChecker.Models;
using System.IO;

public interface IDataService
{
    Task<IEnumerable<IdCardRecord>> ReadRecordsAsync(string filePath);
}

public class DataService : IDataService
{
    public async Task<IEnumerable<IdCardRecord>> ReadRecordsAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return Enumerable.Empty<IdCardRecord>();
        }

        var lines = await File.ReadAllLinesAsync(filePath);
        return lines
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .SelectMany(line =>
            {
                var parts = line.Split(',', StringSplitOptions.TrimEntries);
                if (parts.Length >= 3)
                {
                    return [new IdCardRecord(parts[0], parts[1], parts[2])];
                }
                return Enumerable.Empty<IdCardRecord>();
            });
    }
}
