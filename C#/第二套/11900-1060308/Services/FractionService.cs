namespace FractionArithmetic.Services;

using FractionArithmetic.Models;

public interface IFractionService
{
    Fraction Calculate(Fraction f1, string op, Fraction f2);
    Fraction Simplify(long num, long den);
}

public class FractionService : IFractionService
{
    public Fraction Calculate(Fraction f1, string op, Fraction f2)
    {
        long b = f1.Numerator;
        long a = f1.Denominator;
        long y = f2.Numerator;
        long x = f2.Denominator;

        return op switch
        {
            "+" => Simplify(b * x + a * y, a * x),
            "-" => Simplify(b * x - a * y, a * x),
            "*" => Simplify(b * y, a * x),
            "/" => Simplify(b * x, a * y),
            _ => throw new ArgumentException("Invalid operator")
        };
    }

    public Fraction Simplify(long num, long den)
    {
        if (den == 0) throw new DivideByZeroException();
        
        long common = Gcd(Math.Abs(num), Math.Abs(den));
        num /= common;
        den /= common;

        if (den < 0)
        {
            num = -num;
            den = -den;
        }

        return new Fraction(num, den);
    }

    private static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            a %= b;
            (a, b) = (b, a);
        }
        return a;
    }
}
