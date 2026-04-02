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

        string defaultExePath = Path.Combine(solutionDir, "C#", "第二套", "11900-1060308", "11900-1060308", "bin", "Debug", "net10.0-windows", "11900-1060308.exe");
        string defaultTestFilePath = Path.Combine(solutionDir, "範例光碟(公告測試參考資料區)-修正", "1060308.SM");

        _exePath = TestSettings.GetExePath("task08", defaultExePath);
        _testFilePath = TestSettings.GetTestDataPath("task08", defaultTestFilePath);

        if (!File.Exists(_exePath)) throw new FileNotFoundException($"找不到執行檔: {_exePath}");

        _automation = new UIA3Automation();
        _app = FlaUI.Core.Application.Launch(_exePath);
    }

    [Fact]
    public void TestTask08_FractionArithmeticFlow()
    {
        // 1. 等待對話框出現
        var dialogElement = FlaUI.Core.Tools.Retry.WhileNull(() => 
        {
            return _automation.GetDesktop().FindAllChildren(cf => cf.ByProcessId(_app.ProcessId))
                   .FirstOrDefault(w => w.ClassName == "#32770");
        }, TimeSpan.FromSeconds(20)).Result;
        
        Assert.NotNull(dialogElement);
        var dialogPage = new MainFormPage(dialogElement.AsWindow());
        
        // 2. 處理檔案對話框
        dialogPage.HandleOpenFileDialog(_testFilePath);
        
        // 3. 對話框處理完後，主視窗應該會出現
        Thread.Sleep(3000);
        var mainWin = FlaUI.Core.Tools.Retry.WhileNull(() => 
        {
            return _automation.GetDesktop().FindAllChildren(cf => cf.ByProcessId(_app.ProcessId))
                   .FirstOrDefault(w => w.ClassName != "#32770" && !string.IsNullOrEmpty(w.Name));
        }, TimeSpan.FromSeconds(15)).Result;

        Assert.NotNull(mainWin);
        var mainPage = new MainFormPage(mainWin.AsWindow());

        // 4. 驗證資料與 UI
        Thread.Sleep(5000); 
        
        // 驗證 UI 佈局
        mainPage.VerifyUILayout("分數加、減、乘、除運算", new[] { "VALUE1", "OP", "VALUE2", "ANSWER" });

        Assert.Equal("陳宇威", mainPage.NameTextBox.Text);
        var grid = mainPage.ResultsGrid;
        Assert.True(grid.Rows.Length > 0, "分數運算應該有結果資料");

        // 驗證第一筆運算結果是否包含分數格式或整數
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
