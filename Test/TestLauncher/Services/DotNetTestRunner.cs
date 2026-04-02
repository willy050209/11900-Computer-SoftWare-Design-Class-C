// filepath: Services/DotNetTestRunner.cs
using System.Diagnostics;
using TestLauncher.Models;

namespace TestLauncher.Services;

public class DotNetTestRunner : ITestRunnerService
{
    public event Action<string>? OutputReceived;

    private readonly string _solutionRoot;

    public DotNetTestRunner()
    {
        // 假設執行檔在 Test/TestLauncher/bin/Debug/net10.0-windows/
        // Root 是向上 5 層
        string baseDir = AppContext.BaseDirectory;
        _solutionRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", ".."));
        // 如果沒抓到 C# 資料夾，再往上一層 (適應不同的執行環境)
        if (!Directory.Exists(Path.Combine(_solutionRoot, "C#")))
        {
             _solutionRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "..", ".."));
        }
    }

    public async Task RunTask1Async(Task1Config config)
    {
        string projectPath = Path.Combine(_solutionRoot, "Test", "Task1Tester", "Task1Tester", "Task1Tester.csproj");
        string args = $"run --project \"{projectPath}\" -- \"{config.CodePath}\" \"{config.UserPdfPath}\" \"{config.AnsPdfPath}\" \"{config.Name}\" \"{config.TestNo}\" \"{config.SeatNo}\" \"{config.LoopType}\"";
        
        await ExecuteDotNetAsync(args);
    }

    public async Task RunTask2Async(Task2Config config, string name, string testNo, string seatNo)
    {
        string projectPath = Path.Combine(_solutionRoot, "Test", "WinFormUITester", "WinFormUITester.csproj");
        // 使用 --filter 來指定跑哪一個 Task 的測試
        // 並透過 -- 傳遞參數給測試程式
        string filter = config.TaskId switch 
        {
            "task06" => "Task06UITest",
            "task07" => "Task07UITest",
            "task08" => "Task08UITest",
            _ => throw new ArgumentException("Unknown Task ID")
        };

        string args = $"test \"{projectPath}\" --filter \"{filter}\" -- --{config.TaskId}-exe \"{config.ExePath}\" --{config.TaskId}-data \"{config.TestDataPath}\" --name \"{name}\" --test-no \"{testNo}\" --seat-no \"{seatNo}\"";
        
        await ExecuteDotNetAsync(args);
    }

    private async Task ExecuteDotNetAsync(string arguments)
    {
        OutputReceived?.Invoke($"> dotnet {arguments}\n");

        ProcessStartInfo psi = new()
        {
            FileName = "dotnet",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = System.Text.Encoding.UTF8,
            StandardErrorEncoding = System.Text.Encoding.UTF8,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = _solutionRoot
        };

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
