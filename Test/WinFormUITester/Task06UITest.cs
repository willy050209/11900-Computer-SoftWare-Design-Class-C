using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using System.IO;

namespace WinFormUITester;

[Collection("Sequential UI Tests")]
public class Task06UITest : IDisposable
{
    private readonly FlaUI.Core.Application _app;
    private readonly UIA3Automation _automation;
    private readonly string _exePath;
    private readonly string _testFilePath;

    public Task06UITest()
    {
        string baseDir = AppContext.BaseDirectory;
        string solutionDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", ".."));
        if (!Directory.Exists(Path.Combine(solutionDir, "C#"))) solutionDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "..", ".."));

        _exePath = Path.Combine(solutionDir, "C#", "第二套", "11900-1060306", "11900-1060306", "bin", "Debug", "net10.0-windows", "11900-1060306.exe");
        _testFilePath = Path.Combine(solutionDir, "範例光碟(公告測試參考資料區)-修正", "1060306.SM");

        _automation = new UIA3Automation();
        _app = FlaUI.Core.Application.Launch(_exePath);
    }

    [Fact]
    public void TestTask06_FullFlow()
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

        // 4. 驗證資料
        Thread.Sleep(5000); 
        Assert.Equal("陳宇威", mainPage.NameTextBox.Text);
        Assert.True(mainPage.ResultsGrid.Rows.Length > 0, "身分證檢查應該有結果資料");
    }

    public void Dispose()
    {
        _app?.Close();
        _automation?.Dispose();
    }
}
