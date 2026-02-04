using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using ConsoleTools.Modulos;
using MotorDArranque;
using Spectre.Console;
using WGetNET;
using ConsoleTools.Utils;


// DI SETUP
var services = new ServiceCollection();

services.AddLogging(s =>
{
    s.AddSimpleConsole();
    s.SetMinimumLevel(LogLevel.Information);
});
services.AddSingleton<WinGet>();
services.AddSingleton<WinGetPackageManager>();
services.AddSingleton<WingetStartup>();
services.AddSingleton<Modulos>();
services.AddSingleton<MenuHandler>();

var provider = services.BuildServiceProvider();
var modulos = provider.GetRequiredService<Modulos>();
var logger = provider.GetRequiredService<ILogger<Program>>();

// VERIFICA ESTADO WINGET 
var checks = provider.GetRequiredService<WingetStartup>();
checks.RunStartupVerif();

// CRIA PASTAS
Directory.CreateDirectory(AppPaths.AppDirInUserTemp);

//Menu loop
// ├─ try
// │   └─ Menu.RunAsync
// │       └─ Command.Run → Result (expected)
// │       └─ throws Exception (unexpected)
// ├─ catch UserFriendlyException → warn + message
// └─ catch Exception → critical + exit

// TODO: Criar UserFriendlyException

while (true)
{
    try
    {
        AnsiConsole.Clear();
        await modulos.MainMenuAsync();
    }
    catch(Exception ex)
    {
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
        return;
    }
}