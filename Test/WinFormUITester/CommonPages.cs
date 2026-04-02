using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;

namespace WinFormUITester;

/// <summary>
/// 代表標準開啟檔案對話框的組件
/// </summary>
public class OpenFileDialogComponent
{
    private readonly Window _window;

    public OpenFileDialogComponent(Window window)
    {
        _window = window;
    }

    public void OpenFile(string filePath)
    {
        _window.Focus();
        var edit = FlaUI.Core.Tools.Retry.WhileNull(() => {
            FlaUI.Core.Input.Keyboard.Press(VirtualKeyShort.ALT);
            FlaUI.Core.Input.Keyboard.Press(VirtualKeyShort.KEY_N);
            FlaUI.Core.Input.Keyboard.Release(VirtualKeyShort.ALT);
            var focused = _window.Automation.FocusedElement()?.AsTextBox();
            if (focused != null && focused.ClassName == "Edit") return focused;
            return _window.FindFirstDescendant(cf => cf.ByAutomationId("1148"))?.AsTextBox();
        }, TimeSpan.FromSeconds(5)).Result;

        if (edit == null) throw new Exception("找不到檔名輸入框");

        edit.Focus();
        if (edit.Patterns.Value.IsSupported)
            edit.Patterns.Value.Pattern.SetValue(filePath);
        else
            edit.Enter(filePath);

        var openBtn = _window.FindFirstDescendant(cf => cf.ByAutomationId("1").And(cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button)))?.AsButton()
                   ?? _window.FindFirstDescendant(cf => cf.ByName("開啟"))?.AsButton();

        if (openBtn != null) openBtn.Invoke();
        else FlaUI.Core.Input.Keyboard.Press(VirtualKeyShort.ENTER);

        // 使用較穩定的等待對話框消失邏輯
        try {
            FlaUI.Core.Tools.Retry.WhileTrue(() => _window != null && !_window.IsOffscreen, TimeSpan.FromSeconds(5));
        } catch { /* 忽略關閉時可能的屬性異常 */ }
    }
}

/// <summary>
/// 代表應檢人資料區域的組件
/// </summary>
public class CandidateInfoComponent
{
    private readonly AutomationElement _container;

    public CandidateInfoComponent(AutomationElement container)
    {
        _container = container;
    }

    public string GetValueByLabel(string labelName)
    {
        var label = _container.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text).And(cf.ByName(labelName)));
        if (label == null)
        {
            string simplified = labelName.Replace(" ", "");
            label = _container.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text))
                              .FirstOrDefault(c => c.Name.Replace(" ", "") == simplified);
        }

        if (label == null) throw new Exception($"找不到標籤: {labelName}");

        var labelBounds = label.BoundingRectangle;
        var candidates = _container.FindAllDescendants(cf => 
            cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit)
            .Or(cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text))
            .Or(cf.ByControlType(FlaUI.Core.Definitions.ControlType.Document))
        );

        var nearest = candidates
            .Where(c => !string.IsNullOrEmpty(c.AutomationId) && c.AutomationId != label.AutomationId)
            .OrderBy(c => {
                var b = c.BoundingRectangle;
                return Math.Abs(b.Left - labelBounds.Right) + Math.Abs((b.Top + b.Height/2) - (labelBounds.Top + labelBounds.Height/2));
            }).FirstOrDefault();

        if (nearest == null) throw new Exception($"找不到標籤 '{labelName}' 的對應值");
        return nearest.ControlType == FlaUI.Core.Definitions.ControlType.Edit ? nearest.AsTextBox().Text : nearest.Name;
    }

    public void VerifyInfo(string expectedName, string expectedTestNo, string expectedSeatNo)
    {
        if (GetValueByLabel("姓名") != expectedName) throw new Exception("姓名不符");
        if (GetValueByLabel("術科測試編號") != expectedTestNo) throw new Exception("編號不符");
        if (GetValueByLabel("座號") != expectedSeatNo) throw new Exception("座號不符");
    }
}

/// <summary>
/// 代表第二套題目的主視窗 Page Object
/// </summary>
public class TaskMainPage
{
    private readonly Window _window;
    public CandidateInfoComponent CandidateInfo { get; }
    public FlaUI.Core.AutomationElements.DataGridView ResultsGrid => _window.FindFirstDescendant(cf => cf.ByAutomationId("dgvResults")).AsDataGridView();

    public TaskMainPage(Window window)
    {
        _window = window;
        CandidateInfo = new CandidateInfoComponent(window);
    }

    public void VerifyLayout(TaskConfig config)
    {
        if (_window.Title != config.ExpectedTitle) throw new Exception($"標題預期 '{config.ExpectedTitle}'，實際 '{_window.Title}'");
        
        var headerItems = ResultsGrid.Header.FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.HeaderItem))
                                    .Select(x => x.Name.Trim()).ToList();

        if (headerItems.Count < config.ExpectedColumns.Length)
            throw new Exception($"欄位數量不足，偵測到 {headerItems.Count} 個欄位: {string.Join(", ", headerItems)}");

        for (int i = 0; i < config.ExpectedColumns.Length; i++)
        {
            if (headerItems[i] != config.ExpectedColumns[i])
                throw new Exception($"第 {i+1} 欄預期 '{config.ExpectedColumns[i]}'，實際 '{headerItems[i]}'");
        }
    }
}
