namespace PokerGame.Services;

using System.IO;

public interface IDataService
{
    Task<(int rounds, List<double> randomNumbers)> ReadDataAsync(string filePath);
}

public class DataService : IDataService
{
    public async Task<(int rounds, List<double> randomNumbers)> ReadDataAsync(string filePath)
    {
        if (!File.Exists(filePath)) return (0, new List<double>());

        var lines = await File.ReadAllLinesAsync(filePath);
        if (lines.Length == 0) return (0, new List<double>());

        if (!int.TryParse(lines[0], out int rounds)) rounds = 0;

        var randomNumbers = lines.Skip(1)
            .Select(line => double.TryParse(line, out double val) ? val : -1.0)
            .Where(val => val >= 0)
            .ToList();

        return (rounds, randomNumbers);
    }
}
