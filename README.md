# 11900 電腦軟體設計丙級 - 自動化測試系統

本專案提供針對「電腦軟體設計丙級」試題的自動化檢測工具，旨在協助應試人確認程式碼邏輯與輸出結果是否符合評分標準。

## 專案結構

- `/C#`: 包含考生開發的術科程式原始碼。
  - `第一站`: 術科第一題至第五題。
  - `第二套`: 術科第二套試題 (1060306, 1060307, 1060308)。
- `/Test`: 自動化測試工具集
  - `TestLauncher`: **[New]** 視覺化測試啟動器 (WinForm GUI)。
  - `Task1Tester`: 術科第一站專屬自動化測試專案 (含程式碼規範與 PDF 頁首校驗)。
  - `WinFormUITester`: 術科第二套專屬 UI 自動化測試專案 (基於 FlaUI)。
- `ans.pdf`: 術科測試參考答案 PDF。
- `119003B14.pdf`: 術科測試應試人參考資料。

## 視覺化測試工具 (TestLauncher)

為了方便應試人使用，本專案提供了一個視覺化的啟動器介面，整合了所有自動化測試功能。

### 核心功能：
- **一鍵執行**：整合第一站與第二套所有測試項目。
- **多語系支援**：提供「繁體中文」與「English」介面切換。
- **彩色 Log 輸出**：仿 PowerShell 風格的彩色 Log，綠色代表成功、紅色代表失敗、青色代表執行指令。
- **路徑自動推算**：自動偵測專案結構並填入預設執行路徑與測試資料路徑。
- **UTF-8 相容**：完整支援中文路徑與內容，無亂碼問題。

### 執行方式：
在專案根目錄執行：
```powershell
dotnet run --project Test/TestLauncher/TestLauncher.csproj
```

---

## 測試工具說明 (命令列介面)

### 1. 術科第一站自動化測試 (Task1Tester)
用於驗證 940301~940305 的演算法邏輯與 PDF 輸出格式。

#### 執行方式：
```powershell
dotnet run --project Test/Task1Tester/Task1Tester/Task1Tester.csproj -- <code_path> <user_pdf_path> <ans_pdf_path> <name> <test_no> <seat_no> <loop_type>
```

### 2. 術科第二套 UI 自動化測試 (WinFormUITester)
使用 FlaUI 模擬真實使用者操作，驗證 1060306~1060308 的 UI 互動與資料呈現。

#### 核心特點：
- **動態參數支援**：可透過 args 自訂待測程式與資料檔案路徑。
- **強健的對話框處理**：自動填寫 `OpenFileDialog`。
- **精準聚焦技術**：使用 `Alt+N` 快捷鍵強制聚焦。

#### 執行方式：
```powershell
# 執行所有 UI 測試 (使用預設路徑)
dotnet test Test/WinFormUITester/WinFormUITester.csproj

# 指定特定執行檔與資料進行測試
dotnet test Test/WinFormUITester/WinFormUITester.csproj --filter Task06 -- --task06-exe "C:\Path\To\App.exe" --task06-data "C:\Path\To\Data.SM"
```

## 測試規範 (依據 119003B14 規定)

### 程式碼檢查 (Code Validation)
- **規則 6.2**: 禁止直接輸出結果。系統會偵測程式碼是否缺乏迴圈或判斷結構。
- **規則 6.3**: 禁止使用 `Go To` 指令。
- **規則 6.4**: 禁止使用內置演算法函數 (如 `Math.Sqrt`, `Array.Sort` 等)。

### PDF 輸出檢查 (PDF Validation)
- **頁首校驗**: 姓名、術科測試編號、座號，以及**民國格式日期** (`yyy/mm/dd`)。
- **內容校驗**: 比對第一題至第五題的結果文字是否與標準答案一致。

## 注意事項
- 請確保系統已安裝 .NET 10 SDK。
- UI 測試執行期間，請勿操作滑鼠或鍵盤，以免干擾自動化流程。
- 本工具僅供自我檢測參考，實際評分以監評人員為準。
