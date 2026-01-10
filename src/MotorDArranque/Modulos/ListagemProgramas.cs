using MotorDArranque.WingetOps;
using Spectre.Console;

namespace ConsoleTools.Modulos;

public partial class Modulos
{
    public async Task ListagemProgramas()
    {
        var tabelaInstalados = new Table()
            .Border(TableBorder.SimpleHeavy)
            .BorderStyle(Color.SlateBlue1)
            .ShowHeaders()
            .ShowRowSeparators()
            .LeftAligned();

        tabelaInstalados.AddColumn("Nome", col => col.LeftAligned());
        tabelaInstalados.AddColumn("Id", col => col.LeftAligned());
        tabelaInstalados.AddColumn("Instalado", col => col.RightAligned());
        tabelaInstalados.AddColumn("Disponivel", col => col.RightAligned());
        // tabelaInstalados.AddColumn("Origem", col => col.Centered());
       
        var listaProgramas = (await WingetBase.GetListaProgramasAsync())
            .OrderByDescending(x => x.AvailableVersion != x.InstalledVersion)
            .ThenBy(x => x.Name)
            .ToList();
        
        if (listaProgramas.Count == 0)
        {
            AnsiConsole.Markup("[yellow]Não existem programas instalados através do Winget.[/]");
            return;
        }

        foreach (var prg in listaProgramas)
        {
            var installed = prg.InstalledVersion ?? "---";
            var available = prg.AvailableVersion ?? "---";
            var markupAvailable = installed != available && available != "---"
                ? $"[yellow]{available}[/]"
                : installed;
    
            tabelaInstalados.AddRow(
                prg.Name,
                prg.Id,
                installed,
                markupAvailable
            );
        }
        
        AnsiConsole.Write("/n/n");
        
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[Bold Underline Turquoise2]Escolha como proceder[/]")
                .WrapAround()
                .AddChoices(
                [
                    "Actualizar todos",
                    "Escolher actualizações",
                    "Sair"
                ])
                .HighlightStyle(new Style(Styles.Base.Background, decoration: Decoration.Bold)));
        
        AnsiConsole.Write(tabelaInstalados);
    }
}