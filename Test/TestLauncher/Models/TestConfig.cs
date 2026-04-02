// filepath: Models/TestConfig.cs
namespace TestLauncher.Models;

public record Task1Config(
    string CodePath,
    string UserPdfPath,
    string AnsPdfPath,
    string Name,
    string TestNo,
    string SeatNo,
    string LoopType
);

public record Task2Config(
    string TaskId,
    string ExePath,
    string TestDataPath
);
