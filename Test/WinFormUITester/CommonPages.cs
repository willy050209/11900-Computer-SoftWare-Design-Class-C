using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;

namespace WinFormUITester;

/// <summary>
/// 代表第二套題目通用主視窗 Page Object
/// </summary>
public class MainFormPage
{
    private readonly Window _window;

    public MainFormPage(Window window)
    {
        _window = window;
    }

    public FlaUI.Core.AutomationElements.TextBox NameTextBox => FindControlWithRetry<FlaUI.Core.AutomationElements.TextBox>("txtName");
    public FlaUI.Core.AutomationElements.TextBox NumberTextBox => FindControlWithRetry<FlaUI.Core.AutomationElements.TextBox>("txtNumber");
    public FlaUI.Core.AutomationElements.DataGridView ResultsGrid => FindControlWithRetry<FlaUI.Core.AutomationElements.DataGridView>("dgvResults");

    /// <summary>
    /// 根據標籤名稱尋找右側最近的控制項並獲取其文字內容
    /// </summary>
    public string GetValueByLabel(string labelName)
    {
        // 1. 找到標籤元件 (精確匹配 Name/Text)
        var label = _window.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text).And(cf.ByName(labelName)));
        if (label == null) 
        {
            // 嘗試模糊匹配 (處理空白問題)
            string simplifiedName = labelName.Replace(" ", "");
            label = _window.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text))
                           .FirstOrDefault(c => c.Name.Replace(" ", "") == simplifiedName);
        }

        if (label == null) throw new Exception($"找不到標籤: {labelName}");

        var labelBounds = label.BoundingRectangle;
        var labelCenterY = labelBounds.Top + (labelBounds.Height / 2);
        
        // 2. 獲取所有可能的「數值容器」元件 (排除 Label 自身)
        var candidates = _window.FindAllDescendants(cf => 
            cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit) // TextBox
            .Or(cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text)) // 可能是用 Label 顯示數值
            .Or(cf.ByControlType(FlaUI.Core.Definitions.ControlType.Document))
        );

        // 3. 篩選出位於標籤右側、且垂直中心點接近的元件
        var nearest = candidates
            .Where(c => !string.IsNullOrEmpty(c.AutomationId) && c.AutomationId != label.AutomationId)
            .Select(c => new { Element = c, Bounds = c.BoundingRectangle })
            .Where(c => c.Bounds.Left >= labelBounds.Left + 5 && // 在標籤起點右方
                        c.Bounds.Left < labelBounds.Right + 400 && // 容許一定距離
                        Math.Abs((c.Bounds.Top + c.Bounds.Height / 2) - labelCenterY) < 20) // 垂直中心對齊 (放寬到 20)
            .OrderBy(c => {
                // 優先級 1: 水平距離 (以左側距離標籤左側的位移為準)
                double distance = Math.Abs(c.Bounds.Left - labelBounds.Right);
                // 優先級 2: 如果是在標籤內部或重疊，給予較高優先權 (距離設為 0)
                if (c.Bounds.Left < labelBounds.Right && c.Bounds.Right > labelBounds.Left) distance = 0;
                
                return distance;
            })
            .FirstOrDefault();

        if (nearest == null) throw new Exception($"在標籤 '{labelName}' 右側找不到對應的數值控制項");

        // 4. 讀取值
        var el = nearest.Element;
        if (el.ControlType == FlaUI.Core.Definitions.ControlType.Edit)
        {
            return el.AsTextBox().Text;
        }
        
        // 如果是 Text (Label)，則其 Name 就是顯示的內容
        return el.Name; 
    }

    private T FindControlWithRetry<T>(string automationId) where T : AutomationElement
    {
        var retryResult = FlaUI.Core.Tools.Retry.WhileNull(() => 
        {
            var control = _window.FindFirstDescendant(cf => cf.ByAutomationId(automationId));
            return control ?? _window.FindFirstDescendant(cf => cf.ByName(automationId));
        }, TimeSpan.FromSeconds(10));

        if (!retryResult.Success || retryResult.Result == null)
            throw new Exception($"在 10 秒內找不到控制項: {automationId}");
        
        var element = retryResult.Result;
        if (typeof(T) == typeof(FlaUI.Core.AutomationElements.TextBox)) return (T)(object)element.AsTextBox();
        if (typeof(T) == typeof(FlaUI.Core.AutomationElements.DataGridView)) return (T)(object)element.AsDataGridView();
        
        throw new NotSupportedException($"不支援的控制項型別: {typeof(T).Name}");
    }

    public void VerifyUILayout(string expectedTitle, string[] expectedColumns, string? expectedName = null, string? expectedTestNo = null, string? expectedSeatNo = null)
    {
        // 1. 驗證視窗標題
        if (_window.Title != expectedTitle)
        {
            throw new Exception($"視窗標題不符。預期: '{expectedTitle}', 實際: '{_window.Title}'");
        }

        // 2. 驗證群組框標題
        var groupBox = _window.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Group));
        if (groupBox == null || groupBox.Name != "應檢人資料")
        {
            throw new Exception($"找不到標題為 '應檢人資料' 的群組框，或標題不符。實際: '{groupBox?.Name}'");
        }

        // 3. 驗證應檢人資料內容 (利用相對定位)
        if (expectedName != null) ValidateField("姓名", expectedName);
        if (expectedTestNo != null) ValidateField("術科測試編號", expectedTestNo);
        if (expectedSeatNo != null) ValidateField("座號", expectedSeatNo);
        
        // 驗證日期格式 (西元 YYYY/MM/DD)
        var dateValue = GetValueByLabel("考 試 日 期");
        var todayGregorian = $"{DateTime.Now:yyyy/MM/dd}";
        if (dateValue != todayGregorian)
        {
            throw new Exception($"日期不符。預期: '{todayGregorian}', 實際: '{dateValue}'");
        }

        // 4. 驗證 DataGridView 欄位標題
        var header = ResultsGrid.FindFirstChild(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Header));
        if (header != null)
        {
            var headerItems = header.FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.HeaderItem))
                                    .Select(x => x.Name).Where(name => !string.IsNullOrWhiteSpace(name) && name != "Top Left Header Cell").ToList();
            
            for (int i = 0; i < expectedColumns.Length; i++)
            {
                if (i >= headerItems.Count || headerItems[i] != expectedColumns[i])
                {
                    throw new Exception($"DataGridView 欄位不符。預期第 {i} 欄為 '{expectedColumns[i]}'，實際為 '{(i < headerItems.Count ? headerItems[i] : "null")}'");
                }
            }
        }
    }

    public void VerifyData(Action<string[]> rowValidator)
    {
        var grid = ResultsGrid;
        var rows = grid.Rows;
        
        for (int i = 0; i < rows.Length; i++)
        {
            var row = rows[i];
            // 擷取該列所有儲存格的數值，並處理 (null) 或空值問題
            string[] values = row.Cells.Select(c => {
                string val = c.Value?.ToString() ?? "";
                if (val == "(null)") val = "";
                return val.Trim();
            }).ToArray();
            
            try
            {
                rowValidator(values);
            }
            catch (Exception ex)
            {
                throw new Exception($"DataGridView 第 {i + 1} 列驗證失敗: {ex.Message} (原始資料: {string.Join(", ", values)})");
            }
        }
    }

    private void ValidateField(string label, string expectedValue)
    {
        string actual = GetValueByLabel(label);
        if (actual != expectedValue)
        {
            throw new Exception($"欄位 '{label}' 驗證失敗。預期: '{expectedValue}', 實際: '{actual}'");
        }
    }

    public void HandleOpenFileDialog(string filePath)
    {
        Console.WriteLine($"Starting HandleOpenFileDialog for: {filePath}");
        
        // 1. 等待對話框出現
        var dialog = FlaUI.Core.Tools.Retry.WhileNull(() => 
        {
            return _window.ClassName == "#32770" ? _window : _window.ModalWindows.FirstOrDefault(w => w.ClassName == "#32770");
        }, TimeSpan.FromSeconds(10)).Result;

        if (dialog == null) 
        {
            // 嘗試從桌面找
            dialog = FlaUI.Core.AutomationElements.AutomationElementExtensions.AsWindow(
                _window.Automation.GetDesktop().FindFirstChild(cf => cf.ByClassName("#32770")));
        }

        if (dialog == null) return;

        dialog.Focus();
        Thread.Sleep(1000);

        // 2. 使用 Alt+N 強制聚焦到檔名輸入框
        dialog.Focus();
        FlaUI.Core.Input.Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ALT);
        FlaUI.Core.Input.Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_N);
        FlaUI.Core.Input.Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.ALT);
        Thread.Sleep(500);

        // 3. 獲取當前聚焦的控制項 (應該是輸入框)
        var edit = dialog.Automation.FocusedElement()?.AsTextBox();
        
        // 如果沒抓到，嘗試用 ID 找 (1148 是標準檔名編輯框 ID)
        if (edit == null || edit.ClassName != "Edit")
        {
            edit = dialog.FindFirstDescendant(cf => cf.ByAutomationId("1148"))?.AsTextBox();
        }

        if (edit != null)
        {
            edit.Focus();
            // 嘗試直接設定值
            try 
            {
                if (edit.Patterns.Value.IsSupported)
                {
                    edit.Patterns.Value.Pattern.SetValue(filePath);
                }
                else
                {
                    edit.Text = filePath;
                }
            }
            catch
            {
                // 如果設定失敗，使用模擬打字，但先清空
                edit.Enter(filePath);
            }
            Thread.Sleep(1000);

            // 4. 點擊「開啟」
            var openBtn = dialog.FindFirstDescendant(cf => cf.ByAutomationId("1").And(cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button)))?.AsButton()
                       ?? dialog.FindFirstDescendant(cf => cf.ByName("開啟"))?.AsButton();

            if (openBtn != null)
            {
                openBtn.Invoke();
            }
            else
            {
                FlaUI.Core.Input.Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            }
        }
        else
        {
            // 最終手段：直接打字
            FlaUI.Core.Input.Keyboard.Type(filePath);
            Thread.Sleep(500);
            FlaUI.Core.Input.Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
        }
        
        Thread.Sleep(3000); 
    }
}
