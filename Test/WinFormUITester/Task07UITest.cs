using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using System.IO;

namespace WinFormUITester;

[Collection("Sequential UI Tests")]
public class Task07UITest : IDisposable
{
    private readonly FlaUI.Core.Application _app;
    private readonly UIA3Automation _automation;
    private readonly string _exePath;
    private readonly string _testFilePath;

    public Task07UITest()
    {
        string baseDir = AppContext.BaseDirectory;
        string solutionDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", ".."));
        if (!Directory.Exists(Path.Combine(solutionDir, "C#"))) solutionDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "..", ".."));

        _exePath = Path.Combine(solutionDir, "C#", "第二套", "11900-1060307", "11900-1060307", "bin", "Debug", "net10.0-windows", "11900-1060307.exe");
        _testFilePath = Path.Combine(solutionDir, "範例光碟(公告測試參考資料區)-修正", "1060307.SM");

        if (!File.Exists(_exePath)) throw new FileNotFoundException($"找不到執行檔: {_exePath}");

        _automation = new UIA3Automation();
        _app = FlaUI.Core.Application.Launch(_exePath);
    }

    [Fact]
    public void TestTask07_PokerGameFlow()
    {
        // 先給予一點啟動緩衝，避免 UIA 鎖死
        Thread.Sleep(2000);

        // 使用較輕量的 GetMainWindow，這在對話框阻塞時較不易超時
        var window = _app.GetMainWindow(_automation, TimeSpan.FromSeconds(15));

        Assert.NotNull(window);
        var page = new MainFormPage(window);

        // 1. 處理檔案對話框
        page.HandleOpenFileDialog(_testFilePath);

        // 2. 驗證基本資料
        Thread.Sleep(2000); 
        Assert.Equal("陳宇威", page.NameTextBox.Text);

        // 3. 驗證 DataGridView 內容 (撲克牌)
        var grid = page.ResultsGrid;
        Assert.True(grid.Rows.Length > 0, "撲克牌比大小應該有回合資料");

        // 隨機檢查一筆資料是否包含撲克牌符號 (♠, ♥, ♦, ♣)
        var firstPlayerCard = grid.Rows[0].Cells[1].Value;
        Assert.Matches("[♠♥♦♣]", firstPlayerCard);
    }

    public void Dispose()
    {
        _app?.Close();
        _automation?.Dispose();
    }
}
