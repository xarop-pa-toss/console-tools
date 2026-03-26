using ConsoleTools.Framework;
using ConsoleTools.Modulos;
using ConsoleTools.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using WGetNET;

namespace MotorDArranque;

public sealed class MotorDArranqueModule(
    WingetStartup startup,
    Modulos modulos,
    ILoggerFactory loggerFactory,
    ILogger<MotorDArranqueModule> logger) : IConsoleToolModule
{
    private readonly WingetStartup _startup = startup;
    private readonly Modulos _modulos = modulos;
    private readonly ILogger<MotorDArranqueModule> _logger = logger;

    public string Id => "motor-darranque";
    public string DisplayName => "MotorDArranque";
    public string Description => "Winget wrapper";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(AppPaths.AppDirInUserTemp);
        ConsoleUtils.Logger = loggerFactory.CreateLogger("ConsoleUtils");

        _startup.RunStartupVerif();

        var keepRunning = true;
        while (keepRunning && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                AnsiConsole.Clear();
                keepRunning = await _modulos.MainMenuAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in MotorDArranque module");
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                ConsoleToolsShell.WaitForContinue();
            }
        }
    }
}

public static class MotorDArranqueModuleServiceCollectionExtensions
{
    public static IServiceCollection AddMotorDArranqueModule(this IServiceCollection services)
    {
        services.AddSingleton<WinGet>();
        services.AddSingleton<WinGetPackageManager>();
        services.AddSingleton<WingetStartup>();
        services.AddSingleton<Modulos>();
        services.AddSingleton<MenuHandler>();

        services.AddSingleton<IConsoleToolModule, MotorDArranqueModule>();
        return services;
    }
}
