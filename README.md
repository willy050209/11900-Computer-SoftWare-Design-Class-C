# 11900 電腦軟體設計丙級 - 自動化測試系統 (V2.1)

本專案提供針對「電腦軟體設計丙級」試題的現代化自動化檢測工具。透過 **Roslyn 語義分析** 與 **FlaUI 穩定強化架構**，為應試人提供工業級的程式碼品質檢查與 UI 邏輯驗證。支援 **C#** 與 **VB.NET** 雙語言版本。

## 專案結構

- `/C#`: C# 術科程式碼。
  - `第一套`: 實作 for, while, do-while 三種迴圈版本。
  - `第二套`: 術科第二套試題 (1060306, 1060307, 1060308)。
- `/VB`: VB.NET 術科程式碼。
- `/Test`: 自動化測試工具集
  - `TestLauncher`: 視覺化測試啟動器 (WinForm GUI)。
  - `Task1Tester`: 第一站專屬測試專案 (含 Roslyn 代碼分析、PDF 內容提取與比對)。
  - `WinFormUITester`: 第二站專屬 UI 自動化測試專案 (基於強化型 POM 組件架構)。
- `/TestReports`: (自動生成) 存放 HTML 格式的測試詳細報告。

## 核心技術優化 (V2.1 更新內容)

### 1. Roslyn 語義檢查器 (`CodeValidatorService`)
不再依賴脆弱的正則表達式 (Regex)，改用 **Microsoft.CodeAnalysis (Roslyn)** 語法樹分析：
- **精確識別**：徹底區分「真正的程式碼關鍵字」與「註解中的文字」，零誤判。
- **違規偵測**：精確鎖定 `goto` 使用、禁用函式呼叫 (如 `Array.Sort`, `Math.Max`) 以及混用迴圈類型。
- **行號回報**：報告中會直接指出違反規則的具體行號，方便考生快速修正。

### 2. 強化型 UI 測試架構 (Stable POM)
針對 Windows 不同環境下的 UI 樹變異進行了穩定性強化：
- **自動降級比對機制**：若 UIA3 驅動程式無法獲取 DataGridView 標題 (Header)，系統會自動輸出警告並跳過標題檢查，優先確保核心「資料列邏輯」的比對。
- **相對定位演算法**：應檢人資料 (姓名、編號) 採用「標籤-控制項」相對座標搜尋，徹底解決 AutomationId 變動導致的定位失敗。
- **跨進程安全搜尋**：尋找視窗時採用 `ProcessId` 與 `ControlType` 雙重過濾，確保在多視窗環境下精確鎖定目標。

### 3. 資料清洗與 Null 安全
- **(null) 標記處理**：自動將 DataGridView 中的 `(null)` 預設字串轉換為空字串，確保與檢定手冊邏輯一致。
- **編譯級 Null 安全**：全面修復 C# 10 可為 Null 參考型別警告，提升測試工具本身的健壯性。

### 4. 自動化診斷與報告
- **失敗瞬間截圖**：測試失敗時自動捕捉現場畫面，存檔於 `Screenshots/`。
- **HTML 視覺化報告**：整合測試結果、耗時與錯誤明細，產出美觀的驗證清單。

## 快速開始

### 1. 配置測試環境
編輯 `Test/WinFormUITester/testsettings.json` 以調整執行檔路徑：
```json
{
  "DefaultCandidate": { "Name": "陳宇威", "TestNo": "112590005", "SeatNo": "005" },
  "Tasks": { ... }
}
```

### 2. 執行方式
- **視覺化 (推薦)**：執行 `Test/TestLauncher/bin/Debug/net10.0-windows/TestLauncher.exe`。
- **命令列**：`dotnet test Test/WinFormUITester/WinFormUITester.csproj --logger "html"`。

## 注意事項
- **環境要求**：需安裝 .NET 10 SDK。
- **UI 測試**：執行期間請勿操作滑鼠鍵盤，以免干擾自動化流程。
- **Git 忽略**：測試產出的 `TestReports/` 與 `Screenshots/` 已加入 `.gitignore`。

---
產出工具：Gemini CLI 測試自動化套件 (V2.1)
更新日期：2026/04/03
