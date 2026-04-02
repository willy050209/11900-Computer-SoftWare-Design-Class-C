using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

namespace WinFormUITester;

public class TaskConfig
{
    public string ExePath { get; set; } = "";
    public string TestDataPath { get; set; } = "";
    public string ExpectedTitle { get; set; } = "";
    public string[] ExpectedColumns { get; set; } = Array.Empty<string>();
}

public class TestSettings
{
    private static readonly JsonDocument _config;
    private static readonly string _solutionRoot;

    static TestSettings()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        string baseDir = AppContext.BaseDirectory;
        _solutionRoot = FindSolutionRoot(baseDir);
        
        string configPath = Path.Combine(baseDir, "testsettings.json");
        if (!File.Exists(configPath))
        {
            // 備援搜尋 (開發環境可能在專案目錄下)
            configPath = Path.Combine(_solutionRoot, "Test", "WinFormUITester", "testsettings.json");
        }

        if (File.Exists(configPath))
        {
            _config = JsonDocument.Parse(File.ReadAllText(configPath));
        }
        else
        {
            throw new FileNotFoundException($"Cannot find testsettings.json at {configPath}");
        }
    }

    private static string FindSolutionRoot(string startDir)
    {
        string? current = startDir;
        while (current != null && !Directory.Exists(Path.Combine(current, "C#")))
        {
            current = Path.GetDirectoryName(current);
        }
        return current ?? startDir;
    }

    public static TaskConfig GetTaskConfig(string taskId)
    {
        var taskSection = _config.RootElement.GetProperty("Tasks").GetProperty(taskId);
        var config = new TaskConfig
        {
            ExePath = Environment.GetEnvironmentVariable("TEST_EXE_PATH") 
                    ?? ResolvePath(taskSection.GetProperty("ExePath").GetString(), ""),
            TestDataPath = Environment.GetEnvironmentVariable("TEST_DATA_PATH")
                    ?? ResolvePath(taskSection.GetProperty("TestDataPath").GetString(), ""),
            ExpectedTitle = taskSection.GetProperty("ExpectedTitle").GetString() ?? "",
            ExpectedColumns = taskSection.GetProperty("ExpectedColumns").EnumerateArray().Select(x => x.GetString() ?? "").ToArray()
        };
        return config;
    }

    public static string ResolvePath(string? path, string defaultPath)
    {
        if (string.IsNullOrWhiteSpace(path)) return defaultPath;
        if (Path.IsPathRooted(path)) return path;
        return Path.GetFullPath(Path.Combine(_solutionRoot, path));
    }

    public static string GetCandidateName() => Environment.GetEnvironmentVariable("TEST_CANDIDATE_NAME") ?? _config.RootElement.GetProperty("DefaultCandidate").GetProperty("Name").GetString() ?? "陳宇威";
    public static string GetCandidateTestNo() => Environment.GetEnvironmentVariable("TEST_CANDIDATE_NO") ?? _config.RootElement.GetProperty("DefaultCandidate").GetProperty("TestNo").GetString() ?? "112590005";
    public static string GetCandidateSeatNo() => Environment.GetEnvironmentVariable("TEST_CANDIDATE_SEAT") ?? _config.RootElement.GetProperty("DefaultCandidate").GetProperty("SeatNo").GetString() ?? "005";

    public static void CaptureScreenshot(FlaUI.Core.AutomationElements.AutomationElement element, string testName)
    {
        try
        {
            string screenshotDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
            if (!Directory.Exists(screenshotDir)) Directory.CreateDirectory(screenshotDir);

            string fileName = $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            string filePath = Path.Combine(screenshotDir, fileName);

            var image = FlaUI.Core.Capturing.Capture.Element(element);
            image.ToFile(filePath);
            Console.WriteLine($"[Screenshot Captured]: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to capture screenshot: {ex.Message}");
        }
    }
}
