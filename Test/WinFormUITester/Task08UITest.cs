using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using System.IO;
using System.Linq;

namespace WinFormUITester;

[Collection("Sequential UI Tests")]
public class Task08UITest : IDisposable
{
    private readonly FlaUI.Core.Application _app;
    private readonly UIA3Automation _automation;
    private readonly TaskConfig _config;

    public Task08UITest()
    {
        _config = TestSettings.GetTaskConfig("task08");
        if (!File.Exists(_config.ExePath)) throw new FileNotFoundException($"找不到執行檔: {_config.ExePath}");
        _automation = new UIA3Automation();
        _app = FlaUI.Core.Application.Launch(_config.ExePath);
    }

    [Fact]
    public void TestTask08_FractionArithmeticFlow()
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
                if (cells.Length < 4 || string.IsNullOrEmpty(cells[0]) || string.IsNullOrEmpty(cells[1])) continue;
                string v1 = cells[0], op = cells[1], v2 = cells[2], actualAns = cells[3];
                string expectedAns = ValidationService.GetFractionAnswer(v1, op, v2);
                if (actualAns != expectedAns)
                    throw new Exception($"分數運算錯誤。{v1} {op} {v2}。預期: '{expectedAns}', 實際: '{actualAns}'");
            }
        }
        catch (Exception)
        {
            var activeWindow = _app.GetMainWindow(_automation) ?? _automation.GetDesktop();
            TestSettings.CaptureScreenshot(activeWindow, "Task08_Failure");
            throw;
        }
    }

    public void Dispose()
    {
        _app?.Close();
        _automation?.Dispose();
    }
}
