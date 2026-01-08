using Spectre.Console;

namespace console_tools.Modulos;

public partial class Modulos
{
    public async Task TabelaInstalados()
    {
        var tabelaInstalados = new Table()
            .Border(TableBorder.SimpleHeavy)
            .BorderStyle(Color.SlateBlue1)
            .ShowHeaders()
            .ShowRowSeparators()
            .LeftAligned();
    
        tabelaInstalados.AddColumn("Id", col => col.LeftAligned());
        tabelaInstalados.AddColumn("Instalado", col => col.RightAligned());
        tabelaInstalados.AddColumn("Disponivel", col => col.RightAligned());
        tabelaInstalados.AddColumn("Origem", col => col.Centered());
        
        var listaProgramas = await WingetOps.GetProgramasInstaladosAsync();
        if (listaProgramas.Count == 0)
        {
            AnsiConsole.Markup("[yellow]Não existem programas instalados através do Winget.[/]");
            return;
        }

        foreach (var prg in listaProgramas)
        {
            var installed = prg.Installed ?? "---";
            var available = prg.Available ?? "---";
            var markupAvailable = installed != available && available != "---"
                ? $"[yellow]{available}[/]"
                : installed;
    
            tabelaInstalados.AddRow(
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
    }
}