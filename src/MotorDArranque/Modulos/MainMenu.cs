using ConsoleTools.Utils;
using Spectre.Console;

namespace ConsoleTools.Modulos;

public partial class Modulos
{
    public async Task MainMenuAsync()
    {
        ConsoleUtils.ImprimeLogo();
        AnsiConsole.Write(Align.Left(new Markup("[Bold Underline Turquoise2]Operações[/]")));

        var mainMenu = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .WrapAround()
            .AddChoices(
                "Lista de programas instalados",
                "Pacotes de Programas",
                "Sobre",
                "Sair")
            .HighlightStyle(new Style(Styles.Base.Background, decoration: Decoration.Bold)));

        // RESULTADOS MAIN MENU
        switch (mainMenu)
        {
            case "Lista de programas instalados":
                _menuHandler.Handle(await ListagemProgramas());
                break;
            case "Pacotes de Programas":
                _menuHandler.Handle(await ProcurarProgramasAsync());
                break;
            case "Sair":
                Environment.Exit(0);
                break;
            default:
                AnsiConsole.WriteLine("nothing");
                break;
        }
    }
}
