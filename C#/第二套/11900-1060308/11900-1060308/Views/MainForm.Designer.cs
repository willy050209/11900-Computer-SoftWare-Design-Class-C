namespace FractionArithmetic.Views;

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
        this.grpCandidate = new GroupBox();
        this.lblCandidateName = new Label();
        this.txtName = new TextBox();
        this.lblCandidateNumber = new Label();
        this.txtNumber = new TextBox();
        this.lblDeskNumber = new Label();
        this.txtDesk = new TextBox();
        this.lblTestDate = new Label();
        this.txtDate = new TextBox();
        this.dgvResults = new DataGridView();
        this.colValue1 = new DataGridViewTextBoxColumn();
        this.colOp = new DataGridViewTextBoxColumn();
        this.colValue2 = new DataGridViewTextBoxColumn();
        this.colAnswer = new DataGridViewTextBoxColumn();

        this.grpCandidate.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
        this.SuspendLayout();

        // grpCandidate
        this.grpCandidate.Controls.Add(this.lblCandidateName);
        this.grpCandidate.Controls.Add(this.txtName);
        this.grpCandidate.Controls.Add(this.lblCandidateNumber);
        this.grpCandidate.Controls.Add(this.txtNumber);
        this.grpCandidate.Controls.Add(this.lblDeskNumber);
        this.grpCandidate.Controls.Add(this.txtDesk);
        this.grpCandidate.Controls.Add(this.lblTestDate);
        this.grpCandidate.Controls.Add(this.txtDate);
        this.grpCandidate.Location = new Point(12, 12);
        this.grpCandidate.Size = new Size(760, 100);
        this.grpCandidate.Text = "應檢人資料";

        // 姓名
        this.lblCandidateName.Text = "姓名";
        this.lblCandidateName.Location = new Point(10, 25);
        this.lblCandidateName.Size = new Size(40, 23);
        this.txtName.Location = new Point(55, 22);
        this.txtName.Size = new Size(100, 23);

        // 術科測試編號
        this.lblCandidateNumber.Text = "術科測試編號";
        this.lblCandidateNumber.Location = new Point(180, 25);
        this.lblCandidateNumber.Size = new Size(90, 23);
        this.txtNumber.Location = new Point(275, 22);
        this.txtNumber.Size = new Size(150, 23);

        // 座號
        this.lblDeskNumber.Text = "座號";
        this.lblDeskNumber.Location = new Point(10, 60);
        this.lblDeskNumber.Size = new Size(40, 23);
        this.txtDesk.Location = new Point(55, 57);
        this.txtDesk.Size = new Size(100, 23);

        // 考試日期
        this.lblTestDate.Text = "考試日期";
        this.lblTestDate.Location = new Point(180, 60);
        this.lblTestDate.Size = new Size(90, 23);
        this.txtDate.Location = new Point(275, 57);
        this.txtDate.Size = new Size(150, 23);

        // dgvResults
        this.dgvResults.AllowUserToAddRows = false;
        this.dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvResults.Columns.AddRange(new DataGridViewColumn[] { this.colValue1, this.colOp, this.colValue2, this.colAnswer });
        this.dgvResults.Location = new Point(12, 120);
        this.dgvResults.Size = new Size(760, 330);
        this.dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        this.colValue1.HeaderText = "VALUE1";
        this.colValue1.Width = 150;
        this.colOp.HeaderText = "OP";
        this.colOp.Width = 80;
        this.colValue2.HeaderText = "VALUE2";
        this.colValue2.Width = 150;
        this.colAnswer.HeaderText = "ANSWER";
        this.colAnswer.Width = 200;

        // MainForm
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(784, 461);
        this.Controls.Add(this.grpCandidate);
        this.Controls.Add(this.dgvResults);
        this.Text = "求分數的加、減、乘、除運算";
        this.grpCandidate.ResumeLayout(false);
        this.grpCandidate.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
        this.ResumeLayout(false);
        
        // 確保 GroupBox 在最上層
        this.grpCandidate.BringToFront();
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
    private DataGridViewTextBoxColumn colValue1;
    private DataGridViewTextBoxColumn colOp;
    private DataGridViewTextBoxColumn colValue2;
    private DataGridViewTextBoxColumn colAnswer;
}
