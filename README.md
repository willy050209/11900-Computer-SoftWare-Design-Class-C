# 11900 電腦軟體設計丙級 - 自動化測試系統 (V2.0)

本專案提供針對「電腦軟體設計丙級」試題的現代化自動化檢測工具。透過 **Roslyn 語義分析** 與 **FlaUI 組件化架構**，為應試人提供工業級的程式碼品質檢查與 UI 佈局驗證。支援 **C#** 與 **VB.NET** 雙語言版本。

## 專案結構

- `/C#`: C# 術科程式碼。
  - `第一套`: 實作 for, while, do-while 三種迴圈版本。
  - `第二套`: 術科第二套試題 (1060306, 1060307, 1060308)。
- `/VB`: VB.NET 術科程式碼。
- `/Test`: 自動化測試工具集
  - `TestLauncher`: 視覺化測試啟動器 (WinForm GUI)。
  - `Task1Tester`: 第一站專屬測試專案 (含 Roslyn 代碼分析、PDF 內容提取與比對)。
  - `WinFormUITester`: 第二站專屬 UI 自動化測試專案 (基於 FlaUI POM 組件架構)。
- `/TestReports`: (自動生成) 存放 HTML 格式的測試詳細報告。

## 核心技術優化 (V2.0 更新內容)

### 1. Roslyn 語義檢查器 (`CodeValidatorService`)
不再依賴脆弱的正則表達式 (Regex)，改用 **Microsoft.CodeAnalysis (Roslyn)** 語法樹分析：
- **精確識別**：徹底區分「真正的程式碼關鍵字」與「註解中的文字」，零誤判。
- **違規偵測**：精確鎖定 `goto` 使用、禁用函式呼叫 (如 `Array.Sort`, `Math.Max`) 以及混用迴圈類型。
- **行號回報**：報告中會直接指出違反規則的具體行號，方便考生快速修正。

### 2. 組件化 UI 測試架構 (POM Components)
重構 `WinFormUITester` 為組件化設計，提升測試穩定性：
- **`OpenFileDialogComponent`**：穩定處理 Windows 標準開啟檔案對話框，內建 Retry 等待機制。
- **`CandidateInfoComponent`**：採用標籤相對定位演算法，自動驗證考生資料區塊。
- **移除 Thread.Sleep**：全面改用異步等待 (Retry)，大幅提升測試執行效率與環境適應力。

### 3. 失敗截圖與診斷機制
當 UI 測試失敗或驗證不符時，系統會自動執行以下動作：
- **自動截圖**：於失敗瞬間捕捉當前視窗畫面，存檔於 `Screenshots/` 目錄。
- **現場保留**：保留失敗時的 UI 狀態，配合截圖讓應檢人能直觀發現「標題錯字」或「欄位遺漏」。

### 4. HTML 視覺化測試報告
測試執行完畢後，將自動產出美觀的 HTML 報告：
- **第一站報告**：整合 PDF 標題、內容比對結果與程式碼違規明細。
- **第二站報告**：整合 xUnit 測試結果，包含每個測試案例的耗時與詳細錯誤堆疊。
- **存放位置**：統一存放於根目錄之 `/TestReports` 資料夾。

## 快速開始

### 1. 配置測試環境
編輯 `Test/WinFormUITester/testsettings.json` 以調整預設的考生資訊與執行檔路徑：
```json
{
  "DefaultCandidate": { "Name": "陳宇威", "TestNo": "112590005", "SeatNo": "005" },
  "Tasks": { ... }
}
```

### 2. 執行啟動器 (GUI)
```powershell
dotnet run --project Test/TestLauncher/TestLauncher.csproj
```

### 3. 命令列執行 (CLI)
若需單獨測試第一站：
```powershell
dotnet run --project Test/Task1Tester/Task1Tester/Task1Tester.csproj -- <code_path> <user_pdf_path> <ans_pdf_path> <name> <test_no> <seat_no> <loop_type> [report_path]
```

## 注意事項
- **Git 忽略**：測試產出的 `TestReports/` 與 `Screenshots/` 已加入 `.gitignore`，不會被上傳。
- **環境要求**：需安裝 .NET 10 SDK。
- **UI 測試**：執行期間請勿操作滑鼠鍵盤，以免干擾自動化流程。

---
產出工具：Gemini CLI 測試自動化套件 (V2.0)
更新日期：2026/04/02
