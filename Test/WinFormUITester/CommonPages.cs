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

    public void HandleOpenFileDialog(string filePath)
    {
        // 等待對話框出現，最多 8 秒
        var retryResult = FlaUI.Core.Tools.Retry.WhileNull(() => 
            _window.ModalWindows.FirstOrDefault(w => w.ClassName == "#32770" || w.Name.Contains("Open") || w.Name.Contains("開啟")),
            TimeSpan.FromSeconds(8));

        if (retryResult.Success && retryResult.Result != null)
        {
            var dialog = retryResult.Result;
            // 找到輸入框
            var edit = dialog.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit)).AsTextBox();
            edit.Enter(filePath);
            Thread.Sleep(500);

            // 優先尋找並點擊「開啟」或 "Open" 按鈕，這比按 Enter 穩定
            var openBtn = dialog.FindFirstDescendant(cf => cf.ByName("開啟"))?.AsButton() 
                       ?? dialog.FindFirstDescendant(cf => cf.ByName("Open"))?.AsButton();
            
            if (openBtn != null)
            {
                openBtn.Invoke();
            }
            else
            {
                FlaUI.Core.Input.Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            }
            
            FlaUI.Core.Input.Wait.UntilInputIsProcessed();
            Thread.Sleep(1000); // 給予一點緩衝時間讓對話框完全消失
        }
    }
}
