using MotorDArranque.Modelos;
using Spectre.Console;
using WGetNET;

namespace ConsoleTools.Modulos;

public partial class Modulos
{
    public async Task<Resultado> ProcurarProgramasAsync()
    {
        var listaProgramas = await _packMgr.GetInstalledPackagesAsync();
        var listaProgWinget = listaProgramas
            .FindAll(p => p.SourceName == "winget")
            .OrderByDescending(p => p.Version != p.AvailableVersion)
            .ThenBy(p => p.Name)
            .ToList();
        
        // Largura das colunas reflecte o nome e id com maior num. de caracteres
        int nameWidth = listaProgWinget.Max(p => p.Name.Length) + 3;
        int idWidth   = listaProgWinget.Max(p => p.Id.Length) + 3;
        int instWidth = listaProgWinget.Max(p => p.VersionString.Length) + 3;
        int dispWidth = listaProgWinget.Max(p => p.AvailableVersionString.Length);

        string cabecalhos = string.Concat(
            "[underline turquoise2]",
            new string(' ', 8),
            "Nome".PadRight(nameWidth),
            "Id".PadRight(idWidth),
            "Instalado".PadRight(instWidth),
            "Disponivel".PadRight(dispWidth),
            "Actualizado?",
            "[/]"
        );

        var progList = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title(cabecalhos)
                .NotRequired()
                .HighlightStyle(Color.Violet)
                .PageSize(25)
                .AddChoices(listaProgWinget.Select(p =>
                    string.Concat(
                        new string(' ', 2),
                        p.Name.PadRight(nameWidth),
                        p.Id.PadRight(idWidth),
                        p.VersionString.PadRight(instWidth),
                        p.AvailableVersionString.PadRight(dispWidth),
                        (p.Version < p.AvailableVersion
                            ? new Markup("[yellow]:check_mark:[/]").ToString()
                            : new Markup("[green]:check_mark:[/]").ToString())
                ))
        ));
        
        AnsiConsole.MarkupLine("[bold turquoise2]Programas seleccionados: [/]");
        foreach (var s in progList)
        {
            AnsiConsole.WriteLine(s);
        }
        AnsiConsole.WriteLine();

        var operacao = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle(Color.Violet)
            .AddChoices(
                "Actualizar",
                "Desinstalar",
                "Voltar para a lista")
        );

        // No internal logic implemented here (stub). Caller should handle operations.
        return Resultado.Ok();
    }
}
