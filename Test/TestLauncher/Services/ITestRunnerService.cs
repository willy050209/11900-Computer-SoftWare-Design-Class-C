// filepath: Services/ITestRunnerService.cs
namespace TestLauncher.Services;

public interface ITestRunnerService
{
    event Action<string>? OutputReceived;
    Task RunTask1Async(TestLauncher.Models.Task1Config config);
    Task RunTask2Async(TestLauncher.Models.Task2Config config);
}
