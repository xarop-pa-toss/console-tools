using MotorDArranque.Modelos;
using Spectre.Console;
using WGetNET;

namespace ConsoleTools.Modulos;

public partial class Modulos
{
    public async Task<Resultado> ListagemProgramas()
    {
        // var listaProgramas = await WingetBase.GetListaProgramasAsync();
        var listaProgramas = await _packMgr.GetInstalledPackagesAsync();
        var listaProgWinget = listaProgramas
            .FindAll(p => p.SourceName == "winget")
            .OrderByDescending(p => p.Version != p.AvailableVersion)
            .ThenBy(p => p.Name)
            .ToList();
        
        // Largura das colunas reflecte o nome e id mais longos
        int nameWidth = listaProgWinget.Max(p => p.Name.Length) + 3;
        int idWidth   = listaProgWinget.Max(p => p.Id.Length) + 3;
        int instWidth = listaProgWinget.Max(p => p.VersionString.Length) + 3;
        int dispWidth = listaProgWinget.Max(p => p.AvailableVersionString.Length);

        string headers = string.Concat(
            "[underline turquoise2]",
            new string(' ', 8),
            "Nome".PadRight(nameWidth),
            "Id".PadRight(idWidth),
            "Instalado".PadRight(instWidth),
            "Disponivel".PadRight(dispWidth),
            "Actualizado?",
            "[/]"
        );

        var selected = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title(headers)
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
        
        AnsiConsole.MarkupLine("[green]Selected:[/]");
        foreach (var s in selected)
        {
            AnsiConsole.WriteLine(s);
        }


        return Resultado.Ok();
    }
}