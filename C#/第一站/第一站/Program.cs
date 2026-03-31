/*
 * 11900-940301 ~ 11900-940305 程式設計測驗
 * 使用 For 迴圈
 * */


var name = "姓名：陳宇威";
var no = "術科測試編號：112590005";
var id = "座號：005";
var date = $"日期：{DateTime.Now.Year - 1911}/{DateTime.Now:MM/dd}";
var output = $"{name,-10}{no}\n{id,-13}{date}\n";


// ******************************
// * 11900-940301 Program Start *
// ******************************
{
    bool IsPalindromeFor(string input)
    {
        for (int index = 0; index < input.Length / 2;)
        {
            if (input[index] != input[^++index])
            {
                return false;
            }
        }
        return true;
    }


    var path = @"./1060301.SM";
    var input = File.ReadAllText(path);
    var result = IsPalindromeFor(input) ? $"{input} is a palindrome." : $"{input} is not a palindrome.";
    output += $"第一題結果： {result}\n";
}

// ******************************
// * 11900-940302 Program Start *
// ******************************
{
    
    string PrintNumberTriangle(int i)
    {
        if (i == 0) return "";
        else return PrintNumberTriangle(i - 1) + PrintNumberSeq(i) + '\n';
        string PrintNumberSeq(int i)
        {
            if (i == 0) return "";
            else return PrintNumberSeq(i - 1) + $"{i}";
        }
    }

    for (; false;) ;
    var path = @"./1060302.SM";
    var input = File.ReadAllText(path);
    var result = PrintNumberTriangle(int.Parse(input));
    output += $"第二題結果： \n{result}\n";
}

// ******************************
// * 11900-940303 Program Start *
// ******************************
{
    
    bool IsPrime(int n,int i = 2)
    {
        if (n < 2) return false;
        if (i * i > n) return true;
        if (n % i == 0) return false;
        return IsPrime(n, i + 1);
    }

    for (; false;) ;
    var path = @"./1060303.SM";
    var input = File.ReadAllText(path);
    var result = IsPrime(int.Parse(input)) ? $"{input} is a prime number." : $"{input} is not a prime number.";
    output += $"第三題結果： {result}\n";
}

// ******************************
// * 11900-940304 Program Start *
// ******************************
{
    int CalculateBMI(int height,int weight)
    {
        return (int)((weight * 10000f / (height * height)) + 0.5f);
    }


    var path = @"./1060304.SM";
    using var ifs = new StreamReader(path);
    int minBMI = int.MaxValue;
    for (int i = 0; i < 3; ++i) 
    {
        var input = ifs.ReadLine() ?? "";
        var ss = input.Split(',', StringSplitOptions.TrimEntries);
        var height = int.Parse(ss[0]);
        var weight = int.Parse(ss[1]);
        var bmi = CalculateBMI(height, weight);
        if (minBMI > bmi) minBMI = bmi;
    }


    var result = (minBMI >= 20 && minBMI <= 25) ? $"最小BMI值={minBMI}，正常" : $"最小BMI值={minBMI}，不正常";
    output += $"第四題結果： {result}\n";
}

// ******************************
// * 11900-940305 Program Start *
// ******************************
{
    var path = @"./1060305.SM";
    using var ifs = new StreamReader(path);
    int a0, a1, a2, a3, b0, b1, b2, b3;
    for (; false;) ;
    {
        var input = ifs.ReadLine() ?? "";
        var ss = input.Split(',', StringSplitOptions.TrimEntries);
        a0 = int.Parse(ss[0]);
        a1 = int.Parse(ss[1]);
    }
    {
        var input = ifs.ReadLine() ?? "";
        var ss = input.Split(',', StringSplitOptions.TrimEntries);
        a2 = int.Parse(ss[0]);
        a3 = int.Parse(ss[1]);
    }
    {
        var input = ifs.ReadLine() ?? "";
        var ss = input.Split(',', StringSplitOptions.TrimEntries);
        b0 = int.Parse(ss[0]);
        b1 = int.Parse(ss[1]);
    }
    {
        var input = ifs.ReadLine() ?? "";
        var ss = input.Split(',', StringSplitOptions.TrimEntries);
        b2 = int.Parse(ss[0]);
        b3 = int.Parse(ss[1]);
    }

    var result = $"[{a0 + b0}\t{a1 + b1}]\n[{a2 + b2}\t{a3 + b3}]";
    output += $"第五題結果： \n{result}\n";
}

Console.WriteLine(output);

var pd = new System.Drawing.Printing.PrintDocument();
pd.PrintPage += (_,e) =>
{
    e.Graphics?.DrawString(output, new System.Drawing.Font("Consolas", 12), System.Drawing.Brushes.Black, new System.Drawing.PointF(10, 10));
};

pd.Print();
