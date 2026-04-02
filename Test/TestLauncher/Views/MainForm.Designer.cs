// filepath: Views/MainForm.Designer.cs
#nullable enable
namespace TestLauncher.Views;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

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
        menuStrip1 = new MenuStrip();
        menuLanguage = new ToolStripMenuItem();
        menuLangChinese = new ToolStripMenuItem();
        menuLangEnglish = new ToolStripMenuItem();

        tabControl1 = new TabControl();
        tabTask1 = new TabPage();
        tabTask2 = new TabPage();
        rtbLog = new RichTextBox();
        btnRunT1 = new Button();
        btnRunT06 = new Button();
        btnRunT07 = new Button();
        btnRunT08 = new Button();
        btnRunAllTask2 = new Button();
        
        // Task 1 Controls
        txtT1Code = new TextBox();
        txtT1UserPdf = new TextBox();
        txtT1AnsPdf = new TextBox();
        cmbLoopType = new ComboBox();
        
        // Task 2 Controls
        txtT06Exe = new TextBox();
        txtT06Data = new TextBox();
        txtT07Exe = new TextBox();
        txtT07Data = new TextBox();
        txtT08Exe = new TextBox();
        txtT08Data = new TextBox();

        // Common Candidate Controls
        grpCommonCandidate = new GroupBox();
        txtName = new TextBox();
        txtTestNo = new TextBox();
        txtSeatNo = new TextBox();

        menuStrip1.SuspendLayout();
        tabControl1.SuspendLayout();
        tabTask1.SuspendLayout();
        tabTask2.SuspendLayout();
        grpCommonCandidate.SuspendLayout();
        SuspendLayout();

        // menuStrip1
        menuStrip1.Items.AddRange(new ToolStripItem[] { menuLanguage });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(800, 24);
        menuStrip1.TabIndex = 0;
        menuStrip1.Text = "menuStrip1";

        // menuLanguage
        menuLanguage.DropDownItems.AddRange(new ToolStripItem[] { menuLangChinese, menuLangEnglish });
        menuLanguage.Name = "menuLanguage";
        menuLanguage.Size = new Size(77, 20);
        menuLanguage.Text = "Language";

        // menuLangChinese
        menuLangChinese.Name = "menuLangChinese";
        menuLangChinese.Size = new Size(180, 22);
        menuLangChinese.Text = "繁體中文";
        menuLangChinese.Click += (s, e) => OnLanguageMenuClick(Services.Language.TraditionalChinese);

        // menuLangEnglish
        menuLangEnglish.Name = "menuLangEnglish";
        menuLangEnglish.Size = new Size(180, 22);
        menuLangEnglish.Text = "English";
        menuLangEnglish.Click += (s, e) => OnLanguageMenuClick(Services.Language.English);

        // GroupBox: Common Candidate Info (Top, below menu)
        grpCommonCandidate.Text = "Candidate Information";
        grpCommonCandidate.Location = new Point(10, 30);
        grpCommonCandidate.Size = new Size(780, 60);
        grpCommonCandidate.Name = "grpCommonCandidate";
        
        Label lblN = new Label { Name = "lblName", Text = "Name:", Location = new Point(15, 32), Width = 60 };
        txtName.Location = new Point(80, 30); txtName.Width = 120;
        
        Label lblT = new Label { Name = "lblTestNo", Text = "Test No:", Location = new Point(220, 32), Width = 80 };
        txtTestNo.Location = new Point(300, 30); txtTestNo.Width = 150;
        
        Label lblS = new Label { Name = "lblSeatNo", Text = "Seat No:", Location = new Point(470, 32), Width = 80 };
        txtSeatNo.Location = new Point(550, 30); txtSeatNo.Width = 100;

        grpCommonCandidate.Controls.Add(lblN); grpCommonCandidate.Controls.Add(txtName);
        grpCommonCandidate.Controls.Add(lblT); grpCommonCandidate.Controls.Add(txtTestNo);
        grpCommonCandidate.Controls.Add(lblS); grpCommonCandidate.Controls.Add(txtSeatNo);

        // tabControl1 (Below GroupBox)
        tabControl1.Controls.Add(tabTask1);
        tabControl1.Controls.Add(tabTask2);
        tabControl1.Location = new Point(0, 100);
        tabControl1.Size = new Size(800, 300);

        // tabTask1 Layout
        tabTask1.Padding = new Padding(10);
        AddLabelAndControl(tabTask1, "CodePath", txtT1Code, 20, true, "Code Path:");
        AddLabelAndControl(tabTask1, "UserPdf", txtT1UserPdf, 50, true, "User PDF:");
        AddLabelAndControl(tabTask1, "AnsPdf", txtT1AnsPdf, 80, true, "Ans PDF:");
        
        Label lblLoop = new Label { Name = "lblLoopType", Text = "Loop Type:", Location = new Point(10, 115), Width = 100 };
        cmbLoopType.Location = new Point(120, 115);
        cmbLoopType.Width = 100;
        cmbLoopType.Items.AddRange(new object[] { "for", "while", "do" });
        cmbLoopType.SelectedIndex = 0;
        tabTask1.Controls.Add(lblLoop);
        tabTask1.Controls.Add(cmbLoopType);

        btnRunT1.Name = "btnRunT1";
        btnRunT1.Location = new Point(120, 155);
        btnRunT1.Size = new Size(200, 40);
        btnRunT1.Click += btnRunT1_Click;
        tabTask1.Controls.Add(btnRunT1);

        // tabTask2 Layout (Optimized spacing)
        tabTask2.Padding = new Padding(10);
        Label lbl06Title = new Label { Name = "lblT06Title", Text = "Task 06:", Font = new Font(Font, FontStyle.Bold), Location = new Point(10, 10), Width = 100 };
        tabTask2.Controls.Add(lbl06Title);
        AddLabelAndControl(tabTask2, "Exe06", txtT06Exe, 35, true, "Exe:");
        AddLabelAndControl(tabTask2, "Data06", txtT06Data, 65, true, "Data:");
        btnRunT06.Name = "btnRunT06"; btnRunT06.Location = new Point(560, 48); btnRunT06.Size = new Size(100, 30); btnRunT06.Click += btnRunT06_Click;
        tabTask2.Controls.Add(btnRunT06);

        Label lbl07Title = new Label { Name = "lblT07Title", Text = "Task 07:", Font = new Font(Font, FontStyle.Bold), Location = new Point(10, 100), Width = 100 };
        tabTask2.Controls.Add(lbl07Title);
        AddLabelAndControl(tabTask2, "Exe07", txtT07Exe, 125, true, "Exe:");
        AddLabelAndControl(tabTask2, "Data07", txtT07Data, 155, true, "Data:");
        btnRunT07.Name = "btnRunT07"; btnRunT07.Location = new Point(560, 138); btnRunT07.Size = new Size(100, 30); btnRunT07.Click += btnRunT07_Click;
        tabTask2.Controls.Add(btnRunT07);

        Label lbl08Title = new Label { Name = "lblT08Title", Text = "Task 08:", Font = new Font(Font, FontStyle.Bold), Location = new Point(10, 190), Width = 100 };
        tabTask2.Controls.Add(lbl08Title);
        AddLabelAndControl(tabTask2, "Exe08", txtT08Exe, 215, true, "Exe:");
        AddLabelAndControl(tabTask2, "Data08", txtT08Data, 245, true, "Data:");
        btnRunT08.Name = "btnRunT08"; btnRunT08.Location = new Point(560, 228); btnRunT08.Size = new Size(100, 30); btnRunT08.Click += btnRunT08_Click;
        tabTask2.Controls.Add(btnRunT08);

        // btnRunAllTask2 (Placed to the right of individual run buttons)
        btnRunAllTask2.Name = "btnRunAllTask2";
        btnRunAllTask2.Location = new Point(675, 48);
        btnRunAllTask2.Size = new Size(100, 210);
        btnRunAllTask2.Click += btnRunAllTask2_Click;
        tabTask2.Controls.Add(btnRunAllTask2);

        // Log Area (Fill bottom)
        rtbLog.Dock = DockStyle.Bottom;
        rtbLog.Height = 280;
        rtbLog.BackColor = Color.Black;
        rtbLog.ForeColor = Color.White;
        rtbLog.Font = new Font("Consolas", 10F);
        rtbLog.ReadOnly = true;

        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 680);
        Controls.Add(tabControl1);
        Controls.Add(grpCommonCandidate);
        Controls.Add(menuStrip1);
        Controls.Add(rtbLog);
        MainMenuStrip = menuStrip1;
        Name = "MainForm";
        Text = "Automated Test Launcher - Level C Software Design";
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        tabControl1.ResumeLayout(false);
        tabTask1.ResumeLayout(false);
        tabTask1.PerformLayout();
        tabTask2.ResumeLayout(false);
        tabTask2.PerformLayout();
        grpCommonCandidate.ResumeLayout(false);
        grpCommonCandidate.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private void AddLabelAndControl(TabPage page, string baseName, TextBox textBox, int y, bool showBrowse, string? labelText = null)
    {
        Label label = new Label { Name = "lbl" + baseName, Text = labelText ?? baseName + ":", Location = new Point(10, y), Width = 100 };
        textBox.Name = "txt" + baseName;
        textBox.Location = new Point(120, y);
        textBox.Width = 400;
        page.Controls.Add(label);
        page.Controls.Add(textBox);

        if (showBrowse)
        {
            Button btn = new Button { Name = "btnBrowse" + baseName, Text = "...", Location = new Point(525, y - 2), Width = 30, Tag = textBox };
            btn.Click += btnBrowse_Click;
            page.Controls.Add(btn);
        }
    }

    private MenuStrip menuStrip1 = default!;
    private ToolStripMenuItem menuLanguage = default!;
    private ToolStripMenuItem menuLangChinese = default!;
    private ToolStripMenuItem menuLangEnglish = default!;

    private TabControl tabControl1 = default!;
    private TabPage tabTask1 = default!;
    private TabPage tabTask2 = default!;
    private RichTextBox rtbLog = default!;
    private Button btnRunT1 = default!;
    private Button btnRunT06 = default!;
    private Button btnRunT07 = default!;
    private Button btnRunT08 = default!;
    private Button btnRunAllTask2 = default!;

    private TextBox txtT1Code = default!;
    private TextBox txtT1UserPdf = default!;
    private TextBox txtT1AnsPdf = default!;
    private ComboBox cmbLoopType = default!;

    private TextBox txtT06Exe = default!;
    private TextBox txtT06Data = default!;
    private TextBox txtT07Exe = default!;
    private TextBox txtT07Data = default!;
    private TextBox txtT08Exe = default!;
    private TextBox txtT08Data = default!;

    private GroupBox grpCommonCandidate = default!;
    private TextBox txtName = default!;
    private TextBox txtTestNo = default!;
    private TextBox txtSeatNo = default!;
}
