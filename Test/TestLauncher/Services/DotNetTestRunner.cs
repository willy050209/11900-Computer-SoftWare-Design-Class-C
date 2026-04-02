// filepath: Services/DotNetTestRunner.cs
using System.Diagnostics;
using TestLauncher.Models;

namespace TestLauncher.Services;

public class DotNetTestRunner : ITestRunnerService
{
    public event Action<string>? OutputReceived;

    private readonly string _solutionRoot;
    private readonly string _toolsDir;
    private readonly bool _isReleaseMode;

    public DotNetTestRunner()
    {
        string baseDir = AppContext.BaseDirectory;
        
        // 偵測是否為發佈後的 "tools" 資料夾模式
        _toolsDir = Path.Combine(baseDir, "tools");
        _isReleaseMode = Directory.Exists(_toolsDir);

        if (_isReleaseMode)
        {
            _solutionRoot = baseDir; // 發佈模式下以目前目錄為準
        }
        else
        {
            // 開發模式下的路徑追蹤 (適應不同的執行路徑)
            string? current = baseDir;
            while (current != null && !Directory.Exists(Path.Combine(current, "C#")))
            {
                current = Path.GetDirectoryName(current);
            }
            _solutionRoot = current ?? baseDir;
        }
    }

    public async Task RunTask1Async(Task1Config config)
    {
        if (_isReleaseMode && File.Exists(Path.Combine(_toolsDir, "Task1Tester.exe")))
        {
            string exePath = Path.Combine(_toolsDir, "Task1Tester.exe");
            string args = $"\"{config.CodePath}\" \"{config.UserPdfPath}\" \"{config.AnsPdfPath}\" \"{config.Name}\" \"{config.TestNo}\" \"{config.SeatNo}\" \"{config.LoopType}\"";
            await ExecuteCommandAsync(exePath, args);
        }
        else
        {
            string projectPath = Path.Combine(_solutionRoot, "Test", "Task1Tester", "Task1Tester", "Task1Tester.csproj");
            string args = $"run --project \"{projectPath}\" -- \"{config.CodePath}\" \"{config.UserPdfPath}\" \"{config.AnsPdfPath}\" \"{config.Name}\" \"{config.TestNo}\" \"{config.SeatNo}\" \"{config.LoopType}\"";
            await ExecuteCommandAsync("dotnet", args);
        }
    }

    public async Task RunTask2Async(Task2Config config, string name, string testNo, string seatNo)
    {
        string filter = config.TaskId switch 
        {
            "task06" => "Task06UITest",
            "task07" => "Task07UITest",
            "task08" => "Task08UITest",
            _ => throw new ArgumentException("Unknown Task ID")
        };

        // 使用環境變數傳遞參數，這在發佈模式下最穩定
        var envVars = new Dictionary<string, string>
        {
            { "TEST_EXE_PATH", Path.GetFullPath(config.ExePath) },
            { "TEST_DATA_PATH", Path.GetFullPath(config.TestDataPath) },
            { "TEST_CANDIDATE_NAME", name },
            { "TEST_CANDIDATE_NO", testNo },
            { "TEST_CANDIDATE_SEAT", seatNo },
            { "TEST_TASK_ID", config.TaskId }
        };

        if (_isReleaseMode && File.Exists(Path.Combine(_toolsDir, "WinFormUITester.dll")))
        {
            string dllPath = Path.Combine(_toolsDir, "WinFormUITester.dll");
            string args = $"test \"{dllPath}\" --filter \"{filter}\"";
            await ExecuteCommandAsync("dotnet", args, envVars);
        }
        else
        {
            string projectPath = Path.Combine(_solutionRoot, "Test", "WinFormUITester", "WinFormUITester.csproj");
            string args = $"test \"{projectPath}\" --filter \"{filter}\"";
            await ExecuteCommandAsync("dotnet", args, envVars);
        }
    }

    private async Task ExecuteCommandAsync(string fileName, string arguments, Dictionary<string, string>? envVars = null)
    {
        OutputReceived?.Invoke($"> {fileName} {arguments}\n");

        ProcessStartInfo psi = new()
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = System.Text.Encoding.UTF8,
            StandardErrorEncoding = System.Text.Encoding.UTF8,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = _solutionRoot
        };

        if (envVars != null)
        {
            foreach (var kvp in envVars)
            {
                psi.EnvironmentVariables[kvp.Key] = kvp.Value;
            }
        }

        using Process process = new() { StartInfo = psi };
        
        process.OutputDataReceived += (s, e) => { if (e.Data != null) OutputReceived?.Invoke(e.Data + "\n"); };
        process.ErrorDataReceived += (s, e) => { if (e.Data != null) OutputReceived?.Invoke("ERROR: " + e.Data + "\n"); };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();
        OutputReceived?.Invoke($"\n[Process Exited with Code: {process.ExitCode}]\n------------------------------------------\n");
    }
}
