using ConsoleTools.Framework;
using ConsoleTools.Framework.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotorDArranque;
using TeleDroid;

var services = new ServiceCollection();
var logsPath = Path.Combine(Path.GetTempPath(), "ConsoleTools", "logs");
Directory.CreateDirectory(logsPath);

services.AddConsoleToolsLogging(logsPath);
services.AddMotorDArranqueModule();
services.AddTeleDroidModule();

var provider = services.BuildServiceProvider();
var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger("ConsoleTools.Host");
var modules = provider.GetServices<IConsoleToolModule>().OrderBy(m => m.DisplayName).ToList();

if (modules.Count == 0)
{
    ConsoleToolsShell.ShowHeader();
    Console.WriteLine("No modules registered.");
    return;
}

while (true)
{
    ConsoleToolsShell.ShowHeader();
    var selected = ConsoleToolsShell.PromptMainAction(modules);

    if (selected == "Exit")
    {
        break;
    }

    var module = modules.FirstOrDefault(m => selected.StartsWith(m.DisplayName, StringComparison.Ordinal));
    if (module is null)
    {
        continue;
    }

    try
    {
        await module.RunAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Unhandled exception while running module {ModuleId}", module.Id);
        ConsoleToolsShell.WaitForContinue("Module failed. Press Enter to return to the main menu");
    }
}
