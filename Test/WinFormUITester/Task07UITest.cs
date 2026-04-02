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
                _automation.GetDesktop().FindAllChildren(cf => cf.ByProcessId(_app.ProcessId))
                .FirstOrDefault(w => w.ClassName == "#32770"), TimeSpan.FromSeconds(20)).Result;
            
            Assert.NotNull(dialogElement);
            new OpenFileDialogComponent(dialogElement.AsWindow()).OpenFile(_config.TestDataPath);
            
            var mainWin = FlaUI.Core.Tools.Retry.WhileNull(() => 
                _automation.GetDesktop().FindAllChildren(cf => cf.ByProcessId(_app.ProcessId))
                .FirstOrDefault(w => w.ClassName != "#32770" && !string.IsNullOrEmpty(w.Name)), 
                TimeSpan.FromSeconds(15)).Result;

            Assert.NotNull(mainWin);
            var mainPage = new TaskMainPage(mainWin.AsWindow());

            int expectedRounds = int.Parse(File.ReadLines(_config.TestDataPath).First().Trim());
            FlaUI.Core.Tools.Retry.WhileTrue(() => mainPage.ResultsGrid.Rows.Length < expectedRounds, TimeSpan.FromSeconds(10));
            
            Assert.Equal(expectedRounds, mainPage.ResultsGrid.Rows.Length);

            mainPage.VerifyLayout(_config);
            mainPage.CandidateInfo.VerifyInfo(
                TestSettings.GetCandidateName(), 
                TestSettings.GetCandidateTestNo(), 
                TestSettings.GetCandidateSeatNo());

            foreach (var row in mainPage.ResultsGrid.Rows)
            {
                var cells = row.Cells.Select(c => c.Value?.ToString()?.Trim() ?? "").ToArray();
                string playerCard = cells[1], bankerCard = cells[2], actualResult = cells[3];
                string expectedResult = ValidationService.GetPokerResult(playerCard, bankerCard);
                if (actualResult != expectedResult)
                    throw new Exception($"比對錯誤。玩家: {playerCard}, 莊家: {bankerCard}。預期: '{expectedResult}', 實際: '{actualResult}'");
                Assert.Matches("[♠♥♦♣]", playerCard);
                Assert.Matches("[♠♥♦♣]", bankerCard);
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
