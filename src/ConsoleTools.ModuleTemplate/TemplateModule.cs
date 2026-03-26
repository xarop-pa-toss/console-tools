using ConsoleTools.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace ConsoleTools.ModuleTemplate;

public sealed class TemplateModule(ILogger<TemplateModule> logger) : IConsoleToolModule
{
    private readonly ILogger<TemplateModule> _logger = logger;

    public string Id => "template-module";
    public string DisplayName => "Template Module";
    public string Description => "Starter skeleton for new modules";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            var panel = new Panel("[bold turquoise2]Module Template[/]\nUse this project as a base for new modules.")
                .Border(BoxBorder.Rounded)
                .BorderColor(ConsoleToolsStyles.AccentColor);
            AnsiConsole.Write(panel);

            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[underline turquoise2]Actions[/]")
                    .HighlightStyle(ConsoleToolsStyles.PromptHighlight)
                    .AddChoices(
                        "Run sample action",
                        "Back to ConsoleTools"));

            if (action == "Back to ConsoleTools")
            {
                return;
            }

            await RunSampleActionAsync(cancellationToken);
        }
    }

    private Task RunSampleActionAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Template module sample action executed.");
        AnsiConsole.MarkupLine("[green]Sample action completed.[/]");
        ConsoleToolsShell.WaitForContinue();
        return Task.CompletedTask;
    }
}

public static class TemplateModuleServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateModule(this IServiceCollection services)
    {
        services.AddSingleton<IConsoleToolModule, TemplateModule>();
        return services;
    }
}
