Imports System.IO

Namespace Services
    Public Interface IDataService
        Function LoadRandomNumbers(path As String) As IEnumerable(Of Double)
    End Interface

    Public Class DataService
        Implements IDataService

        Public Function LoadRandomNumbers(path As String) As IEnumerable(Of Double) Implements IDataService.LoadRandomNumbers
            Dim numbers As New List(Of Double)()
            If File.Exists(path) Then
                For Each line In File.ReadAllLines(path)
                    Dim val As Double
                    If Double.TryParse(line, val) Then
                        numbers.Add(val)
                    End If
                Next
            End If
            Return numbers
        End Function
    End Class
End Namespace
