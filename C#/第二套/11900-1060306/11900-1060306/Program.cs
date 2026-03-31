using Microsoft.Extensions.DependencyInjection;
using IdCardChecker.Views;
using IdCardChecker.Presenters;
using IdCardChecker.Services;

namespace IdCardChecker;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();

        // Register Services
        services.AddSingleton<IIdCardValidatorService, IdCardValidatorService>();
        services.AddSingleton<IDataService, DataService>();

        // Register Views
        services.AddSingleton<IMainView, MainForm>();

        // Register Presenters
        services.AddSingleton<MainPresenter>();

        using var serviceProvider = services.BuildServiceProvider();

        // Resolve Presenter to wire up events
        var presenter = serviceProvider.GetRequiredService<MainPresenter>();
        var mainForm = (Form)serviceProvider.GetRequiredService<IMainView>();

        Application.Run(mainForm);
    }
}
