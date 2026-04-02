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

        string defaultExePath = Path.Combine(solutionDir, "C#", "第二套", "11900-1060307", "11900-1060307", "bin", "Debug", "net10.0-windows", "11900-1060307.exe");
        string defaultTestFilePath = Path.Combine(solutionDir, "範例光碟(公告測試參考資料區)-修正", "1060307.SM");

        _exePath = TestSettings.GetExePath("task07", defaultExePath);
        _testFilePath = TestSettings.GetTestDataPath("task07", defaultTestFilePath);

        if (!File.Exists(_exePath)) throw new FileNotFoundException($"找不到執行檔: {_exePath}");

        _automation = new UIA3Automation();
        _app = FlaUI.Core.Application.Launch(_exePath);
    }

    [Fact]
    public void TestTask07_PokerGameFlow()
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
        mainPage.VerifyUILayout("撲克牌比大小", 
            new[] { "序號", "玩家", "莊家", "結果" },
            TestSettings.GetCandidateName(),
            TestSettings.GetCandidateTestNo(),
            TestSettings.GetCandidateSeatNo());

        Assert.Equal(TestSettings.GetCandidateName(), mainPage.GetValueByLabel("姓名"));
        var grid = mainPage.ResultsGrid;
        Assert.True(grid.Rows.Length > 0, "撲克牌比大小應該有回合資料");

        // 5. 驗證資料列數值
        mainPage.VerifyData(row => {
            string playerCard = row[1];
            string bankerCard = row[2];
            string actualResult = row[3];
            string expectedResult = ValidationService.GetPokerResult(playerCard, bankerCard);

            if (actualResult != expectedResult)
            {
                throw new Exception($"撲克牌比對錯誤。玩家: {playerCard}, 莊家: {bankerCard}。預期: '{expectedResult}', 實際: '{actualResult}'");
            }
        });

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
