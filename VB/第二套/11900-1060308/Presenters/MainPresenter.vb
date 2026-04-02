Namespace Presenters
    Public Class MainPresenter
        Private ReadOnly _view As Views.IMainView
        Private ReadOnly _fractionService As Services.IFractionService
        Private ReadOnly _dataService As Services.IDataService

        Public Sub New(view As Views.IMainView, fraction As Services.IFractionService, data As Services.IDataService)
            _view = view
            _fractionService = fraction
            _dataService = data
            
            _view.CandidateName = "陳宇威"
            _view.CandidateNumber = "112590005"
            _view.DeskNumber = "005"
            _view.TestDate = $"{DateTime.Now:yyyy/MM/dd}"
            
            AddHandler _view.LoadDataRequested, AddressOf OnLoadDataRequested
        End Sub

        Private Sub OnLoadDataRequested(sender As Object, filePath As String)
            Dim inputs = _dataService.LoadCalculations(filePath)
            Dim results As New List(Of Views.CalculationResult)()
            
            For Each i In inputs
                Dim res = _fractionService.Calculate(i.F1, i.Op, i.F2)
                results.Add(New Views.CalculationResult(i.F1.ToString(), i.Op, i.F2.ToString(), res.ToString()))
            Next
            _view.SetResults(results)
        End Sub
    End Class
End Namespace
