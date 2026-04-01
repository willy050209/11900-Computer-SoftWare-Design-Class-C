namespace PokerGame.Views;

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
        this.colIndex = new DataGridViewTextBoxColumn();
        this.colPlayer = new DataGridViewTextBoxColumn();
        this.colBanker = new DataGridViewTextBoxColumn();
        this.colResult = new DataGridViewTextBoxColumn();

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
        this.grpCandidate.Size = new Size(743, 100);
        this.grpCandidate.Text = "應檢人資料";

        // 姓名
        this.lblCandidateName.Text = "姓名";
        this.lblCandidateName.Location = new Point(10, 25);
        this.lblCandidateName.Size = new Size(40, 23);
        this.lblCandidateName.TextAlign = ContentAlignment.MiddleRight;
        this.txtName.Location = new Point(55, 22);
        this.txtName.Size = new Size(100, 23);

        // 術科測試編號
        this.lblCandidateNumber.Text = "術科測試編號";
        this.lblCandidateNumber.Location = new Point(180, 25);
        this.lblCandidateNumber.Size = new Size(90, 23);
        this.lblCandidateNumber.TextAlign = ContentAlignment.MiddleRight;
        this.txtNumber.Location = new Point(275, 22);
        this.txtNumber.Size = new Size(150, 23);

        // 座號
        this.lblDeskNumber.Text = "座號";
        this.lblDeskNumber.Location = new Point(10, 60);
        this.lblDeskNumber.Size = new Size(40, 23);
        this.lblDeskNumber.TextAlign = ContentAlignment.MiddleRight;
        this.txtDesk.Location = new Point(55, 57);
        this.txtDesk.Size = new Size(100, 23);

        // 考試日期
        this.lblTestDate.Text = "考試日期";
        this.lblTestDate.Location = new Point(180, 60);
        this.lblTestDate.Size = new Size(90, 23);
        this.lblTestDate.TextAlign = ContentAlignment.MiddleRight;
        this.txtDate.Location = new Point(275, 57);
        this.txtDate.Size = new Size(150, 23);

        // dgvResults
        this.dgvResults.AllowUserToAddRows = false;
        this.dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        this.dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvResults.Columns.AddRange(new DataGridViewColumn[] { this.colIndex, this.colPlayer, this.colBanker, this.colResult });
        this.dgvResults.Location = new Point(12, 120);
        this.dgvResults.Size = new Size(743, 330);
        this.dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        this.colIndex.HeaderText = "序號";
        this.colIndex.Width = 100;
        this.colPlayer.HeaderText = "玩家";
        this.colPlayer.Width = 150;
        this.colBanker.HeaderText = "莊家";
        this.colBanker.Width = 150;
        this.colResult.HeaderText = "結果";
        this.colResult.Width = 300;

        // MainForm
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(767, 461);
        this.Controls.Add(this.grpCandidate);
        this.Controls.Add(this.dgvResults);
        this.Text = "撲克牌比大小";
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
    private DataGridViewTextBoxColumn colIndex;
    private DataGridViewTextBoxColumn colPlayer;
    private DataGridViewTextBoxColumn colBanker;
    private DataGridViewTextBoxColumn colResult;
}
