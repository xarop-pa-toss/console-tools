using Spectre.Console;

namespace ConsoleTools.Utils;

public static class Mensagens
{
    public static void Erro(string mensagem)
    {
        var panel = new Panel(new Markup($"[red]{mensagem}[/]"))
        {
            Header = new PanelHeader("[red bold] :cross_mark:  Erro [/]"),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Red3)
        };

        AnsiConsole.Write(panel);
    }
    
    public static void Aviso(string mensagem)
    {
        var panel = new Panel(new Markup($"[yellow]{mensagem}[/]"))
        {
            Header = new PanelHeader("[yellow bold] :warning:  Aviso [/]"),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Yellow3_1)
        };

        AnsiConsole.Write(panel);
    }
}