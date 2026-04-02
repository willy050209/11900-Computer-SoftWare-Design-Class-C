Namespace Models
    Public Class RoundResult
        Public Property RoundIndex As Integer
        Public Property PlayerCard As Card
        Public Property BankerCard As Card
        Public Property Result As String

        Public Sub New(idx As Integer, p As Card, b As Card, res As String)
            Me.RoundIndex = idx
            Me.PlayerCard = p
            Me.BankerCard = b
            Me.Result = res
        End Sub
    End Class
End Namespace

Namespace Services
    Public Interface IPokerService
        Function PlayRound(roundIndex As Integer, playerRandom As Double, bankerRandom As Double) As Models.RoundResult
    End Interface

    Public Class PokerService
        Implements IPokerService

        Public Function PlayRound(roundIndex As Integer, playerRandom As Double, bankerRandom As Double) As Models.RoundResult Implements IPokerService.PlayRound
            Dim playerCardNum As Integer = Int(playerRandom * 52)
            Dim bankerCardNum As Integer = Int(bankerRandom * 52)

            Dim pCard As New Models.Card(playerCardNum)
            Dim bCard As New Models.Card(bankerCardNum)

            Dim resStr As String = "平手"
            If pCard.PowerValue > bCard.PowerValue Then
                resStr = "玩家贏"
            ElseIf pCard.PowerValue < bCard.PowerValue Then
                resStr = "莊家贏"
            End If

            Return New Models.RoundResult(roundIndex, pCard, bCard, resStr)
        End Function
    End Class
End Namespace
