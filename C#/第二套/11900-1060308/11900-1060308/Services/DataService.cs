namespace FractionArithmetic.Services;

using FractionArithmetic.Models;
using System.IO;

public interface IDataService
{
    Task<IEnumerable<(Fraction f1, string op, Fraction f2)>> ReadDataAsync(string filePath);
}

public class DataService : IDataService
{
    public async Task<IEnumerable<(Fraction f1, string op, Fraction f2)>> ReadDataAsync(string filePath)
    {
        if (!File.Exists(filePath)) return Enumerable.Empty<(Fraction, string, Fraction)>();

        var lines = await File.ReadAllLinesAsync(filePath);
        return lines
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line =>
            {
                var parts = line.Split(',', StringSplitOptions.TrimEntries);
                if (parts.Length >= 5)
                {
                    if (long.TryParse(parts[0], out long n1) &&
                        long.TryParse(parts[1], out long d1) &&
                        long.TryParse(parts[3], out long n2) &&
                        long.TryParse(parts[4], out long d2))
                    {
                        return (new Fraction(n1, d1), parts[2], new Fraction(n2, d2));
                    }
                }
                return default;
            })
            .Where(x => x != default);
    }
}
