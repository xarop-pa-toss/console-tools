using ConsoleTools;
using ConsoleTools.Modulos;
using ConsoleTools.Utils;
using MotorDArranque;
using Spectre.Console;

// STARTUP
Modulos Modulos = new Modulos();
if (OperatingSystem.IsWindows())
{
    Console.SetWindowSize(200, 50);
    Console.SetBufferSize(200,100);
}

Directory.CreateDirectory(AppPaths.AppDirInUserTemp);

Utils.WriteGradient(Assets.InfoLogo, Color.Purple, Color.Aqua);

var panelTitulo = new Panel(
    new Markup(
        "[Invert Aqua]   MOTOR D'ARRANQUE   [/]\n\n" +
        "Ferramenta de instalação de software com Winget").Centered()
    ).BorderColor(Color.Purple)
    .HeaderAlignment(Justify.Center)
    .RoundedBorder();
AnsiConsole.Write(Align.Left(panelTitulo));
    
//    AnsiConsole.Prompt(
//    new ConfirmationPrompt("Quer analisar os programas instalados e procurar actualizações?")
//    {
//        DefaultValue = false,
//        ChoicesStyle = Styles.Base.Foreground,
//        DefaultValueStyle = Styles.Base.Background
        
//    }
//    .Yes('s')
//    .No('n')
//    .InvalidChoiceMessage("Ou (s)im ou (n)ão jovem...")
//);

AnsiConsole.MarkupLine("[Bold Underline Turquoise2]Operações[/]");
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