Namespace Models
    Public Class Card
        Public Property CardNumber As Integer ' 0-51
        
        Public ReadOnly Property Suit As Integer
            Get
                Return CardNumber \ 13
            End Get
        End Property

        Public ReadOnly Property Rank As Integer
            Get
                Return CardNumber Mod 13
            End Get
        End Property

        Public Sub New(cardNumber As Integer)
            Me.CardNumber = cardNumber
        End Sub

        ' Rank weight: A(13) > K(12) > ... > 2(1)
        Public ReadOnly Property PowerValue As Integer
            Get
                If Rank = 0 Then Return 13
                Return Rank
            End Get
        End Property

        ' For Display: Symbol + Rank (e.g., ♠A, ♠10)
        Public Overrides Function ToString() As String
            Dim suitSymbols As String() = {"♣", "♦", "♥", "♠"}
            Dim rankNames As String() = {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"}
            Return suitSymbols(Suit) & rankNames(Rank)
        End Function
    End Class
End Namespace
