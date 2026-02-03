using Microsoft.Extensions.DependencyInjection;
using ConsoleTools;
using ConsoleTools.Modulos;
using MotorDArranque;
using Spectre.Console;
using WGetNET;
using ConsoleTools.Utils;

// DI SETUP
var services = new ServiceCollection();
services.AddSingleton<WinGet>();
services.AddSingleton<WinGetPackageManager>();
services.AddSingleton<WingetStartup>();
services.AddSingleton<Modulos>();
services.AddSingleton<Menu>();

var provider = services.BuildServiceProvider();
var modulos = provider.GetRequiredService<Modulos>();

// VERIFICA ESTADO WINGET 
var checks = provider.GetRequiredService<WingetStartup>();
checks.RunStartupVerif();

// CRIA PASTAS
Directory.CreateDirectory(AppPaths.AppDirInUserTemp);

var menu = provider.GetRequiredService<Menu>();
while (true)
{
    try
    {
        AnsiConsole.Clear();
        await menu.RunMenuAsync();
    }
    catch(Exception ex)
    {
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
        return;
    }
}