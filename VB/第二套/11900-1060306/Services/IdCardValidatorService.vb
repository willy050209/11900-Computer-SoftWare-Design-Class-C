Imports System.Collections.Generic

Namespace Services
    Public Interface IIdCardValidatorService
        Function Validate(record As Models.IdCardRecord) As Models.IdCardRecord
    End Interface

    Public Class IdCardValidatorService
        Implements IIdCardValidatorService

        Private Shared ReadOnly LetterMap As New Dictionary(Of Char, Integer) From {
            {"A"c, 10}, {"B"c, 11}, {"C"c, 12}, {"D"c, 13}, {"E"c, 14}, {"F"c, 15}, {"G"c, 16}, {"H"c, 17},
            {"J"c, 18}, {"K"c, 19}, {"L"c, 20}, {"M"c, 21}, {"N"c, 22}, {"P"c, 23}, {"Q"c, 24}, {"R"c, 25},
            {"S"c, 26}, {"T"c, 27}, {"U"c, 28}, {"V"c, 29}, {"X"c, 30}, {"Y"c, 31}, {"W"c, 32}, {"Z"c, 33},
            {"I"c, 34}, {"O"c, 35}
        }

        Public Function Validate(record As Models.IdCardRecord) As Models.IdCardRecord Implements IIdCardValidatorService.Validate
            ' (A) FORMAT ERROR
            If Not IsValidFormat(record.Id) Then
                record.ErrorMessage = "FORMAT ERROR"
                Return record
            End If

            ' (B) SEX CODE ERROR
            If Not IsValidSex(record.Id, record.Sex) Then
                record.ErrorMessage = "SEX CODE ERROR"
                Return record
            End If

            ' (C) CHECK SUM ERROR
            If Not IsValidChecksum(record.Id) Then
                record.ErrorMessage = "CHECK SUM ERROR"
                Return record
            End If

            record.ErrorMessage = String.Empty
            Return record
        End Function

        Private Shared Function IsValidFormat(id As String) As Boolean
            If id.Length <> 10 Then Return False
            If Not Char.IsLetter(id(0)) Then Return False
            
            ' VB doesn't have isAsciiLetterUpper, check A-Z
            Dim firstChar As Char = Char.ToUpper(id(0))
            If firstChar < "A"c OrElse firstChar > "Z"c Then Return False

            For i As Integer = 1 To 9
                If Not Char.IsDigit(id(i)) Then Return False
            Next
            Return True
        End Function

        Private Shared Function IsValidSex(id As String, sex As String) As Boolean
            Dim sexDigit As Char = id(1)
            If sex = "M" AndAlso sexDigit = "1"c Then Return True
            If sex = "F" AndAlso sexDigit = "2"c Then Return True
            Return False
        End Function

        Private Shared Function IsValidChecksum(id As String) As Boolean
            Dim firstChar As Char = Char.ToUpper(id(0))
            If Not LetterMap.ContainsKey(firstChar) Then Return False

            Dim code As Integer = LetterMap(firstChar)
            Dim x1 As Integer = code \ 10
            Dim x2 As Integer = code Mod 10

            Dim sum As Integer = x1 + 9 * x2
            Dim weights As Integer() = {8, 7, 6, 5, 4, 3, 2, 1}

            For i As Integer = 0 To 7
                sum += (Val(id(i + 1))) * weights(i)
            Next

            sum += Val(id(9))

            Return sum Mod 10 = 0
        End Function
    End Class
End Namespace
