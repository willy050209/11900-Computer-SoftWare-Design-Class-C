# 11900 電腦軟體設計丙級 - 自動化測試系統

本專案提供針對「電腦軟體設計丙級」試題的自動化檢測工具，旨在協助應試人確認程式碼邏輯與輸出結果是否符合評分標準。

## 專案結構

- `/C#`: 包含考生開發的術科程式原始碼。
  - `第一站`: 術科第一題至第五題。
  - `第二套`: 術科第二套試題 (1060306, 1060307, 1060308)。
- `/Test`: 自動化測試工具集
  - `TestLauncher`: **[Updated]** 視覺化測試啟動器 (WinForm GUI)。
  - `Task1Tester`: 術科第一站專屬自動化測試專案 (含程式碼規範與 PDF 頁首校驗)。
  - `WinFormUITester`: 術科第二套專屬 UI 自動化測試專案 (基於 FlaUI)。
- `ans.pdf`: 術科測試參考答案 PDF。
- `119003B14.pdf`: 術科測試應試人參考資料。

## 視覺化測試工具 (TestLauncher)

為了提供更佳的使用者體驗，`TestLauncher` 經過了介面重構與穩定性強化。

### 核心功能：
- **全域應檢人資料**：將姓名、編號與座號獨立於視窗最上方，輸入一次即可應用於所有測試項目。
- **工具列語言切換**：語言切換功能已移至頂部選單 (MenuStrip)，釋放更多垂直空間以解決元件重疊問題。
- **優化 UI 佈局**：調整了 Task 2 的間距與 Log 區域的配置，確保在各種解析度下皆能清晰顯示。
- **現代化代碼標準**：完整實作 `#nullable enable`，專案編譯達到 **0 錯誤 0 警告**。
- **彩色 Log 輸出**：仿 PowerShell 風格，直觀區分執行指令 (青)、成功 (綠) 與失敗 (紅)。

### 執行方式：
在專案根目錄執行：
```powershell
dotnet run --project Test/TestLauncher/TestLauncher.csproj
```

---

## 測試工具說明 (命令列介面)

### 1. 術科第一站自動化測試 (Task1Tester)
用於驗證 940301~940305 的演算法邏輯與 PDF 輸出格式。

### 2. 術科第二套 UI 自動化測試 (WinFormUITester)
使用 FlaUI 模擬真實使用者操作，驗證 1060306~1060308 的 UI 互動與資料呈現。

#### 核心特點：
- **智慧型相對定位**：採用視覺座標演算法，自動辨識標籤 (如「姓名」) 右側最近的元件。不論應檢人使用 TextBox 或 Label，皆能精確抓取。
- **日期格式適應**：自動驗證第二套要求的 **西元年 (YYYY/MM/DD)** 格式。
- **強健的對話框處理**：自動處理 `OpenFileDialog` 並支援 `Alt+N` 強制聚焦。

#### 執行方式：
```powershell
# 執行所有 UI 測試
dotnet test Test/WinFormUITester/WinFormUITester.csproj

# 傳遞自定義應檢人資料進行驗證
dotnet test Test/WinFormUITester/WinFormUITester.csproj -- --name "姓名" --test-no "編號" --seat-no "座號"
```

## 注意事項
- UI 測試執行期間，請勿操作滑鼠或鍵盤，以免干擾自動化流程。
- 本工具預設資料已對齊範例程式 (陳宇威 / 112590005 / 005)。
- 開發環境建議：Visual Studio 2022 / .NET 10。
