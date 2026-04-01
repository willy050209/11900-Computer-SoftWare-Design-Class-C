using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using System.IO;

namespace WinFormUITester;

[Collection("Sequential UI Tests")]
public class Task08UITest : IDisposable
{
    private readonly FlaUI.Core.Application _app;
    private readonly UIA3Automation _automation;
    private readonly string _exePath;
    private readonly string _testFilePath;

    public Task08UITest()
    {
        string baseDir = AppContext.BaseDirectory;
        string solutionDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", ".."));
        if (!Directory.Exists(Path.Combine(solutionDir, "C#"))) solutionDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "..", ".."));

        _exePath = Path.Combine(solutionDir, "C#", "第二套", "11900-1060308", "11900-1060308", "bin", "Debug", "net10.0-windows", "11900-1060308.exe");
        _testFilePath = Path.Combine(solutionDir, "範例光碟(公告測試參考資料區)-修正", "1060308.SM");

        if (!File.Exists(_exePath)) throw new FileNotFoundException($"找不到執行檔: {_exePath}");

        _automation = new UIA3Automation();
        _app = FlaUI.Core.Application.Launch(_exePath);
    }

    [Fact]
    public void TestTask08_FractionArithmeticFlow()
    {
        Thread.Sleep(2000);
        var window = _app.GetMainWindow(_automation, TimeSpan.FromSeconds(15));

        Assert.NotNull(window);
        var page = new MainFormPage(window);

        // 1. 處理檔案對話框
        page.HandleOpenFileDialog(_testFilePath);
        
        // 2. 驗證基本資料
        Thread.Sleep(2000); 
        Assert.Equal("陳宇威", page.NameTextBox.Text);

        // 3. 驗證 DataGridView 內容 (分數運算結果)
        var grid = page.ResultsGrid;
        Assert.True(grid.Rows.Length > 0, "分數運算應該有結果資料");

        // 驗證第一筆運算結果是否包含分數格式或整數
        // 1060308.SM 的內容範例通常是 a,b,op,c,d
        var firstAnswer = grid.Rows[0].Cells[3].Value;
        Assert.NotEmpty(firstAnswer);
        // 檢查是否符合整數或分數格式 (例如 "1", "1/2", "-3/4")
        Assert.Matches(@"^-?\d+(/\d+)?$", firstAnswer);
    }

    public void Dispose()
    {
        _app?.Close();
        _automation?.Dispose();
    }
}
