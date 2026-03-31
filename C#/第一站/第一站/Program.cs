
var name = "姓名：陳宇威";
var no = "術科測試編號：112590005";
var id = "座號：005";
var date = $"日期：{DateTime.Now.Year - 1911}/{DateTime.Now:MM/dd}";
var output = $"{name,-10}{no}\n{id,-13}{date}\n";

// ******************************
// * 11900-940301 Program Start *
// ******************************
{
    ///*
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
    //*/

    /*
    bool IsPalindromeWhile(string input)
    {
        int index = 0;
        while (index < input.Length / 2)
        {
            if (input[index] != input[^++index])
            {
                return false;
            }
        }
        return true;
    }
    //*/

    /*
    bool IsPalindromeDoWhile(string input)
    {
        int index = 0;
        do
        {
            if (input[index] != input[^++index])
            {
                return false;
            }
        } while (index < input.Length / 2);
        return true;
    }
    //*/

    var path = @"./1060301.SM";
    var input = File.ReadAllText(path);
    var result = IsPalindromeFor(input) ? $"{input} is a palindromeFor." : $"{input} is not a palindromeFor.";
    output += $"第一題結果： {result}\n";
}

// ******************************
// * 11900-940302 Program Start *
// ******************************
{
    ///*
    int StrToIntFor(string str)
    {
        int result = 0;
        for (int i = 0; i < str.Length; ++i)
        {
            result = result * 10 + (str[i] - '0');
        }
        return result;
    }
    //*/
    /*
    int StrToIntWhile(string str)
    {
        int result = 0;
        int i = 0;
        while (i < str.Length) 
        {
            result = result * 10 + (str[i] - '0');
            ++i;
        }
        return result;
    }
    //*/
    /*
    int StrToIntForDoWhile(string str)
    {
        int result = 0;
        int i = 0;
        do
        {
            result = result * 10 + (str[i] - '0');
            ++i;
        }
        while (i < str.Length);
        return result;
    }
    //*/
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


    var path = @"./1060302.SM";
    var input = File.ReadAllText(path);
    var result = PrintNumberTriangle(StrToIntFor(input));
    output += $"第二題結果： \n{result}\n";
}

// ******************************
// * 11900-940303 Program Start *
// ******************************
{
    ///*
    int StrToIntFor(string str)
    {
        int result = 0;
        for (int i = 0; i < str.Length; ++i)
        {
            result = result * 10 + (str[i] - '0');
        }
        return result;
    }
    //*/
    /*
    int StrToIntWhile(string str)
    {
        int result = 0;
        int i = 0;
        while (i < str.Length) 
        {
            result = result * 10 + (str[i] - '0');
            ++i;
        }
        return result;
    }
    //*/
    /*
    int StrToIntForDoWhile(string str)
    {
        int result = 0;
        int i = 0;
        do
        {
            result = result * 10 + (str[i] - '0');
            ++i;
        }
        while (i < str.Length);
        return result;
    }
    //*/
    
    bool IsPrime(int n,int i = 2)
    {
        if (n < 2) return false;
        if (i * i > n) return true;
        if (n % i == 0) return false;
        return IsPrime(n, i + 1);
    }


    var path = @"./1060303.SM";
    var input = File.ReadAllText(path);
    var result = IsPrime(StrToIntFor(input)) ? $"{input} is a prime number." : $"{input} not is a prime number.";
    output += $"第三題結果： {result}\n";
}

