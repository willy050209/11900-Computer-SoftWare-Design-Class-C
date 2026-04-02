Imports System.Windows.Forms
Imports System.Drawing

Namespace Views
    Public Interface IMainView
        Property CandidateName As String
        Property CandidateNumber As String
        Property DeskNumber As String
        Property TestDate As String
        
        Event LoadDataRequested As EventHandler(Of String)
        Sub SetResults(results As IEnumerable(Of Models.RoundResult))
    End Interface

    Public Class MainForm
        Inherits Form
        Implements IMainView

        Public Event LoadDataRequested As EventHandler(Of String) Implements IMainView.LoadDataRequested

        Private grpCandidate As GroupBox
        Private lblCandidateName, lblCandidateNumber, lblDeskNumber, lblTestDate As Label
        Private txtName, txtNumber, txtDesk, txtDate As TextBox
        Private dgvResults As DataGridView
        Private colRound, colPlayer, colBanker, colResult As DataGridViewTextBoxColumn

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
        Public Property CandidateName As String Implements IMainView.CandidateName
            Get
                Return txtName.Text
            End Get
            Set(value As String)
                txtName.Text = value
            End Set
        End Property

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
        Public Property CandidateNumber As String Implements IMainView.CandidateNumber
            Get
                Return txtNumber.Text
            End Get
            Set(value As String)
                txtNumber.Text = value
            End Set
        End Property

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
        Public Property DeskNumber As String Implements IMainView.DeskNumber
            Get
                Return txtDesk.Text
            End Get
            Set(value As String)
                txtDesk.Text = value
            End Set
        End Property

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
        Public Property TestDate As String Implements IMainView.TestDate
            Get
                Return txtDate.Text
            End Get
            Set(value As String)
                txtDate.Text = value
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
            AddHandler Me.Load, AddressOf MainForm_Load
        End Sub

        Private Sub MainForm_Load(sender As Object, e As EventArgs)
            ' 直接彈出對話框，不使用 BeginInvoke 以符合測試程式的同步步調
            Using ofd As New OpenFileDialog()
                ofd.Filter = "SM Files (*.SM)|*.SM|All Files (*.*)|*.*"
                If ofd.ShowDialog(Me) = DialogResult.OK Then
                    RaiseEvent LoadDataRequested(Me, ofd.FileName)
                End If
            End Using
        End Sub

        Private Sub InitializeComponent()
            Me.Text = "撲克牌比大小"
            Me.Size = New Size(800, 600)

            Me.grpCandidate = New GroupBox() With {.Text = "應檢人資料"}
            Me.grpCandidate.SetBounds(10, 10, 760, 120)

            Me.lblCandidateName = New Label() With {.Text = "姓名"}
            Me.lblCandidateName.SetBounds(10, 25, 60, 25)
            Me.txtName = New TextBox() With {.Name = "txtName"}
            Me.txtName.SetBounds(80, 25, 140, 30)
            
            Me.lblCandidateNumber = New Label() With {.Text = "術科測試編號"}
            Me.lblCandidateNumber.SetBounds(250, 25, 130, 25)
            Me.txtNumber = New TextBox() With {.Name = "txtNumber"}
            Me.txtNumber.SetBounds(390, 25, 210, 30)
            
            Me.lblDeskNumber = New Label() With {.Text = "座號"}
            Me.lblDeskNumber.SetBounds(10, 70, 60, 25)
            Me.txtDesk = New TextBox() With {.Name = "txtDesk"}
            Me.txtDesk.SetBounds(80, 70, 140, 30)
            
            Me.lblTestDate = New Label() With {.Text = "考 試 日 期"}
            Me.lblTestDate.SetBounds(250, 70, 130, 25)
            Me.txtDate = New TextBox() With {.Name = "txtDate"}
            Me.txtDate.SetBounds(390, 70, 210, 30)
            
            Me.dgvResults = New DataGridView() With {.Name = "dgvResults"}
            Me.dgvResults.SetBounds(10, 140, 760, 400)
            Me.dgvResults.AllowUserToAddRows = False
            Me.dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            
            colRound = New DataGridViewTextBoxColumn() With {.HeaderText = "序號", .Name = "colRound"}
            colPlayer = New DataGridViewTextBoxColumn() With {.HeaderText = "玩家", .Name = "colPlayer"}
            colBanker = New DataGridViewTextBoxColumn() With {.HeaderText = "莊家", .Name = "colBanker"}
            colResult = New DataGridViewTextBoxColumn() With {.HeaderText = "結果", .Name = "colResult"}
            
            dgvResults.Columns.AddRange({colRound, colPlayer, colBanker, colResult})
            grpCandidate.Controls.AddRange({lblCandidateName, txtName, lblCandidateNumber, txtNumber, lblDeskNumber, txtDesk, lblTestDate, txtDate})
            
            Me.Controls.AddRange({grpCandidate, dgvResults})
        End Sub

        Public Sub SetResults(results As IEnumerable(Of Models.RoundResult)) Implements IMainView.SetResults
            dgvResults.Rows.Clear()
            For Each r In results
                dgvResults.Rows.Add(r.RoundIndex, r.PlayerCard.ToString(), r.BankerCard.ToString(), r.Result)
            Next
        End Sub
    End Class
End Namespace
