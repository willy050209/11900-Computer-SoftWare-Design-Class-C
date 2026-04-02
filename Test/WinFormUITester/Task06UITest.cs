using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using System.IO;
using System.Linq;

namespace WinFormUITester;

[Collection("Sequential UI Tests")]
public class Task06UITest : IDisposable
{
    private readonly FlaUI.Core.Application _app;
    private readonly UIA3Automation _automation;
    private readonly TaskConfig _config;

    public Task06UITest()
    {
        _config = TestSettings.GetTaskConfig("task06");
        if (!File.Exists(_config.ExePath)) throw new FileNotFoundException($"找不到執行檔: {_config.ExePath}");
        _automation = new UIA3Automation();
        _app = FlaUI.Core.Application.Launch(_config.ExePath);
    }

    [Fact]
    public void TestTask06_FullFlow()
    {
        try 
        {
            var dialogElement = FlaUI.Core.Tools.Retry.WhileNull(() => 
                _automation.GetDesktop().FindAllChildren().FirstOrDefault(w => {
                    try { return w.Properties.ProcessId.Value == _app.ProcessId && (w.Name.Contains("開啟") || w.ControlType == FlaUI.Core.Definitions.ControlType.Window); }
                    catch { return false; }
                }), TimeSpan.FromSeconds(20)).Result;
            
            Assert.NotNull(dialogElement);
            new OpenFileDialogComponent(dialogElement.AsWindow()).OpenFile(_config.TestDataPath);
            
            var mainWin = FlaUI.Core.Tools.Retry.WhileNull(() => 
                _automation.GetDesktop().FindAllChildren().FirstOrDefault(w => {
                    try { return w.Properties.ProcessId.Value == _app.ProcessId && !w.Name.Contains("開啟") && !string.IsNullOrEmpty(w.Name); }
                    catch { return false; }
                }), TimeSpan.FromSeconds(15)).Result;

            Assert.NotNull(mainWin);
            var mainPage = new TaskMainPage(mainWin.AsWindow());

            FlaUI.Core.Tools.Retry.WhileTrue(() => (mainPage.ResultsGrid?.Rows.Length ?? 0) == 0, TimeSpan.FromSeconds(10));
            mainPage.VerifyLayout(_config);
            mainPage.CandidateInfo.VerifyInfo(TestSettings.GetCandidateName(), TestSettings.GetCandidateTestNo(), TestSettings.GetCandidateSeatNo());

            var rows = mainPage.ResultsGrid?.Rows ?? Array.Empty<FlaUI.Core.AutomationElements.DataGridViewRow>();
            foreach (var row in rows)
            {
                var cells = row.Cells.Select(c => {
                    string val = "";
                    try { val = c.Value?.ToString() ?? ""; } catch { }
                    return val == "(null)" ? "" : val.Trim();
                }).ToArray();
                if (cells.Length < 4 || string.IsNullOrEmpty(cells[0])) continue;
                
                string id = cells[0], name = cells[1], sex = cells[2], actualError = cells[3];
                string expectedError = ValidationService.GetIdCardError(id, sex);
                if (actualError != expectedError)
                    throw new Exception($"ID {id} 錯誤預期 '{expectedError}' 實際 '{actualError}'");
            }
        }
        catch (Exception)
        {
            var activeWindow = _app.GetMainWindow(_automation) ?? _automation.GetDesktop();
            TestSettings.CaptureScreenshot(activeWindow, "Task06_Failure");
            throw;
        }
    }

    public void Dispose()
    {
        _app?.Close();
        _automation?.Dispose();
    }
}
