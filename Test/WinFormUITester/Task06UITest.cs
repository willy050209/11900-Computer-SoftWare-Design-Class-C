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
        Thread.Sleep(2000);
        var window = _app.GetMainWindow(_automation, TimeSpan.FromSeconds(15));

        Assert.NotNull(window);
        var page = new MainFormPage(window);
        page.HandleOpenFileDialog(_testFilePath);
        
        Thread.Sleep(2000); 
        Assert.Equal("陳宇威", page.NameTextBox.Text);
        Assert.True(page.ResultsGrid.Rows.Length > 0);
    }

    public void Dispose()
    {
        _app?.Close();
        _automation?.Dispose();
    }
}
