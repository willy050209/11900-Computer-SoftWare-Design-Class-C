Namespace Models
    Public Class IdCardRecord
        Public Property Id As String
        Public Property Name As String
        Public Property Sex As String
        Public Property ErrorMessage As String

        Public Sub New(id As String, name As String, sex As String)
            Me.Id = id
            Me.Name = name
            Me.Sex = sex
            Me.ErrorMessage = String.Empty
        End Sub
    End Class
End Namespace
