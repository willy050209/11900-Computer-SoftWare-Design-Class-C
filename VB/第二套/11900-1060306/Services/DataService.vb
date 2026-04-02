Imports System.IO

Namespace Services
    Public Interface IDataService
        Function LoadData(path As String) As IEnumerable(Of Models.IdCardRecord)
    End Interface

    Public Class DataService
        Implements IDataService

        Public Function LoadData(path As String) As IEnumerable(Of Models.IdCardRecord) Implements IDataService.LoadData
            Dim records As New List(Of Models.IdCardRecord)()
            If File.Exists(path) Then
                For Each line In File.ReadAllLines(path)
                    Dim ss = line.Split(","c)
                    If ss.Length >= 3 Then
                        records.Add(New Models.IdCardRecord(ss(0).Trim(), ss(1).Trim(), ss(2).Trim()))
                    End If
                Next
            End If
            Return records
        End Function
    End Class
End Namespace
