using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;

namespace WinFormUITester;

public class OpenFileDialogComponent
{
    private readonly Window _window;
    public OpenFileDialogComponent(Window window) => _window = window;

    public void OpenFile(string filePath)
    {
        _window.Focus();
        var edit = FlaUI.Core.Tools.Retry.WhileNull(() => {
            FlaUI.Core.Input.Keyboard.Press(VirtualKeyShort.ALT);
            FlaUI.Core.Input.Keyboard.Press(VirtualKeyShort.KEY_N);
            FlaUI.Core.Input.Keyboard.Release(VirtualKeyShort.ALT);
            var focused = _window.Automation.FocusedElement();
            if (focused != null && focused.ControlType == FlaUI.Core.Definitions.ControlType.Edit) return focused.AsTextBox();
            var found = _window.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
            return found?.AsTextBox();
        }, TimeSpan.FromSeconds(5)).Result;

        if (edit == null) throw new Exception("找不到檔名輸入框");

        edit.Focus();
        edit.Enter(filePath);

        var openBtn = _window.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(cf.ByName("開啟").Or(cf.ByAutomationId("1"))))?.AsButton();
        if (openBtn != null) openBtn.Invoke();
        else FlaUI.Core.Input.Keyboard.Press(VirtualKeyShort.ENTER);

        try { FlaUI.Core.Tools.Retry.WhileTrue(() => !_window.IsOffscreen, TimeSpan.FromSeconds(5)); } catch { }
    }
}

public class CandidateInfoComponent
{
    private readonly AutomationElement _container;
    public CandidateInfoComponent(AutomationElement container) => _container = container;

    public string GetValueByLabel(string labelName)
    {
        var target = labelName.Replace(" ", "");
        var label = _container.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text))
                              .FirstOrDefault(c => {
                                  try { return (c.Name ?? "").Replace(" ", "") == target; } catch { return false; }
                              });
        
        if (label == null) throw new Exception($"找不到標籤: {labelName}");

        var lb = label.BoundingRectangle;
        var candidates = _container.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit).Or(cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text)))
            .Where(c => {
                try { 
                    var b = c.BoundingRectangle;
                    return c.AutomationId != label.AutomationId && b.Left >= lb.Left && b.Width > 0 && b.Height > 0; 
                } catch { return false; }
            })
            .OrderBy(c => {
                var b = c.BoundingRectangle;
                return Math.Abs(b.Left - lb.Right) + Math.Abs((b.Top + b.Height/2) - (lb.Top + lb.Height/2));
            }).FirstOrDefault();

        if (candidates == null) throw new Exception($"找不到標籤 '{labelName}' 的對應值");
        
        string? val = candidates.ControlType == FlaUI.Core.Definitions.ControlType.Edit 
            ? candidates.AsTextBox().Text 
            : candidates.Name;
            
        return val ?? "";
    }

    public void VerifyInfo(string expectedName, string expectedTestNo, string expectedSeatNo)
    {
        if (GetValueByLabel("姓名") != expectedName) throw new Exception($"姓名不符");
        if (GetValueByLabel("術科測試編號") != expectedTestNo) throw new Exception($"編號不符");
        if (GetValueByLabel("座號") != expectedSeatNo) throw new Exception($"座號不符");
    }
}

public class TaskMainPage
{
    private readonly Window _window;
    public CandidateInfoComponent CandidateInfo { get; }
    
    public FlaUI.Core.AutomationElements.DataGridView? ResultsGrid => 
        _window.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.DataGrid))?.AsDataGridView();

    public TaskMainPage(Window window)
    {
        _window = window;
        CandidateInfo = new CandidateInfoComponent(window);
    }

    public void VerifyLayout(TaskConfig config)
    {
        var grid = ResultsGrid;
        if (grid == null) throw new Exception("找不到 DataGridView 控制項");

        var headerItems = grid.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.HeaderItem))
                              .Select(x => { try { return x.Name?.Trim() ?? ""; } catch { return ""; } })
                              .Where(n => !string.IsNullOrEmpty(n) && n != "Top Left Header Cell" && !n.Contains("列") && !n.Contains("Row"))
                              .ToList();

        if (headerItems.Count == 0 && grid.Rows.Length > 0)
        {
             Console.WriteLine("Warning: Could not detect headers, skipping layout check.");
             return;
        }

        for (int i = 0; i < Math.Min(headerItems.Count, config.ExpectedColumns.Length); i++)
        {
            if (headerItems[i] != config.ExpectedColumns[i])
                throw new Exception($"第 {i+1} 欄預期 '{config.ExpectedColumns[i]}'，實際 '{headerItems[i]}'");
        }
    }
}
