using MotorDArranque.Modelos;
using MotorDArranque.WingetOps;
using Spectre.Console;

namespace ConsoleTools.Modulos;

public partial class Modulos
{
    public async Task ListagemProgramas()
    {
        var listaProgramas = await WingetBase.GetListaProgramasAsync();
        
        // Largura das colunas reflecte o nome e id mais longos
        int nameWidth = listaProgramas.Max(p => p.Name.Length) + 3;
        int idWidth   = listaProgramas.Max(p => p.Id.Length) + 3;
        int instWidth = listaProgramas.Max(p => p.InstalledVersion.Length) + 3;
        int dispWidth = listaProgramas.Max(p => p.AvailableVersion.Length);

        // Multiselect prompt
        string headers = string.Concat(
            "[underline turquoise2]",
            new string(' ', 8),
            "Name".PadRight(nameWidth),
            "Id".PadRight(idWidth),
            "Installed".PadRight(instWidth),
            "Available".PadRight(dispWidth),
            "[/]"
        );
        AnsiConsole.MarkupLine(headers);

        var selected = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title(headers)
                .NotRequired()
                .HighlightStyle(Color.Violet)
                .MoreChoicesText("Mais programas abaixo!")
                .PageSize(20)
                .AddChoices(listaProgramas.Select(p =>
                    string.Concat(
                        new string(' ', 2),
                        p.Name.PadRight(nameWidth),
                        p.Id.PadRight(idWidth),
                        p.InstalledVersion.PadRight(instWidth),
                        p.AvailableVersion.PadRight(dispWidth)
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