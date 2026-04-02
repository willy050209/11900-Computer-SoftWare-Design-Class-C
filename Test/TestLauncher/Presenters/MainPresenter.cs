// filepath: Presenters/MainPresenter.cs
using TestLauncher.Models;
using TestLauncher.Services;
using TestLauncher.Views;

namespace TestLauncher.Presenters;

public class MainPresenter
{
    private readonly IMainView _view;
    private readonly ITestRunnerService _runner;
    private readonly LocalizationService _loc;

    public MainPresenter(IMainView view, ITestRunnerService runner, LocalizationService loc)
    {
        _view = view;
        _runner = runner;
        _loc = loc;

        _runner.OutputReceived += (text) => AppendLogWithColor(text);
        _loc.LanguageChanged += () => _view.UpdateLocalizedText(_loc.GetString);
        
        // 初始化時設定一次語言
        _view.UpdateLocalizedText(_loc.GetString);
    }

    private void AppendLogWithColor(string text)
    {
        var color = System.Drawing.Color.White;
        string upperText = text.ToUpper();

        if (text.StartsWith(">"))
        {
            color = System.Drawing.Color.Cyan;
        }
        else if (upperText.Contains("PASSED") || upperText.Contains("✓") || upperText.Contains("SUCCEEDED"))
        {
            color = System.Drawing.Color.LimeGreen;
        }
        else if (upperText.Contains("FAILED") || upperText.Contains("✗") || upperText.Contains("ERROR") || upperText.Contains("EXCEPTION"))
        {
            color = System.Drawing.Color.Red;
        }
        else if (upperText.Contains("WARNING"))
        {
            color = System.Drawing.Color.Yellow;
        }
        else if (text.StartsWith("[") && text.Contains("]"))
        {
            color = System.Drawing.Color.PowderBlue;
        }

        _view.AppendLog(text, color);
    }

    public void ChangeLanguage(Language lang)
    {
        _loc.CurrentLanguage = lang;
    }

    public async Task RunTask1Async()
    {
        _view.SetBusy(true);
        _view.ClearLog();
        _view.AppendLog("Starting Task 1 Test...\n");

        var config = new Task1Config(
            _view.Task1CodePath,
            _view.Task1UserPdfPath,
            _view.Task1AnsPdfPath,
            _view.CandidateName,
            _view.CandidateTestNo,
            _view.CandidateSeatNo,
            _view.LoopType
        );

        try
        {
            await _runner.RunTask1Async(config);
        }
        catch (Exception ex)
        {
            _view.AppendLog($"ERROR: {ex.Message}\n");
        }
        finally
        {
            _view.SetBusy(false);
        }
    }

    public async Task RunTask2Async(string taskId)
    {
        _view.SetBusy(true);
        _view.ClearLog();
        _view.AppendLog($"Starting {taskId.ToUpper()} UI Test...\n");

        Task2Config config = taskId switch
        {
            "task06" => new Task2Config("task06", _view.Task06ExePath, _view.Task06DataPath),
            "task07" => new Task2Config("task07", _view.Task07ExePath, _view.Task07DataPath),
            "task08" => new Task2Config("task08", _view.Task08ExePath, _view.Task08DataPath),
            _ => throw new ArgumentException("Unknown Task ID")
        };

        try
        {
            await _runner.RunTask2Async(config, _view.CandidateName, _view.CandidateTestNo, _view.CandidateSeatNo);
        }
        catch (Exception ex)
        {
            _view.AppendLog($"ERROR: {ex.Message}\n");
        }
        finally
        {
            _view.SetBusy(false);
        }
    }
}
