using Microsoft.Extensions.DependencyInjection;
using FractionArithmetic.Views;
using FractionArithmetic.Presenters;
using FractionArithmetic.Services;

namespace FractionArithmetic;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();

        // Register Services
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<IFractionService, FractionService>();

        // Register Views
        services.AddSingleton<IMainView, MainForm>();

        // Register Presenters
        services.AddSingleton<MainPresenter>();

        using var serviceProvider = services.BuildServiceProvider();

        // Resolve Presenter
        var presenter = serviceProvider.GetRequiredService<MainPresenter>();
        var mainForm = (Form)serviceProvider.GetRequiredService<IMainView>();

        Application.Run(mainForm);
    }
}
