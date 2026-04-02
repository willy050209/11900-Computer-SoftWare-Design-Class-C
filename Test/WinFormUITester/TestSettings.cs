using System;
using System.IO;
using System.Linq;

namespace WinFormUITester;

public static class TestSettings
{
    static TestSettings()
    {
        // 確保 Console 輸出使用 UTF-8，以正確顯示中文與特殊符號
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    private static readonly string[] _args = Environment.GetCommandLineArgs();

    public static string? GetArgument(string flag)
    {
        for (int i = 0; i < _args.Length - 1; i++)
        {
            if (_args[i].Equals(flag, StringComparison.OrdinalIgnoreCase))
            {
                return _args[i + 1];
            }
        }
        return null;
    }

    public static string ResolvePath(string? path, string defaultPath)
    {
        if (string.IsNullOrWhiteSpace(path)) return defaultPath;
        return Path.IsPathRooted(path) ? path : Path.GetFullPath(path);
    }

    public static string GetExePath(string taskId, string defaultPath)
    {
        // 先找特定 Task 的參數，例如 --task06-exe
        string? path = GetArgument($"--{taskId}-exe") ?? GetArgument("--exe");
        return ResolvePath(path, defaultPath);
    }

    public static string GetTestDataPath(string taskId, string defaultPath)
    {
        // 先找特定 Task 的參數，例如 --task06-data
        string? path = GetArgument($"--{taskId}-data") ?? GetArgument("--data");
        return ResolvePath(path, defaultPath);
    }

    public static string GetCandidateName() => GetArgument("--name") ?? "陳宇威";
    public static string GetCandidateTestNo() => GetArgument("--test-no") ?? "112590005";
    public static string GetCandidateSeatNo() => GetArgument("--seat-no") ?? "005";
}
