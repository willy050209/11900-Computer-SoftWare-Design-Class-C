// filepath: Views/MainForm.cs
#nullable enable
using System.Text;
using TestLauncher.Models;
using TestLauncher.Presenters;
using TestLauncher.Services;

namespace TestLauncher.Views;

public partial class MainForm : Form, IMainView
{
    private readonly MainPresenter _presenter;

    public MainForm(Func<IMainView, MainPresenter> presenterFactory)
    {
        InitializeComponent();
        _presenter = presenterFactory(this);
        
        // 設定預設路徑 (以便快速測試)
        string baseDir = AppContext.BaseDirectory;
        string root = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", ".."));
        if (!Directory.Exists(Path.Combine(root, "C#"))) root = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "..", ".."));

        Task1CodePath = Path.Combine(root, "C#", "第一站", "第一站", "Program.cs");
        Task1UserPdfPath = Path.Combine(root, "C#", "第一站", "第一站", "bin", "Debug", "net10.0-windows", "output.pdf");
        Task1AnsPdfPath = Path.Combine(root, "ans.pdf");
        
        CandidateName = "陳宇威";
        CandidateTestNo = "112590005";
        CandidateSeatNo = "005";
        LoopType = "for";

        Task06ExePath = Path.Combine(root, "C#", "第二套", "11900-1060306", "11900-1060306", "bin", "Debug", "net10.0-windows", "11900-1060306.exe");
        Task06DataPath = Path.Combine(root, "範例光碟(公告測試參考資料區)-修正", "1060306.SM");
        
        Task07ExePath = Path.Combine(root, "C#", "第二套", "11900-1060307", "11900-1060307", "bin", "Debug", "net10.0-windows", "11900-1060307.exe");
        Task07DataPath = Path.Combine(root, "範例光碟(公告測試參考資料區)-修正", "1060307.SM");

        Task08ExePath = Path.Combine(root, "C#", "第二套", "11900-1060308", "11900-1060308", "bin", "Debug", "net10.0-windows", "11900-1060308.exe");
        Task08DataPath = Path.Combine(root, "範例光碟(公告測試參考資料區)-修正", "1060308.SM");
    }

    // Task 1
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Task1CodePath { get => txtT1Code.Text; set => txtT1Code.Text = value; }
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Task1UserPdfPath { get => txtT1UserPdf.Text; set => txtT1UserPdf.Text = value; }
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Task1AnsPdfPath { get => txtT1AnsPdf.Text; set => txtT1AnsPdf.Text = value; }
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string CandidateName { get => txtName.Text; set => txtName.Text = value; }
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string CandidateTestNo { get => txtTestNo.Text; set => txtTestNo.Text = value; }
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string CandidateSeatNo { get => txtSeatNo.Text; set => txtSeatNo.Text = value; }
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string LoopType { get => cmbLoopType.SelectedItem?.ToString() ?? "for"; set => cmbLoopType.SelectedItem = value; }

    // Task 2
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Task06ExePath { get => txtT06Exe.Text; set => txtT06Exe.Text = value; }
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Task06DataPath { get => txtT06Data.Text; set => txtT06Data.Text = value; }
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Task07ExePath { get => txtT07Exe.Text; set => txtT07Exe.Text = value; }
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Task07DataPath { get => txtT07Data.Text; set => txtT07Data.Text = value; }
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Task08ExePath { get => txtT08Exe.Text; set => txtT08Exe.Text = value; }
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Task08DataPath { get => txtT08Data.Text; set => txtT08Data.Text = value; }

    public void AppendLog(string text, System.Drawing.Color? color = null)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => AppendLog(text, color)));
            return;
        }

        rtbLog.SelectionStart = rtbLog.TextLength;
        rtbLog.SelectionLength = 0;
        rtbLog.SelectionColor = color ?? System.Drawing.Color.White;
        rtbLog.AppendText(text);
        rtbLog.SelectionStart = rtbLog.TextLength;
        rtbLog.ScrollToCaret();
    }

    public void ClearLog()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(ClearLog));
            return;
        }
        rtbLog.Clear();
    }

    public void SetBusy(bool busy)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => SetBusy(busy)));
            return;
        }
        btnRunT1.Enabled = !busy;
        btnRunT06.Enabled = !busy;
        btnRunT07.Enabled = !busy;
        btnRunT08.Enabled = !busy;
        btnRunAllTask2.Enabled = !busy;
        this.Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
    }

    public string? ShowFileOpenDialog(string filter)
    {
        using OpenFileDialog ofd = new();
        ofd.Filter = filter;
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            return ofd.FileName;
        }
        return null;
    }

    public void UpdateLocalizedText(Func<string, string> getStr)
    {
        this.Text = getStr("Title");
        menuLanguage.Text = getStr("Language");
        tabTask1.Text = getStr("Task1Tab");
        tabTask2.Text = getStr("Task2Tab");

        // Common Candidate Info
        grpCommonCandidate.Text = getStr("CandidateGroup");
        UpdateControlText("lblName", getStr("Name"));
        UpdateControlText("lblTestNo", getStr("TestNo"));
        UpdateControlText("lblSeatNo", getStr("SeatNo"));

        // Task 1
        UpdateControlText("lblCodePath", getStr("CodePath"));
        UpdateControlText("lblUserPdf", getStr("UserPdf"));
        UpdateControlText("lblAnsPdf", getStr("AnsPdf"));
        UpdateControlText("lblLoopType", getStr("LoopType"));
        btnRunT1.Text = getStr("RunTask1");

        // Task 2
        UpdateControlText("lblExe06", getStr("Exe"));
        UpdateControlText("lblData06", getStr("Data"));
        btnRunT06.Text = getStr("RunT06");

        UpdateControlText("lblExe07", getStr("Exe"));
        UpdateControlText("lblData07", getStr("Data"));
        btnRunT07.Text = getStr("RunT07");

        UpdateControlText("lblExe08", getStr("Exe"));
        UpdateControlText("lblData08", getStr("Data"));
        btnRunT08.Text = getStr("RunT08");

        btnRunAllTask2.Text = getStr("RunAllTask2");
    }

    private void UpdateControlText(string name, string text)
    {
        var controls = this.Controls.Find(name, true);
        if (controls.Length > 0) controls[0].Text = text;
    }

    private async void btnRunT1_Click(object? sender, EventArgs e) => await _presenter.RunTask1Async();
    private async void btnRunT06_Click(object? sender, EventArgs e) => await _presenter.RunTask2Async("task06");
    private async void btnRunT07_Click(object? sender, EventArgs e) => await _presenter.RunTask2Async("task07");
    private async void btnRunT08_Click(object? sender, EventArgs e) => await _presenter.RunTask2Async("task08");
    private async void btnRunAllTask2_Click(object? sender, EventArgs e) => await _presenter.RunAllTask2Async();

    private void btnBrowse_Click(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.Tag is TextBox target)
        {
            string filter = "All Files (*.*)|*.*";
            if (target.Name.Contains("Pdf", StringComparison.OrdinalIgnoreCase)) filter = "PDF Files (*.pdf)|*.pdf";
            if (target.Name.Contains("Code", StringComparison.OrdinalIgnoreCase)) filter = "C# Files (*.cs)|*.cs";
            if (target.Name.Contains("Exe", StringComparison.OrdinalIgnoreCase)) filter = "Executable Files (*.exe)|*.exe";
            if (target.Name.Contains("Data", StringComparison.OrdinalIgnoreCase)) filter = "SM Files (*.sm)|*.sm|All Files (*.*)|*.*";

            string? path = ShowFileOpenDialog(filter);
            if (path != null) target.Text = path;
        }
    }

    private void OnLanguageMenuClick(Language lang)
    {
        _presenter.ChangeLanguage(lang);
    }
}
