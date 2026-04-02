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

        string defaultExePath = Path.Combine(solutionDir, "C#", "第二套", "11900-1060306", "11900-1060306", "bin", "Debug", "net10.0-windows", "11900-1060306.exe");
        string defaultTestFilePath = Path.Combine(solutionDir, "範例光碟(公告測試參考資料區)-修正", "1060306.SM");

        _exePath = TestSettings.GetExePath("task06", defaultExePath);
        _testFilePath = TestSettings.GetTestDataPath("task06", defaultTestFilePath);

        if (!File.Exists(_exePath)) throw new FileNotFoundException($"找不到執行檔: {_exePath}");

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

        // 4. 驗證資料與 UI
        Thread.Sleep(5000); 
        
        // 驗證 UI 佈局 (標題、群組框、標籤、欄位、應檢人資料)
        mainPage.VerifyUILayout("身分證號碼檢查", 
            new[] { "ID_NO", "NAME", "SEX", "ERROR" },
            TestSettings.GetCandidateName(),
            TestSettings.GetCandidateTestNo(),
            TestSettings.GetCandidateSeatNo());

        Assert.Equal(TestSettings.GetCandidateName(), mainPage.GetValueByLabel("姓名"));
        Assert.True(mainPage.ResultsGrid.Rows.Length > 0, "身分證檢查應該有結果資料");

        // 5. 驗證資料列數值
        mainPage.VerifyData(row => {
            string id = row[0];
            string sex = row[2];
            string actualError = row[3];
            string expectedError = ValidationService.GetIdCardError(id, sex);
            
            if (actualError != expectedError)
            {
                throw new Exception($"身分證 {id} ({sex}) 驗證錯誤。預期: '{expectedError}', 實際: '{actualError}'");
            }
        });
    }

    public void Dispose()
    {
        _app?.Close();
        _automation?.Dispose();
    }
}
