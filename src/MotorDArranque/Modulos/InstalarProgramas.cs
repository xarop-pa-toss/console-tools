using MotorDArranque.Modelos;
using Spectre.Console;
using WGetNET;

namespace ConsoleTools.Modulos;

public partial class Modulos
{
    // TODO: Implementar InstalarProgramas
    public async Task<Resultado> Instalar(List<string> ids)
    {
        AnsiConsole.MarkupLine("[bold turquoise2]Programas a desinstalar:[/]");
        foreach (var id in ids ?? Enumerable.Empty<string>())
        {
            AnsiConsole.WriteLine(id);
        }

        var confirmar = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("Confirmar", "Cancelar")
                .HighlightStyle(Color.Violet)
        );

        return Resultado.Ok();
    }
}
