using ConsoleTools;
using ConsoleTools.Modulos;
using ConsoleTools.Utils;
using MotorDArranque.Modelos;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTools.Modulos;

public partial class Modulos
{
    public async Task<Resultado> ProgOpsMenu(List<string> progList)
    {
        ConsoleUtils.ImprimeLogo();
        AnsiConsole.Write(Align.Left(new Markup("[Bold Underline Turquoise2]Operações[/]")));

        var menu = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .WrapAround()
            .AddChoices([
                "Actualizar",
                "Desinstalar",
                "Ver informações",
                "Adicionar a um Pacote",
                "Remover de um Pacote",
                "Sair"])
            .HighlightStyle(new Style(Styles.Base.Background, decoration: Decoration.Bold)));

        // RESULTADOS MAIN MENU
        switch (menu)
        {
            case "Actualizar":
                _mh.Handle(await ListagemProgramas());
                break;
            case "Desinstalar":
                _mh.Handle(await ListagemProgramas());
                break;
            case "Ver informações":
                // await modulos.EcraDesinstalar;
                break;
            case "Adicionar a um Pacote":
                // await modulos.EcraDesinstalar;
                break;
            case "Remover de um Pacote":
                // await modulos.EcraDesinstalar;
                break;
            case "Sair":
                return Resultado.Ok();
            default:
                return Resultado.Ok();
        }

        return Resultado.Ok();
    }
}
