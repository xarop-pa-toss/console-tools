using MotorDArranque.Modelos;
using MotorDArranque.WingetOps;
using Spectre.Console;
using WGetNET;

namespace ConsoleTools.Modulos;

public partial class Modulos
{
    private readonly WinGet _wget;
    private readonly WinGetPackageManager _packMgr;

    public Modulos(WinGet wget, WinGetPackageManager packMgr)
    {
        _wget = wget;
        _packMgr = packMgr;
    }
    public async Task ListagemProgramas()
    {
        // var listaProgramas = await WingetBase.GetListaProgramasAsync();
        var listaProgramas = await _packMgr.GetInstalledPackagesAsync();
        var listaProgWinget = listaProgramas.FindAll(p => p.SourceName == "winget");
        
        // Largura das colunas reflecte o nome e id mais longos
        int nameWidth = listaProgWinget.Max(p => p.Name.Length) + 3;
        int idWidth   = listaProgWinget.Max(p => p.Id.Length) + 3;
        int instWidth = listaProgWinget.Max(p => p.VersionString.Length) + 3;
        int dispWidth = listaProgWinget.Max(p => p.AvailableVersionString.Length);

        // Multiselect prompt
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
                            ? new Markup("[yellow]:check_mark:[/]")
                            : new Markup("[green]:check_mark:[/]")

                    )
                ))
        );
        
        AnsiConsole.MarkupLine("[green]Selected:[/]");
        foreach (var s in selected)
        {
            AnsiConsole.WriteLine(s);
        }
    }
}