namespace IdCardChecker.Views;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        grpCandidate = new GroupBox();
        lblCandidateName = new Label();
        txtName = new TextBox();
        lblCandidateNumber = new Label();
        txtNumber = new TextBox();
        lblDeskNumber = new Label();
        txtDesk = new TextBox();
        lblTestDate = new Label();
        txtDate = new TextBox();
        dgvResults = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colSex = new DataGridViewTextBoxColumn();
        colError = new DataGridViewTextBoxColumn();
        grpCandidate.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
        SuspendLayout();
        // 
        // grpCandidate
        // 
        grpCandidate.Controls.Add(lblCandidateName);
        grpCandidate.Controls.Add(txtName);
        grpCandidate.Controls.Add(lblCandidateNumber);
        grpCandidate.Controls.Add(txtNumber);
        grpCandidate.Controls.Add(lblDeskNumber);
        grpCandidate.Controls.Add(txtDesk);
        grpCandidate.Controls.Add(lblTestDate);
        grpCandidate.Controls.Add(txtDate);
        grpCandidate.Location = new Point(17, 20);
        grpCandidate.Margin = new Padding(4, 5, 4, 5);
        grpCandidate.Name = "grpCandidate";
        grpCandidate.Padding = new Padding(4, 5, 4, 5);
        grpCandidate.Size = new Size(742, 167);
        grpCandidate.TabIndex = 0;
        grpCandidate.TabStop = false;
        grpCandidate.Text = "應檢人資料";
        // 
        // lblCandidateName
        // 
        lblCandidateName.Location = new Point(14, 42);
        lblCandidateName.Margin = new Padding(4, 0, 4, 0);
        lblCandidateName.Name = "lblCandidateName";
        lblCandidateName.Size = new Size(57, 38);
        lblCandidateName.TabIndex = 0;
        lblCandidateName.Text = "姓名";
        lblCandidateName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtName
        // 
        txtName.Location = new Point(79, 37);
        txtName.Margin = new Padding(4, 5, 4, 5);
        txtName.Name = "txtName";
        txtName.Size = new Size(141, 31);
        txtName.TabIndex = 1;
        // 
        // lblCandidateNumber
        // 
        lblCandidateNumber.Location = new Point(257, 42);
        lblCandidateNumber.Margin = new Padding(4, 0, 4, 0);
        lblCandidateNumber.Name = "lblCandidateNumber";
        lblCandidateNumber.Size = new Size(129, 38);
        lblCandidateNumber.TabIndex = 2;
        lblCandidateNumber.Text = "術科測試編號";
        lblCandidateNumber.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNumber
        // 
        txtNumber.Location = new Point(393, 37);
        txtNumber.Margin = new Padding(4, 5, 4, 5);
        txtNumber.Name = "txtNumber";
        txtNumber.Size = new Size(213, 31);
        txtNumber.TabIndex = 3;
        // 
        // lblDeskNumber
        // 
        lblDeskNumber.Location = new Point(14, 100);
        lblDeskNumber.Margin = new Padding(4, 0, 4, 0);
        lblDeskNumber.Name = "lblDeskNumber";
        lblDeskNumber.Size = new Size(57, 38);
        lblDeskNumber.TabIndex = 4;
        lblDeskNumber.Text = "座號";
        lblDeskNumber.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtDesk
        // 
        txtDesk.Location = new Point(79, 95);
        txtDesk.Margin = new Padding(4, 5, 4, 5);
        txtDesk.Name = "txtDesk";
        txtDesk.Size = new Size(141, 31);
        txtDesk.TabIndex = 5;
        // 
        // lblTestDate
        // 
        lblTestDate.Location = new Point(257, 100);
        lblTestDate.Margin = new Padding(4, 0, 4, 0);
        lblTestDate.Name = "lblTestDate";
        lblTestDate.Size = new Size(129, 38);
        lblTestDate.TabIndex = 6;
        lblTestDate.Text = "考試日期";
        lblTestDate.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtDate
        // 
        txtDate.Location = new Point(393, 95);
        txtDate.Margin = new Padding(4, 5, 4, 5);
        txtDate.Name = "txtDate";
        txtDate.Size = new Size(213, 31);
        txtDate.TabIndex = 7;
        // 
        // dgvResults
        // 
        dgvResults.AllowUserToAddRows = false;
        dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvResults.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colSex, colError });
        dgvResults.Location = new Point(17, 200);
        dgvResults.Margin = new Padding(4, 5, 4, 5);
        dgvResults.Name = "dgvResults";
        dgvResults.RowHeadersWidth = 62;
        dgvResults.Size = new Size(742, 383);
        dgvResults.TabIndex = 1;
        // 
        // colId
        // 
        colId.HeaderText = "ID_NO";
        colId.MinimumWidth = 8;
        colId.Name = "colId";
        colId.Width = 150;
        // 
        // colName
        // 
        colName.HeaderText = "NAME";
        colName.MinimumWidth = 8;
        colName.Name = "colName";
        colName.Width = 150;
        // 
        // colSex
        // 
        colSex.HeaderText = "SEX";
        colSex.MinimumWidth = 8;
        colSex.Name = "colSex";
        colSex.Width = 80;
        // 
        // colError
        // 
        colError.HeaderText = "ERROR";
        colError.MinimumWidth = 8;
        colError.Name = "colError";
        colError.Width = 300;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(776, 601);
        Controls.Add(grpCandidate);
        Controls.Add(dgvResults);
        Margin = new Padding(4, 5, 4, 5);
        Name = "MainForm";
        Text = "身分證號碼檢查";
        grpCandidate.ResumeLayout(false);
        grpCandidate.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
        ResumeLayout(false);
    }

    private GroupBox grpCandidate;
    private Label lblCandidateName;
    private TextBox txtName;
    private Label lblCandidateNumber;
    private TextBox txtNumber;
    private Label lblDeskNumber;
    private TextBox txtDesk;
    private Label lblTestDate;
    private TextBox txtDate;
    private DataGridView dgvResults;
    private DataGridViewTextBoxColumn colId;
    private DataGridViewTextBoxColumn colName;
    private DataGridViewTextBoxColumn colSex;
    private DataGridViewTextBoxColumn colError;
}
