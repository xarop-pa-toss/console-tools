using ConsoleTools.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Diagnostics;

namespace TeleDroid;

public sealed class TeleDroidModule(ILogger<TeleDroidModule> logger) : IConsoleToolModule
{
    private readonly ILogger<TeleDroidModule> _logger = logger;

    public string Id => "teledroid";
    public string DisplayName => "TeleDroid";
    public string Description => "scrcpy wrapper";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Panel("[bold turquoise2]TeleDroid[/]\nRemote Android access via scrcpy")
                .Border(BoxBorder.Rounded)
                .BorderColor(ConsoleToolsStyles.AccentColor));

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[underline turquoise2]Actions[/]")
                    .HighlightStyle(ConsoleToolsStyles.PromptHighlight)
                    .AddChoices(
                        "Start scrcpy (USB)",
                        "Start scrcpy (TCP/IP 5555)",
                        "Back to ConsoleTools"));

            if (selected == "Back to ConsoleTools")
            {
                return;
            }

            var scrcpyPath = ResolveScrcpyExecutable();
            var args = selected.Contains("TCP/IP", StringComparison.Ordinal) ? "--tcpip=5555" : string.Empty;

            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = scrcpyPath,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WorkingDirectory = Path.GetDirectoryName(scrcpyPath) ?? Directory.GetCurrentDirectory()
                });

                if (process is null)
                {
                    throw new InvalidOperationException("Failed to start scrcpy process.");
                }

                await process.WaitForExitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to run scrcpy");
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                ConsoleToolsShell.WaitForContinue();
            }
        }
    }

    private static string ResolveScrcpyExecutable()
    {
        var candidates = new List<string>();

        if (OperatingSystem.IsWindows())
        {
            candidates.Add(Path.Combine(Directory.GetCurrentDirectory(), "src", "TeleDroid", "third-party", "scrcpy-win64-v3.3.4", "scrcpy.exe"));
        }
        else if (OperatingSystem.IsLinux())
        {
            candidates.Add(Path.Combine(Directory.GetCurrentDirectory(), "src", "TeleDroid", "third-party", "scrcpy-linux-x86_64-v3.3.4", "scrcpy"));
        }

        var localPath = candidates.FirstOrDefault(File.Exists);
        if (!string.IsNullOrWhiteSpace(localPath))
        {
            return localPath;
        }

        return OperatingSystem.IsWindows() ? "scrcpy.exe" : "scrcpy";
    }
}

public static class TeleDroidServiceCollectionExtensions
{
    public static IServiceCollection AddTeleDroidModule(this IServiceCollection services)
    {
        services.AddSingleton<IConsoleToolModule, TeleDroidModule>();
        return services;
    }
}
