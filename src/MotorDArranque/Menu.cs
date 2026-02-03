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

namespace MotorDArranque;

public sealed class Menu(Modulos modulos)
{
    private readonly Modulos _modulos = modulos;

    public async Task RunAsync()
    {
        ImprimeLogo();
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
                MenuHandler(await _modulos.ListagemProgramas());
                break;
            case "Desinstalar":
                // await modulos.EcraDesinstalar;
                break;
            case "Sair":
                Environment.Exit(0);
                break;
            default:
                AnsiConsole.WriteLine("nothing");
                break;
        }
    }

    private void MenuHandler(Resultado resultado)
    {
        if (!resultado.IsSucesso)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
            Mensagens.ErroPanel(resultado.Erro);
        }
    }

    private void ImprimeLogo()
    {
        Utils.WriteGradient(Assets.InfoLogo3, Color.Purple, Color.Aqua);

        var panelTitulo = new Panel(
            new Markup(
                "[Invert Aqua]   MOTOR D'ARRANQUE   [/]\n\n" +
                "Ferramenta de instalação de software com Winget").Centered()
            ).BorderColor(Color.Purple)
            .HeaderAlignment(Justify.Center)
            .RoundedBorder();
        AnsiConsole.Write(Align.Left(panelTitulo));
    }
}
