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

        // ... (省略中間不變的設定碼) ...
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
