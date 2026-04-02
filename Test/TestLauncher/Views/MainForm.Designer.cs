// filepath: Views/MainForm.Designer.cs
namespace TestLauncher.Views;

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
        tabControl1 = new TabControl();
        tabTask1 = new TabPage();
        tabTask2 = new TabPage();
        rtbLog = new RichTextBox();
        btnRunT1 = new Button();
        btnRunT06 = new Button();
        btnRunT07 = new Button();
        btnRunT08 = new Button();
        
        // Task 1 Controls
        txtT1Code = new TextBox();
        txtT1UserPdf = new TextBox();
        txtT1AnsPdf = new TextBox();
        txtName = new TextBox();
        txtTestNo = new TextBox();
        txtSeatNo = new TextBox();
        cmbLoopType = new ComboBox();
        
        // Task 2 Controls
        txtT06Exe = new TextBox();
        txtT06Data = new TextBox();
        txtT07Exe = new TextBox();
        txtT07Data = new TextBox();
        txtT08Exe = new TextBox();
        txtT08Data = new TextBox();

        // Language
        cmbLanguage = new ComboBox();
        lblLanguage = new Label();

        tabControl1.SuspendLayout();
        tabTask1.SuspendLayout();
        tabTask2.SuspendLayout();
        SuspendLayout();

        // Language Selection (Top Right)
        lblLanguage.Text = "Language:";
        lblLanguage.Location = new Point(580, 5);
        lblLanguage.AutoSize = true;
        cmbLanguage.Location = new Point(650, 2);
        cmbLanguage.Width = 120;
        cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbLanguage.Items.AddRange(new object[] { "繁體中文", "English" });
        cmbLanguage.SelectedIndex = 0;
        cmbLanguage.SelectedIndexChanged += cmbLanguage_SelectedIndexChanged;
        this.Controls.Add(lblLanguage);
        this.Controls.Add(cmbLanguage);

        // tabControl1
        tabControl1.Controls.Add(tabTask1);
        tabControl1.Controls.Add(tabTask2);
        tabControl1.Dock = DockStyle.Top;
        tabControl1.Height = 350;
        tabControl1.Location = new Point(0, 30);

        // tabTask1 Layout
        tabTask1.Padding = new Padding(10);
        AddLabelAndControl(tabTask1, "CodePath", txtT1Code, 20, true);
        AddLabelAndControl(tabTask1, "UserPdf", txtT1UserPdf, 50, true);
        AddLabelAndControl(tabTask1, "AnsPdf", txtT1AnsPdf, 80, true);
        AddLabelAndControl(tabTask1, "Name", txtName, 110, false);
        AddLabelAndControl(tabTask1, "TestNo", txtTestNo, 140, false);
        AddLabelAndControl(tabTask1, "SeatNo", txtSeatNo, 170, false);
        
        Label lblLoop = new Label { Name = "lblLoopType", Text = "LoopType:", Location = new Point(10, 200), Width = 100 };
        cmbLoopType.Location = new Point(120, 200);
        cmbLoopType.Width = 100;
        cmbLoopType.Items.AddRange(new object[] { "for", "while", "do" });
        cmbLoopType.SelectedIndex = 0;
        tabTask1.Controls.Add(lblLoop);
        tabTask1.Controls.Add(cmbLoopType);

        btnRunT1.Name = "btnRunT1";
        btnRunT1.Location = new Point(120, 240);
        btnRunT1.Size = new Size(200, 40);
        btnRunT1.Click += btnRunT1_Click;
        tabTask1.Controls.Add(btnRunT1);

        // tabTask2 Layout (Improved spacing to avoid overlap)
        tabTask2.Padding = new Padding(10);
        
        // Group 1: Task 06
        Label lbl06Title = new Label { Name = "lblT06Title", Text = "Task 06:", Font = new Font(Font, FontStyle.Bold), Location = new Point(10, 10), Width = 100 };
        tabTask2.Controls.Add(lbl06Title);
        AddLabelAndControl(tabTask2, "Exe06", txtT06Exe, 35, true, "Exe:");
        AddLabelAndControl(tabTask2, "Data06", txtT06Data, 65, true, "Data:");
        btnRunT06.Name = "btnRunT06"; btnRunT06.Location = new Point(560, 48); btnRunT06.Size = new Size(100, 30); btnRunT06.Click += btnRunT06_Click;
        tabTask2.Controls.Add(btnRunT06);

        // Group 2: Task 07
        Label lbl07Title = new Label { Name = "lblT07Title", Text = "Task 07:", Font = new Font(Font, FontStyle.Bold), Location = new Point(10, 110), Width = 100 };
        tabTask2.Controls.Add(lbl07Title);
        AddLabelAndControl(tabTask2, "Exe07", txtT07Exe, 135, true, "Exe:");
        AddLabelAndControl(tabTask2, "Data07", txtT07Data, 165, true, "Data:");
        btnRunT07.Name = "btnRunT07"; btnRunT07.Location = new Point(560, 148); btnRunT07.Size = new Size(100, 30); btnRunT07.Click += btnRunT07_Click;
        tabTask2.Controls.Add(btnRunT07);

        // Group 3: Task 08
        Label lbl08Title = new Label { Name = "lblT08Title", Text = "Task 08:", Font = new Font(Font, FontStyle.Bold), Location = new Point(10, 210), Width = 100 };
        tabTask2.Controls.Add(lbl08Title);
        AddLabelAndControl(tabTask2, "Exe08", txtT08Exe, 235, true, "Exe:");
        AddLabelAndControl(tabTask2, "Data08", txtT08Data, 265, true, "Data:");
        btnRunT08.Name = "btnRunT08"; btnRunT08.Location = new Point(560, 248); btnRunT08.Size = new Size(100, 30); btnRunT08.Click += btnRunT08_Click;
        tabTask2.Controls.Add(btnRunT08);

        // Log Area
        rtbLog.Dock = DockStyle.Fill;
        rtbLog.Location = new Point(0, 380);
        rtbLog.BackColor = Color.Black;
        rtbLog.ForeColor = Color.White;
        rtbLog.Font = new Font("Consolas", 10F);
        rtbLog.ReadOnly = true;

        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 650);
        Controls.Add(rtbLog);
        Controls.Add(tabControl1);
        Name = "MainForm";
        Text = "Automated Test Launcher - Level C Software Design";
        tabControl1.ResumeLayout(false);
        tabTask1.ResumeLayout(false);
        tabTask1.PerformLayout();
        tabTask2.ResumeLayout(false);
        tabTask2.PerformLayout();
        ResumeLayout(false);
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

    private TabControl tabControl1;
    private TabPage tabTask1;
    private TabPage tabTask2;
    private RichTextBox rtbLog;
    private Button btnRunT1;
    private Button btnRunT06;
    private Button btnRunT07;
    private Button btnRunT08;

    private TextBox txtT1Code;
    private TextBox txtT1UserPdf;
    private TextBox txtT1AnsPdf;
    private TextBox txtName;
    private TextBox txtTestNo;
    private TextBox txtSeatNo;
    private ComboBox cmbLoopType;

    private TextBox txtT06Exe;
    private TextBox txtT06Data;
    private TextBox txtT07Exe;
    private TextBox txtT07Data;
    private TextBox txtT08Exe;
    private TextBox txtT08Data;

    private ComboBox cmbLanguage;
    private Label lblLanguage;
}
