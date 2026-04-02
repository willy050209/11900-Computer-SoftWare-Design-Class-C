// filepath: Services/LocalizationService.cs
namespace TestLauncher.Services;

public enum Language { English, TraditionalChinese }

public class LocalizationService
{
    private Language _currentLanguage = Language.TraditionalChinese;

    public Language CurrentLanguage
    {
        get => _currentLanguage;
        set { _currentLanguage = value; LanguageChanged?.Invoke(); }
    }

    public event Action? LanguageChanged;

    public string GetString(string key) => _currentLanguage switch
    {
        Language.TraditionalChinese => GetChinese(key),
        _ => GetEnglish(key)
    };

    private string GetEnglish(string key) => key switch
    {
        "Title" => "Automated Test Launcher - Level C Software Design",
        "Task1Tab" => "Task 1 (Code & PDF)",
        "Task2Tab" => "Task 2 (UI Automation)",
        "CodePath" => "Code Path:",
        "UserPdf" => "User PDF:",
        "AnsPdf" => "Ans PDF:",
        "Name" => "Name:",
        "TestNo" => "Test No:",
        "SeatNo" => "Seat No:",
        "LoopType" => "Loop Type:",
        "RunTask1" => "Run Task 1 Validation",
        "Exe" => "Exe:",
        "Data" => "Data:",
        "RunT06" => "Run T06",
        "RunT07" => "Run T07",
        "RunT08" => "Run T08",
        "RunAllTask2" => "Run All Task 2 Tests",
        "Language" => "Language:",
        "CandidateGroup" => "Candidate Information",
        _ => key
        };

        private string GetChinese(string key) => key switch
        {
        "Title" => "電腦軟體設計丙級 - 自動化測試啟動器",
        "Task1Tab" => "第一站 (程式碼與 PDF 驗證)",
        "Task2Tab" => "第二套 (UI 自動化測試)",
        "CodePath" => "程式碼路徑:",
        "UserPdf" => "應檢人 PDF:",
        "AnsPdf" => "參考解答 PDF:",
        "Name" => "姓名:",
        "TestNo" => "術科編號:",
        "SeatNo" => "座號:",
        "LoopType" => "迴圈型態:",
        "RunTask1" => "執行第一站驗證",
        "Exe" => "執行檔:",
        "Data" => "測試資料:",
        "RunT06" => "執行 06",
        "RunT07" => "執行 07",
        "RunT08" => "執行 08",
        "RunAllTask2" => "執行全部第二套測試",
        "Language" => "語言:",
        "CandidateGroup" => "應檢人資料",
        _ => key
        };
}
