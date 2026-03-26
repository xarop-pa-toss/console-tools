using Spectre.Console;

namespace ConsoleTools.Framework;

public static class ConsoleToolsShell
{
    public static void ShowHeader()
    {
        AnsiConsole.Clear();

        var panel = new Panel(new Markup("[bold turquoise2]ConsoleTools[/]\nHost shell for pluggable console apps"))
            .Border(BoxBorder.Rounded)
            .BorderColor(ConsoleToolsStyles.AccentColor)
            .Header("[bold]Main Menu[/]", Justify.Center);

        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    public static string PromptMainAction(IReadOnlyList<IConsoleToolModule> modules)
    {
        var choices = modules.Select(m => $"{m.DisplayName} [grey]({m.Description})[/]").ToList();
        choices.Add("Exit");

        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[underline turquoise2]Select a tool[/]")
                .HighlightStyle(ConsoleToolsStyles.PromptHighlight)
                .AddChoices(choices));
    }

    public static void WaitForContinue(string message = "Press Enter to return to the main menu")
    {
        AnsiConsole.MarkupLine($"[grey]{message}[/]");
        Console.ReadLine();
    }
}
