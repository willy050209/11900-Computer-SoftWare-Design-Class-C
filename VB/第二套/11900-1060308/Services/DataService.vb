Imports System.IO

Namespace Services
    Public Interface IDataService
        Function LoadCalculations(path As String) As IEnumerable(Of CalculationInput)
    End Interface

    Public Class CalculationInput
        Public Property F1 As Models.Fraction
        Public Property Op As String
        Public Property F2 As Models.Fraction
    End Class

    Public Class DataService
        Implements IDataService

        Public Function LoadCalculations(path As String) As IEnumerable(Of CalculationInput) Implements IDataService.LoadCalculations
            Dim list As New List(Of CalculationInput)()
            If File.Exists(path) Then
                For Each line In File.ReadAllLines(path)
                    Dim ss = line.Split(","c)
                    If ss.Length >= 5 Then
                        Dim input As New CalculationInput()
                        input.F1 = New Models.Fraction(Val(ss(0)), Val(ss(1)))
                        input.Op = ss(2).Trim()
                        input.F2 = New Models.Fraction(Val(ss(3)), Val(ss(4)))
                        list.Add(input)
                    End If
                Next
            End If
            Return list
        End Function
    End Class
End Namespace
