using System;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;

/*
 * 11900-940301 ~ 11900-940305 程式設計測驗
 * 使用 For 迴圈
 * */

var name = "姓名：陳宇威";
var no = "術科測試編號：112590005";
var id = "座號：005";
var date = $"日期：{DateTime.Now.Year - 1911}/{DateTime.Now:MM/dd}";
var output = $"{name,-10}{no}\n{id,-13}{date}\n\n";

// ******************************
// * 11900-940301 Program Start *
// ******************************
{
    var path = @"./1060301.SM";
    if (File.Exists(path))
    {
        var input = File.ReadAllText(path).Trim();
        bool isPalindrome = true;
        for (int i = 0; i < input.Length / 2; i++)
        {
            if (input[i] != input[input.Length - 1 - i])
            {
                isPalindrome = false;
                break;
            }
        }
        var result = isPalindrome ? $"{input} is a palindrome." : $"{input} is not a palindrome.";
        output += $"第一題結果： {result}\n";
    }
}

// ******************************
// * 11900-940302 Program Start *
// ******************************
{
    var path = @"./1060302.SM";
    if (File.Exists(path))
    {
        var n = int.Parse(File.ReadAllText(path).Trim());
        string result = "";
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= i; j++)
            {
                result += j.ToString();
            }
            result += "\n";
        }
        output += $"第二題結果： \n{result}";
    }
}

// ******************************
// * 11900-940303 Program Start *
// ******************************
{
    var path = @"./1060303.SM";
    if (File.Exists(path))
    {
        var n = int.Parse(File.ReadAllText(path).Trim());
        bool isPrime = n >= 2;
        for (int i = 2; i * i <= n; i++)
        {
            if (n % i == 0)
            {
                isPrime = false;
                break;
            }
        }
        var result = isPrime ? $"{n} is a prime number." : $"{n} is not a prime number.";
        output += $"第三題結果： {result}\n";
    }
}

// ******************************
// * 11900-940304 Program Start *
// ******************************
{
    var path = @"./1060304.SM";
    if (File.Exists(path))
    {
        var lines = File.ReadAllLines(path);
        int minBMI = int.MaxValue;
        for (int i = 0; i < 3 && i < lines.Length; i++)
        {
            var ss = lines[i].Split(',');
            double height = double.Parse(ss[0]) / 100.0;
            double weight = double.Parse(ss[1]);
            int bmi = (int)Math.Round(weight / (height * height));
            if (bmi < minBMI) minBMI = bmi;
        }
        var status = (minBMI >= 20 && minBMI <= 25) ? "正常" : "不正常";
        output += $"第四題結果： 最小BMI值={minBMI}，{status}\n";
    }
}

// ******************************
// * 11900-940305 Program Start *
// ******************************
{
    var path = @"./1060305.SM";
    if (File.Exists(path))
    {
        var lines = File.ReadAllLines(path);
        int[,] matrixA = new int[2, 2];
        int[,] matrixB = new int[2, 2];
        for (int i = 0; i < 2 && (i + 2) < lines.Length; i++)
        {
            var ssA = lines[i].Split(',');
            var ssB = lines[i + 2].Split(',');
            for (int j = 0; j < 2 && j < ssA.Length && j < ssB.Length; j++)
            {
                matrixA[i, j] = int.Parse(ssA[j]);
                matrixB[i, j] = int.Parse(ssB[j]);
            }
        }
        var result = $"[{matrixA[0, 0] + matrixB[0, 0]}\t{matrixA[0, 1] + matrixB[0, 1]}]\n" +
                     $"[{matrixA[1, 0] + matrixB[1, 0]}\t{matrixA[1, 1] + matrixB[1, 1]}]";
        output += $"第五題結果： \n{result}\n";
    }
}

Console.WriteLine(output);
try
{
    PrintDocument pd = new PrintDocument();
    pd.PrintPage += (s, e) => e.Graphics?.DrawString(output, new Font("細明體", 12), Brushes.Black, 10, 10);
    pd.Print();
}
catch { }
