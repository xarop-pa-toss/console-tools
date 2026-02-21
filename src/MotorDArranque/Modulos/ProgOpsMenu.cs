using MotorDArranque.Modelos;
using Spectre.Console;

namespace ConsoleTools.Modulos;

public partial class Modulos
{
    public async Task<Resultado> ProgOpsMenu(List<string> progList)
    {
        
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
                _menuHandler.Handle(await ListagemProgramas());
                break;
            case "Desinstalar":
                _menuHandler.Handle(await ListagemProgramas());
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
