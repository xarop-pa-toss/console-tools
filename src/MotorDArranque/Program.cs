using ConsoleTools;
using ConsoleTools.Modulos;
using MotorDArranque;
using Spectre.Console;

// STARTUP
Modulos Modulos = new Modulos();
// if (OperatingSystem.IsWindows())
// {
//     Console.SetWindowSize(500, 50);
//     Console.SetBufferSize(500,100);
//     AnsiConsole.Profile.Width = Console.WindowWidth;
// }
Directory.CreateDirectory(AppPaths.AppDirInUserTemp);
// Utils.WriteGradient(Assets.InfoLogo, Color.Purple, Color.Aqua);
//
// var panelTitulo = new Panel(
//     new Markup(
//         "[Invert Aqua]   MOTOR D'ARRANQUE   [/]\n\n" +
//         "Ferramenta de instalação de software com Winget").Centered()
//     ).BorderColor(Color.Purple)
//     .HeaderAlignment(Justify.Center)
//     .RoundedBorder();
// AnsiConsole.Write(Align.Left(panelTitulo));

AnsiConsole.Write(Align.Left(new Markup("[Bold Underline Turquoise2]Operações[/]")));

var mainMenu = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .WrapAround()
        .AddChoices(
            [
                "Lista de programas instalados",
                "Instalar",
                "Desinstalar",
                "Pacotes de Programas",
                "Sobre",
                "Sair"
            ])
        .HighlightStyle(new Style(Styles.Base.Background, decoration: Decoration.Bold)));

// RESULTADOS MAIN MENU
switch (mainMenu)
{
    case "Lista de programas instalados":
        await Modulos.ListagemProgramas();
        break;
    //case "Instalar":
    //    await Modulos.EcraInstalar;
    //    break;
    //case "Desinstalar":
    //    await Modulos.EcraDesinstalar;
    //    break;
    default:
        AnsiConsole.WriteLine("nothing");
        break;
}