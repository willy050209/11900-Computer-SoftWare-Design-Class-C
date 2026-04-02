Namespace Presenters
    Public Class MainPresenter
        Private ReadOnly _view As Views.IMainView
        Private ReadOnly _validator As Services.IIdCardValidatorService
        Private ReadOnly _dataService As Services.IDataService

        Public Sub New(view As Views.IMainView, validator As Services.IIdCardValidatorService, dataService As Services.IDataService)
            _view = view
            _validator = validator
            _dataService = dataService
            
            ' Initialize View Data
            _view.CandidateName = "陳宇威"
            _view.CandidateNumber = "112590005"
            _view.DeskNumber = "005"
            _view.TestDate = $"{DateTime.Now:yyyy/MM/dd}"

            
            AddHandler _view.LoadDataRequested, AddressOf OnLoadDataRequested
        End Sub

        Private Sub OnLoadDataRequested(sender As Object, filePath As String)
            Dim records = _dataService.LoadData(filePath)
            Dim results = New List(Of Models.IdCardRecord)()
            For Each r In records
                results.Add(_validator.Validate(r))
            Next
            
            ' Sort by ID_NO (lexicographical order)
            Dim sortedResults = results.OrderBy(Function(r) r.Id).ToList()
            _view.SetResults(sortedResults)
        End Sub

    End Class
End Namespace
