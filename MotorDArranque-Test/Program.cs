using Spectre.Console;
try
{
    throw new Exception("divide by zero");
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex,
        ExceptionFormats.ShortenEverything | ExceptionFormats.ShowLinks);
}
// var programs = new[]
// {
//     new { Id = "Git.Git", Version = "2.44.0", Available = "2.45.0" },
//     new { Id = "Docker.DockerDesktop", Version = "4.27.0", Available = "4.28.0" },
//     new { Id = "Microsoft.DotNet.SDK.8", Version = "8.0.1", Available = "8.0.2" }
// };
//
// // Display table (read-only)
// var table = new Table()
//     .AddColumn("Id")
//     .AddColumn("Installed")
//     .AddColumn("Available");
//
// foreach (var p in programs)
// {
//     table.AddRow(p.Id, p.Version, p.Available);
// }
//
// AnsiConsole.Write(table);
//
// // Multiselect prompt
// string headers = string.Format(
//     "[underline turquoise2]{0, 5}{1,-30} {2,-9} {3,-9}[/]",
//     "",
//     "Id",
//     "Version",
//     "Available"
// );
// //AnsiConsole.MarkupLine(headers);
// var selected = AnsiConsole.Prompt(
//     new MultiSelectionPrompt<string>()
//         .Title(headers)
//         .NotRequired()
//         .HighlightStyle(Color.Violet)
//         .PageSize(10)
//         .AddChoices(programs.Select(p =>
//             $"{p.Id,-30}|{p.Version,-10}|{p.Available,-10}"
//         ))
// );
//
// AnsiConsole.MarkupLine("[green]Selected:[/]");
// foreach (var s in selected)
// {
//     AnsiConsole.WriteLine(s);
// }