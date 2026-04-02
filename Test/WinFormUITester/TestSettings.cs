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
        for (int i = 0; i < _args.Length; i++)
        {
            if (_args[i].Equals(flag, StringComparison.OrdinalIgnoreCase) && i + 1 < _args.Length)
            {
                // 處理包含空格的路徑：如果後面的參數不以 "--" 開頭，則將它們拼回去
                var valueParts = new System.Collections.Generic.List<string>();
                int j = i + 1;
                while (j < _args.Length && !_args[j].StartsWith("--"))
                {
                    valueParts.Add(_args[j]);
                    j++;
                }
                return string.Join(" ", valueParts).Trim('\"');
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
        // 優先讀取通用環境變數，再找特定 Task 的參數
        string? path = Environment.GetEnvironmentVariable("TEST_EXE_PATH") 
                    ?? GetArgument($"--{taskId}-exe") 
                    ?? GetArgument("--exe");
        return ResolvePath(path, defaultPath);
    }

    public static string GetTestDataPath(string taskId, string defaultPath)
    {
        string? path = Environment.GetEnvironmentVariable("TEST_DATA_PATH")
                    ?? GetArgument($"--{taskId}-data") 
                    ?? GetArgument("--data");
        return ResolvePath(path, defaultPath);
    }

    public static string GetCandidateName() => Environment.GetEnvironmentVariable("TEST_CANDIDATE_NAME") ?? GetArgument("--name") ?? "陳宇威";
    public static string GetCandidateTestNo() => Environment.GetEnvironmentVariable("TEST_CANDIDATE_NO") ?? GetArgument("--test-no") ?? "112590005";
    public static string GetCandidateSeatNo() => Environment.GetEnvironmentVariable("TEST_CANDIDATE_SEAT") ?? GetArgument("--seat-no") ?? "005";
}
