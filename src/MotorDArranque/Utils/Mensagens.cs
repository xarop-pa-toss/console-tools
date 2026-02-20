using Spectre.Console;

namespace ConsoleTools.Utils;

public static class Mensagens
{
    public static void ErroPanel(string? mensagem)
    {
        var panel = new Panel(new Markup($"[red]{mensagem}[/]"))
        {
            Header = new PanelHeader("[red bold] :cross_mark:  Erro [/]"),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Red3)
        };

        AnsiConsole.Write(panel);
    }

    public static Markup ErroMarkup(string? mensagem)
    {
        return (new Markup($"[red bold] :cross_mark:  Erro: [/][red]{mensagem}[/]"));
    }

    public static void AvisoPanel(string? mensagem)
    {
        var panel = new Panel(new Markup($"[yellow]{mensagem}[/]"))
        {
            Header = new PanelHeader("[yellow bold] :warning:  Aviso [/]"),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Yellow3_1)
        };

        AnsiConsole.Write(panel);
    }

    public static Markup AvisoMarkup(string? mensagem)
    {

        return (new Markup($"[yellow bold] :warning:  Aviso: [/][yellow]{mensagem}[/]"));
    }

    public static void SucessoPanel(string? mensagem)
    {
        var panel = new Panel(new Markup($"[green]{mensagem}[/]"))
        {
            Header = new PanelHeader("[green bold] :check_mark:  Sucesso [/"),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Green3)
        };

        AnsiConsole.Write(panel);
    }

    public static Markup SucessoMarkup(string? mensagem)
    {
        return (new Markup($"[green bold] :check_mark:  Sucesso: [/][green]{mensagem}[/]"));
    }
}