Namespace Presenters
    Public Class MainPresenter
        Private ReadOnly _view As Views.IMainView
        Private ReadOnly _pokerService As Services.IPokerService
        Private ReadOnly _dataService As Services.IDataService

        Public Sub New(view As Views.IMainView, poker As Services.IPokerService, data As Services.IDataService)
            _view = view
            _pokerService = poker
            _dataService = data
            
            _view.CandidateName = "陳宇威"
            _view.CandidateNumber = "112590005"
            _view.DeskNumber = "005"
            _view.TestDate = $"{DateTime.Now:yyyy/MM/dd}"
            
            AddHandler _view.LoadDataRequested, AddressOf OnLoadDataRequested
        End Sub

        Private Sub OnLoadDataRequested(sender As Object, filePath As String)
            Dim numbers = _dataService.LoadRandomNumbers(filePath).ToList()
            If numbers.Count = 0 Then Exit Sub

            ' The first number is the number of rounds
            Dim totalRounds As Integer = CInt(numbers(0))
            Dim results As New List(Of Models.RoundResult)()
            Dim usedCards As New HashSet(Of Integer)()
            Dim numIndex As Integer = 1 ' Start from index 1

            For round As Integer = 1 To totalRounds
                Dim playerCardNum As Integer = -1
                Dim bankerCardNum As Integer = -1

                ' Pick Player's Card
                While numIndex < numbers.Count
                    Dim cardNum = Int(numbers(numIndex) * 52)
                    If cardNum > 51 Then cardNum = 51
                    numIndex += 1
                    If Not usedCards.Contains(cardNum) Then
                        playerCardNum = cardNum
                        usedCards.Add(cardNum)
                        Exit While
                    End If
                End While

                ' Pick Banker's Card
                While numIndex < numbers.Count
                    Dim cardNum = Int(numbers(numIndex) * 52)
                    If cardNum > 51 Then cardNum = 51
                    numIndex += 1
                    If Not usedCards.Contains(cardNum) Then
                        bankerCardNum = cardNum
                        usedCards.Add(cardNum)
                        Exit While
                    End If
                End While

                If playerCardNum <> -1 AndAlso bankerCardNum <> -1 Then
                    Dim pCard As New Models.Card(playerCardNum)
                    Dim bCard As New Models.Card(bankerCardNum)
                    
                    Dim resStr As String = "平手"
                    If pCard.PowerValue > bCard.PowerValue Then
                        resStr = "玩家贏"
                    ElseIf pCard.PowerValue < bCard.PowerValue Then
                        resStr = "莊家贏"
                    End If
                    
                    results.Add(New Models.RoundResult(round, pCard, bCard, resStr))
                End If
            Next
            
            _view.SetResults(results)
        End Sub
    End Class
End Namespace
