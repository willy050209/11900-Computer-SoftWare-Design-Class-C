using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using System.IO;

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
            // 1. 處理開啟檔案對話框 (Component 化)
            var dialogElement = FlaUI.Core.Tools.Retry.WhileNull(() => 
                _automation.GetDesktop().FindAllChildren(cf => cf.ByProcessId(_app.ProcessId))
                .FirstOrDefault(w => w.ClassName == "#32770"), TimeSpan.FromSeconds(20)).Result;
            
            Assert.NotNull(dialogElement);
            new OpenFileDialogComponent(dialogElement.AsWindow()).OpenFile(_config.TestDataPath);
            
            // 2. 獲取主視窗 (POM 化)
            var mainWin = FlaUI.Core.Tools.Retry.WhileNull(() => 
                _automation.GetDesktop().FindAllChildren(cf => cf.ByProcessId(_app.ProcessId))
                .FirstOrDefault(w => w.ClassName != "#32770" && !string.IsNullOrEmpty(w.Name)), 
                TimeSpan.FromSeconds(15)).Result;

            Assert.NotNull(mainWin);
            var mainPage = new TaskMainPage(mainWin.AsWindow());

            // 3. 驗證 UI 與資料
            FlaUI.Core.Tools.Retry.WhileTrue(() => mainPage.ResultsGrid.Rows.Length == 0, TimeSpan.FromSeconds(10));
            mainPage.VerifyLayout(_config);
            mainPage.CandidateInfo.VerifyInfo(
                TestSettings.GetCandidateName(), 
                TestSettings.GetCandidateTestNo(), 
                TestSettings.GetCandidateSeatNo());

            // 4. 驗證 Grid 資料邏輯
            var rows = mainPage.ResultsGrid.Rows;
            string? previousId = null;
            foreach (var row in rows)
            {
                var cells = row.Cells.Select(c => c.Value?.ToString()?.Trim() ?? "").ToArray();
                string id = cells[0], name = cells[1], sex = cells[2], actualError = cells[3];

                if (previousId != null && string.Compare(id, previousId, StringComparison.Ordinal) < 0)
                    throw new Exception($"未按 ID 排序: {id} 出現在 {previousId} 之後");
                previousId = id;

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
