using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using System.IO;
using System.Linq;

namespace WinFormUITester;

[Collection("Sequential UI Tests")]
public class Task07UITest : IDisposable
{
    private readonly FlaUI.Core.Application _app;
    private readonly UIA3Automation _automation;
    private readonly TaskConfig _config;

    public Task07UITest()
    {
        _config = TestSettings.GetTaskConfig("task07");
        if (!File.Exists(_config.ExePath)) throw new FileNotFoundException($"找不到執行檔: {_config.ExePath}");
        _automation = new UIA3Automation();
        _app = FlaUI.Core.Application.Launch(_config.ExePath);
    }

    [Fact]
    public void TestTask07_PokerGameFlow()
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

            int expectedRounds = int.Parse(File.ReadLines(_config.TestDataPath).First().Trim());
            FlaUI.Core.Tools.Retry.WhileTrue(() => (mainPage.ResultsGrid?.Rows.Length ?? 0) < expectedRounds, TimeSpan.FromSeconds(10));
            
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
                if (cells.Length < 4 || string.IsNullOrEmpty(cells[1]) || string.IsNullOrEmpty(cells[2])) continue;
                string playerCard = cells[1], bankerCard = cells[2], actualResult = cells[3];
                string expectedResult = ValidationService.GetPokerResult(playerCard, bankerCard);
                if (actualResult != expectedResult)
                    throw new Exception($"比對錯誤。玩家: {playerCard}, 莊家: {bankerCard}。預期: '{expectedResult}', 實際: '{actualResult}'");
            }
        }
        catch (Exception)
        {
            var activeWindow = _app.GetMainWindow(_automation) ?? _automation.GetDesktop();
            TestSettings.CaptureScreenshot(activeWindow, "Task07_Failure");
            throw;
        }
    }

    public void Dispose()
    {
        _app?.Close();
        _automation?.Dispose();
    }
}
