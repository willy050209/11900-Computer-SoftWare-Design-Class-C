Imports System.Windows.Forms
Imports System.Drawing

Namespace Views
    Public Interface IMainView
        Property CandidateName As String
        Property CandidateNumber As String
        Property DeskNumber As String
        Property TestDate As String
        
        Event LoadDataRequested As EventHandler(Of String)
        Sub SetResults(results As IEnumerable(Of Models.IdCardRecord))
    End Interface

    Public Class MainForm
        Inherits Form
        Implements IMainView

        Public Event LoadDataRequested As EventHandler(Of String) Implements IMainView.LoadDataRequested

        Private grpCandidate As GroupBox
        Private lblCandidateName, lblCandidateNumber, lblDeskNumber, lblTestDate As Label
        Private txtName, txtNumber, txtDesk, txtDate As TextBox
        Private dgvResults As DataGridView
        Private colId, colName, colSex, colError As DataGridViewTextBoxColumn

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
            Using ofd As New OpenFileDialog()
                ofd.Filter = "SM Files (*.SM)|*.SM|All Files (*.*)|*.*"
                If ofd.ShowDialog(Me) = DialogResult.OK Then
                    RaiseEvent LoadDataRequested(Me, ofd.FileName)
                End If
            End Using
        End Sub

        Private Sub InitializeComponent()
            Me.grpCandidate = New GroupBox()
            
            Me.lblCandidateName = New Label() With {.Text = "姓名"}
            Me.lblCandidateName.SetBounds(10, 30, 60, 25)
            
            Me.txtName = New TextBox() With {.Name = "txtName"}
            Me.txtName.SetBounds(80, 30, 140, 30)
            
            Me.lblCandidateNumber = New Label() With {.Text = "術科測試編號"}
            Me.lblCandidateNumber.SetBounds(250, 30, 130, 25)
            
            Me.txtNumber = New TextBox() With {.Name = "txtNumber"}
            Me.txtNumber.SetBounds(390, 30, 210, 30)
            
            Me.lblDeskNumber = New Label() With {.Text = "座號"}
            Me.lblDeskNumber.SetBounds(10, 80, 60, 25)
            
            Me.txtDesk = New TextBox() With {.Name = "txtDesk"}
            Me.txtDesk.SetBounds(80, 80, 140, 30)
            
            Me.lblTestDate = New Label() With {.Text = "考 試 日 期"}
            Me.lblTestDate.SetBounds(250, 80, 130, 25)
            
            Me.txtDate = New TextBox() With {.Name = "txtDate"}
            Me.txtDate.SetBounds(390, 80, 210, 30)
            
            Me.dgvResults = New DataGridView()
            
            Me.Text = "身分證號碼檢查"
            Me.Size = New Size(800, 700)

            grpCandidate.Text = "應檢人資料"
            grpCandidate.SetBounds(10, 10, 760, 150)
            grpCandidate.Controls.AddRange({lblCandidateName, txtName, lblCandidateNumber, txtNumber, lblDeskNumber, txtDesk, lblTestDate, txtDate})
            
            dgvResults.Name = "dgvResults"
            dgvResults.SetBounds(10, 170, 760, 480)
            dgvResults.AllowUserToAddRows = False
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            
            colId = New DataGridViewTextBoxColumn() With {.HeaderText = "ID_NO", .Name = "colId"}
            colName = New DataGridViewTextBoxColumn() With {.HeaderText = "NAME", .Name = "colName"}
            colSex = New DataGridViewTextBoxColumn() With {.HeaderText = "SEX", .Name = "colSex"}
            colError = New DataGridViewTextBoxColumn() With {.HeaderText = "ERROR", .Name = "colError"}
            
            dgvResults.Columns.AddRange({colId, colName, colSex, colError})
            
            Me.Controls.AddRange({grpCandidate, dgvResults})
        End Sub

        Public Sub SetResults(results As IEnumerable(Of Models.IdCardRecord)) Implements IMainView.SetResults
            dgvResults.Rows.Clear()
            For Each r In results
                dgvResults.Rows.Add(r.Id, r.Name, r.Sex, r.ErrorMessage)
            Next
        End Sub

    End Class
End Namespace
