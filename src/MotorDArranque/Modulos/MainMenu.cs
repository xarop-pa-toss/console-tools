using ConsoleTools;
using ConsoleTools.Modulos;
using ConsoleTools.Utils;
using MotorDArranque.Modelos;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WGetNET;

namespace ConsoleTools.Modulos;

public partial class Modulos
{
    public async Task RunAsync()
    {
        ConsoleUtils.ImprimeLogo();
        AnsiConsole.Write(Align.Left(new Markup("[Bold Underline Turquoise2]Operações[/]")));

        var mainMenu = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .WrapAround()
            .AddChoices([
                "Lista de programas instalados",
                "Pacotes de Programas",
                "Sobre",
                "Sair"])
            .HighlightStyle(new Style(Styles.Base.Background, decoration: Decoration.Bold)));

        // RESULTADOS MAIN MENU
        switch (mainMenu)
        {
            case "Lista de programas instalados":
                _mh.Handle(await ListagemProgramas());
                break;
            case "Instalar programa":
                _mh.Handle(await ProcurarProgramas());
                _mh.Handle(await InstalarProgramas());
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
