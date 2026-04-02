// filepath: ValidationService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WinFormUITester;

public static class ValidationService
{
    private static readonly Dictionary<char, int> LetterMap = new()
    {
        {'A', 10}, {'B', 11}, {'C', 12}, {'D', 13}, {'E', 14}, {'F', 15}, {'G', 16}, {'H', 17},
        {'J', 18}, {'K', 19}, {'L', 20}, {'M', 21}, {'N', 22}, {'P', 23}, {'Q', 24}, {'R', 25},
        {'S', 26}, {'T', 27}, {'U', 28}, {'V', 29}, {'X', 30}, {'Y', 31}, {'W', 32}, {'Z', 33},
        {'I', 34}, {'O', 35}
    };

    /// <summary>
    /// 驗證身分證檢查邏輯 (Task 06)
    /// </summary>
    public static string GetIdCardError(string id, string sex)
    {
        if (id.Length != 10 || !char.IsAsciiLetterUpper(id[0]) || !id[1..].All(char.IsDigit))
            return "FORMAT ERROR";

        char sexDigit = id[1];
        if (!((sex == "M" && sexDigit == '1') || (sex == "F" && sexDigit == '2')))
            return "SEX CODE ERROR";

        if (!LetterMap.TryGetValue(id[0], out int code)) return "FORMAT ERROR";
        int sum = (code / 10) + 9 * (code % 10);
        int[] weights = { 8, 7, 6, 5, 4, 3, 2, 1 };
        for (int i = 0; i < 8; i++) sum += (id[i + 1] - '0') * weights[i];
        sum += (id[9] - '0');

        return (sum % 10 == 0) ? "" : "CHECK SUM ERROR";
    }

    /// <summary>
    /// 驗證撲克牌勝負邏輯 (Task 07)
    /// </summary>
    public static string GetPokerResult(string pCard, string bCard)
    {
        int pValue = GetCardPower(pCard);
        int bValue = GetCardPower(bCard);

        if (pValue > bValue) return "玩家贏";
        if (pValue < bValue) return "莊家贏";
        return "平手";
    }

    private static int GetCardPower(string card)
    {
        // card format: "♠A", "♥10", etc.
        string rankStr = card[1..];
        return rankStr switch
        {
            "A" => 14,
            "J" => 11,
            "Q" => 12,
            "K" => 13,
            _ => int.Parse(rankStr)
        };
    }

    /// <summary>
    /// 驗證分數運算邏輯 (Task 08)
    /// </summary>
    public static string GetFractionAnswer(string v1, string op, string v2)
    {
        var f1 = ParseFraction(v1);
        var f2 = ParseFraction(v2);

        long b = f1.num, a = f1.den;
        long y = f2.num, x = f2.den;

        (long n, long d) res = op switch
        {
            "+" => (b * x + a * y, a * x),
            "-" => (b * x - a * y, a * x),
            "*" => (b * y, a * x),
            "/" => (b * x, a * y),
            _ => throw new ArgumentException("Invalid op")
        };

        return FormatFraction(Simplify(res.n, res.d));
    }

    private static (long num, long den) ParseFraction(string s)
    {
        if (s.Contains('/'))
        {
            var parts = s.Split('/');
            return (long.Parse(parts[0]), long.Parse(parts[1]));
        }
        return (long.Parse(s), 1);
    }

    private static (long num, long den) Simplify(long num, long den)
    {
        long common = Gcd(Math.Abs(num), Math.Abs(den));
        num /= common; den /= common;
        if (den < 0) { num = -num; den = -den; }
        return (num, den);
    }

    private static long Gcd(long a, long b)
    {
        while (b != 0) { a %= b; (a, b) = (b, a); }
        return a;
    }

    private static string FormatFraction((long num, long den) f)
    {
        if (f.den == 1) return f.num.ToString();
        return $"{f.num}/{f.den}";
    }
}
