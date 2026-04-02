namespace FractionArithmetic.Models;

public record Fraction(long Numerator, long Denominator)
{
    public override string ToString()
    {
        if (Denominator == 1) return Numerator.ToString();
        return $"{Numerator}/{Denominator}";
    }
}

public record CalculationResult(Fraction Value1, string Op, Fraction Value2, Fraction Answer);
