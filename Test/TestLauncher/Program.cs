// filepath: Program.cs
using Microsoft.Extensions.DependencyInjection;
using TestLauncher.Presenters;
using TestLauncher.Services;
using TestLauncher.Views;

namespace TestLauncher;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();
        ConfigureServices(services);

        using var serviceProvider = services.BuildServiceProvider();
        var mainForm = serviceProvider.GetRequiredService<MainForm>();
        Application.Run(mainForm);
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        // Services
        services.AddSingleton<ITestRunnerService, DotNetTestRunner>();
        services.AddSingleton<LocalizationService>();

        // Views
        services.AddTransient<MainForm>();

        // Presenters (Factory pattern to inject IMainView later)
        services.AddTransient<Func<IMainView, MainPresenter>>(sp => 
            view => new MainPresenter(view, sp.GetRequiredService<ITestRunnerService>(), sp.GetRequiredService<LocalizationService>()));
    }
}
