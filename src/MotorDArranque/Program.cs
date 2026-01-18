using System.Runtime.InteropServices.ObjectiveC;
using Microsoft.Extensions.DependencyInjection;
using ConsoleTools;
using ConsoleTools.Modulos;
using ConsoleTools.Utils;
using MotorDArranque;
using Spectre.Console;
using WGetNET;

// DI SETUP
var services = new ServiceCollection();
services.AddSingleton<WinGet>();
services.AddSingleton<WinGetPackageManager>();
services.AddSingleton<WingetStartupChecks>();
services.AddSingleton<Modulos>();

var provider = services.BuildServiceProvider();

// VERIFICA ESTADO WINGET 
var checks = provider.GetRequiredService<WingetStartupChecks>();
checks.Run();

// CRIA PASTAS
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
var modulos = provider.GetRequiredService<Modulos>();
switch (mainMenu)
{
    case "Lista de programas instalados":
        await modulos.ListagemProgramas();
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