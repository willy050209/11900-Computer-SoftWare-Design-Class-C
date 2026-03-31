namespace IdCardChecker.Services;

using IdCardChecker.Models;
using System.Text.RegularExpressions;

public interface IIdCardValidatorService
{
    IdCardRecord Validate(IdCardRecord record);
}

public class IdCardValidatorService : IIdCardValidatorService
{
    private static readonly Dictionary<char, int> LetterMap = new()
    {
        {'A', 10}, {'B', 11}, {'C', 12}, {'D', 13}, {'E', 14}, {'F', 15}, {'G', 16}, {'H', 17},
        {'J', 18}, {'K', 19}, {'L', 20}, {'M', 21}, {'N', 22}, {'P', 23}, {'Q', 24}, {'R', 25},
        {'S', 26}, {'T', 27}, {'U', 28}, {'V', 29}, {'X', 30}, {'Y', 31}, {'W', 32}, {'Z', 33},
        {'I', 34}, {'O', 35}
    };

    /// <summary>
    /// Validates an ID card record.
    /// </summary>
    /// <param name="record">The record to validate.</param>
    /// <returns>The record with error message if any.</returns>
    public IdCardRecord Validate(IdCardRecord record)
    {
        // (A) FORMAT ERROR
        if (!IsValidFormat(record.Id))
        {
            record.Error = "FORMAT ERROR";
            return record;
        }

        // (B) SEX CODE ERROR
        if (!IsValidSex(record.Id, record.Sex))
        {
            record.Error = "SEX CODE ERROR";
            return record;
        }

        // (C) CHECK SUM ERROR
        if (!IsValidChecksum(record.Id))
        {
            record.Error = "CHECK SUM ERROR";
            return record;
        }

        record.Error = string.Empty;
        return record;
    }

    private static bool IsValidFormat(string id)
    {
        if (id.Length != 10) return false;
        if (!char.IsAsciiLetterUpper(id[0])) return false;
        for (int i = 1; i < 10; i++)
        {
            if (!char.IsDigit(id[i])) return false;
        }
        return true;
    }

    private static bool IsValidSex(string id, string sex)
    {
        char sexDigit = id[1];
        if (sex == "M" && sexDigit == '1') return true;
        if (sex == "F" && sexDigit == '2') return true;
        return false;
    }

    private static bool IsValidChecksum(string id)
    {
        if (!LetterMap.TryGetValue(id[0], out int code)) return false;

        int x1 = code / 10;
        int x2 = code % 10;

        int sum = x1 + 9 * x2;
        int[] weights = { 8, 7, 6, 5, 4, 3, 2, 1 };

        for (int i = 0; i < 8; i++)
        {
            sum += (id[i + 1] - '0') * weights[i];
        }

        sum += (id[9] - '0');

        return sum % 10 == 0;
    }
}
