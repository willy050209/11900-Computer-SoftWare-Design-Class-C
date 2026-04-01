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

    public void VerifyUILayout(string expectedTitle, string[] expectedColumns)
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

        // 3. 驗證應檢人資料標籤
        var labels = _window.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text))
                            .Select(x => x.Name).ToList();
        
        string[] requiredLabels = { "姓名", "術科測試編號", "座號", "考 試 日 期" };
        foreach (var reqLabel in requiredLabels)
        {
            if (!labels.Contains(reqLabel))
            {
                throw new Exception($"找不到必要的標籤: '{reqLabel}'");
            }
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
