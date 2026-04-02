Namespace Services
    Public Interface IFractionService
        Function Calculate(f1 As Models.Fraction, op As String, f2 As Models.Fraction) As Models.Fraction
        Function Simplify(num As Long, den As Long) As Models.Fraction
    End Interface

    Public Class FractionService
        Implements IFractionService

        Public Function Calculate(f1 As Models.Fraction, op As String, f2 As Models.Fraction) As Models.Fraction Implements IFractionService.Calculate
            Dim b As Long = f1.Numerator
            Dim a As Long = f1.Denominator
            Dim y As Long = f2.Numerator
            Dim x As Long = f2.Denominator

            Select Case op
                Case "+"
                    Return Simplify(b * x + a * y, a * x)
                Case "-"
                    Return Simplify(b * x - a * y, a * x)
                Case "*"
                    Return Simplify(b * y, a * x)
                Case "/"
                    Return Simplify(b * x, a * y)
                Case Else
                    Throw New ArgumentException("Invalid operator")
            End Select
        End Function

        Public Function Simplify(num As Long, den As Long) As Models.Fraction Implements IFractionService.Simplify
            If den = 0 Then Throw New DivideByZeroException()

            Dim common As Long = Gcd(Math.Abs(num), Math.Abs(den))
            num \= common
            den \= common

            If den < 0 Then
                num = -num
                den = -den
            End If

            Return New Models.Fraction(num, den)
        End Function

        Private Shared Function Gcd(a As Long, b As Long) As Long
            While b <> 0
                Dim temp As Long = b
                b = a Mod b
                a = temp
            End While
            Return a
        End Function

    End Class
End Namespace
