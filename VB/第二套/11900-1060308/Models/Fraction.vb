Namespace Models
    Public Class Fraction
        Public Property Numerator As Long
        Public Property Denominator As Long

        Public Sub New(num As Long, den As Long)
            Me.Numerator = num
            Me.Denominator = den
        End Sub

        Public Overrides Function ToString() As String
            If Denominator = 1 Then
                Return Numerator.ToString()
            Else
                Return $"{Numerator}/{Denominator}"
            End If
        End Function
    End Class
End Namespace
