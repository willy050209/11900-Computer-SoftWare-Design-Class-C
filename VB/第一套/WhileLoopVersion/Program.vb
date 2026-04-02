Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Printing

Module Program
    Sub Main()
        ' ******************************
        ' * 11900-940301 ~ 11900-940305 程式設計測驗
        ' * 使用 While 迴圈
        ' * ******************************

        Dim name As String = "姓名：陳宇威"
        Dim no As String = "術科測試編號：112590005"
        Dim id As String = "座號：005"
        Dim currentDate As String = $"日期：{DateTime.Now.Year - 1911}/{DateTime.Now:MM/dd}"
        Dim output As String = $"{name,-10}{no}" & vbCrLf & $"{id,-13}{currentDate}" & vbCrLf & vbCrLf

        ' ******************************
        ' * 11900-940301 Program Start *
        ' ******************************
        Try
            Dim path As String = "./1060301.SM"
            If File.Exists(path) Then
                Dim input As String = File.ReadAllText(path).Trim()
                Dim isPalindrome As Boolean = True
                Dim i As Integer = 0
                While i < (input.Length \ 2)
                    If input(i) <> input(input.Length - 1 - i) Then
                        isPalindrome = False
                        Exit While
                    End If
                    i += 1
                End While
                Dim result As String = If(isPalindrome, $"{input} is a palindrome.", $"{input} is not a palindrome.")
                output &= $"第一題結果： {result}" & vbCrLf
            End If
        Catch ex As Exception
        End Try

        ' ******************************
        ' * 11900-940302 Program Start *
        ' ******************************
        Try
            Dim path As String = "./1060302.SM"
            If File.Exists(path) Then
                Dim n As Integer = Integer.Parse(File.ReadAllText(path).Trim())
                Dim result As String = ""
                Dim i As Integer = 1
                While i <= n
                    Dim j As Integer = 1
                    While j <= i
                        result &= j.ToString()
                        j += 1
                    End While
                    result &= vbCrLf
                    i += 1
                End While
                output &= $"第二題結果： " & vbCrLf & result
            End If
        Catch ex As Exception
        End Try

        ' ******************************
        ' * 11900-940303 Program Start *
        ' ******************************
        Try
            Dim path As String = "./1060303.SM"
            If File.Exists(path) Then
                Dim n As Integer = Integer.Parse(File.ReadAllText(path).Trim())
                Dim isPrime As Boolean = (n >= 2)
                Dim i As Integer = 2
                While i * i <= n
                    If n Mod i = 0 Then
                        isPrime = False
                        Exit While
                    End If
                    i += 1
                End While
                Dim result As String = If(isPrime, $"{n} is a prime number.", $"{n} is not a prime number.")
                output &= $"第三題結果： {result}" & vbCrLf
            End If
        Catch ex As Exception
        End Try

        ' ******************************
        ' * 11900-940304 Program Start *
        ' ******************************
        Try
            Dim path As String = "./1060304.SM"
            If File.Exists(path) Then
                Dim lines As String() = File.ReadAllLines(path)
                Dim minBMI As Integer = Integer.MaxValue
                Dim i As Integer = 0
                While i < 3 AndAlso i < lines.Length
                    Dim ss As String() = lines(i).Split(","c)
                    Dim height As Integer = Integer.Parse(ss[0])
                    Dim weight As Integer = Integer.Parse(ss[1])
                    
                    ' 手動計算 BMI 並四捨五入：(W * 10000 / H^2) + 0.5
                    Dim bmiValue As Double = (weight * 10000.0) / (height * height)
                    Dim bmi As Integer = Int(bmiValue + 0.5)
                    
                    If bmi < minBMI Then minBMI = bmi
                    i += 1
                End While
                Dim status As String = If(minBMI >= 20 AndAlso minBMI <= 25, "正常", "不正常")
                output &= $"第四題結果： 最小BMI值={minBMI}，{status}" & vbCrLf
            End If
        Catch ex As Exception
        End Try

        ' ******************************
        ' * 11900-940305 Program Start *
        ' ******************************
        Try
            Dim path As String = "./1060305.SM"
            If File.Exists(path) Then
                Dim lines As String() = File.ReadAllLines(path)
                Dim matrixA(1, 1) As Integer
                Dim matrixB(1, 1) As Integer
                Dim i As Integer = 0
                While i < 2
                    Dim ssA As String() = lines(i).Split(","c)
                    Dim ssB As String() = lines(i + 2).Split(","c)
                    Dim j As Integer = 0
                    While j < 2
                        matrixA(i, j) = Integer.Parse(ssA(j))
                        matrixB(i, j) = Integer.Parse(ssB(j))
                        j += 1
                    End While
                    i += 1
                End While
                Dim result As String = $"[{matrixA(0, 0) + matrixB(0, 0)}	{matrixA(0, 1) + matrixB(0, 1)}]" & vbCrLf & _
                                     $"[{matrixA(1, 0) + matrixB(1, 0)}	{matrixA(1, 1) + matrixB(1, 1)}]"
                output &= $"第五題結果： " & vbCrLf & result & vbCrLf
            End If
        Catch ex As Exception
        End Try

        Console.WriteLine(output)

        Try
            Dim pd As New PrintDocument()
            AddHandler pd.PrintPage, Sub(s, ev)
                                         ev.Graphics.DrawString(output, New Font("細明體", 12), Brushes.Black, 10, 10)
                                     End Sub
            pd.Print()
        Catch
        End Try
    End Sub
End Module