// ******************************
// * 11900-940304 Program Start *
// ******************************
{
    ///*
    int StrToIntFor(string str)
    {
        int result = 0;
        for (int i = 0; i < str.Length; ++i)
        {
            result = result * 10 + (str[i] - '0');
        }
        return result;
    }
    //*/
    /*
    int StrToIntWhile(string str)
    {
        int result = 0;
        int i = 0;
        while (i < str.Length) 
        {
            result = result * 10 + (str[i] - '0');
            ++i;
        }
        return result;
    }
    //*/
    /*
    int StrToIntForDoWhile(string str)
    {
        int result = 0;
        int i = 0;
        do
        {
            result = result * 10 + (str[i] - '0');
            ++i;
        }
        while (i < str.Length);
        return result;
    }
    //*/
    
    int FindCommaIndex(string str, int index = 0)
    {
        if (index >= str.Length) return -1;
        if (str[index] == ',') return index;
        return FindCommaIndex(str, index + 1);
    }


    int CalculateBMI(int height,int weight)
    {
        return (int)((weight * 10000f / (height * height)) + 0.5f);
    }


    var path = @"./1060304.SM";
    using var ifs = new StreamReader(path);
    int minBMI = int.MaxValue;
    // 1
    {
        var input = ifs.ReadLine() ?? "";
        var index = FindCommaIndex(input);
        var height = StrToIntFor(input[..index]);
        var weight = StrToIntFor(input[(index + 1)..^1]); // 去除'\n'
        var bmi = CalculateBMI(height, weight);
        if (minBMI > bmi) minBMI = bmi;
    }
    // 2
    {
        var input = ifs.ReadLine() ?? "";
        var index = FindCommaIndex(input);
        var height = StrToIntFor(input[..index]);
        var weight = StrToIntFor(input[(index + 1)..^1]); // 去除'\n'
        var bmi = CalculateBMI(height, weight);
        if (minBMI > bmi) minBMI = bmi;
    }
    // 3
    {
        var input = ifs.ReadLine() ?? "";
        var index = FindCommaIndex(input);
        var height = StrToIntFor(input[..index]);
        var weight = StrToIntFor(input[(index + 1)..]);
        var bmi = CalculateBMI(height, weight);
        if (minBMI > bmi) minBMI = bmi;
    }

    var result = (minBMI >= 20 && minBMI <= 25) ? $"最小BMI值={minBMI}，正常" : $"最小BMI值={minBMI}，不正常";
    output += $"第四題結果： {result}\n";
}

// ******************************
// * 11900-940304 Program Start *
// ******************************
{
    ///*
    int StrToIntFor(string str)
    {
        int result = 0;
        for (int i = 0; i < str.Length; ++i)
        {
            result = result * 10 + (str[i] - '0');
        }
        return result;
    }
    //*/
    /*
    int StrToIntWhile(string str)
    {
        int result = 0;
        int i = 0;
        while (i < str.Length) 
        {
            result = result * 10 + (str[i] - '0');
            ++i;
        }
        return result;
    }
    //*/
    /*
    int StrToIntForDoWhile(string str)
    {
        int result = 0;
        int i = 0;
        do
        {
            result = result * 10 + (str[i] - '0');
            ++i;
        }
        while (i < str.Length);
        return result;
    }
    //*/

    int FindCommaIndex(string str, int index = 0)
    {
        if (index >= str.Length) return -1;
        if (str[index] == ',') return index;
        return FindCommaIndex(str, index + 1);
    }

    var path = @"./1060305.SM";
    using var ifs = new StreamReader(path);
    int a0, a1, a2, a3, b0, b1, b2, b3;
    // 1
    {
        var input = ifs.ReadLine() ?? "";
        var index = FindCommaIndex(input);
        a0 = StrToIntFor(input[..index]);
        a1 = StrToIntFor(input[(index + 1)..]);
    }
    // 2
    {
        var input = ifs.ReadLine() ?? "";
        var index = FindCommaIndex(input);
        a2 = StrToIntFor(input[..index]);
        a3 = StrToIntFor(input[(index + 1)..]);
    }
    {
        var input = ifs.ReadLine() ?? "";
        var index = FindCommaIndex(input);
        b0 = StrToIntFor(input[..index]);
        b1 = StrToIntFor(input[(index + 1)..]);
    }
    // 3
    {
        var input = ifs.ReadLine() ?? "";
        var index = FindCommaIndex(input);
        b2 = StrToIntFor(input[..index]);
        b3 = StrToIntFor(input[(index + 1)..]);
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
