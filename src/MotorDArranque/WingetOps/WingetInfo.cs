using console_tools.Utils;
using MotorDArranque.Modelos;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace MotorDArranque.WingetOps
{
    internal static class WingetInfo
    {
        public static async Task<List<ProgramInfo>> GetProgramasInstaladosAsync(string jsonExportPath)
        {
            string jsonFullPath = Path.Combine(AppPaths.UserTemp, "winget_instalados.json");

            var prinfo = new ProcessStartInfo
            {
                FileName = "winget",
                Arguments = $"export " +
                            "--include-versions " +
                            "--disable-interactivity " +
                            "--accept-source-agreements " +
                            $"--output \"{jsonFullPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(prinfo)
                ?? throw new InvalidOperationException("Erro ao iniciar processo Winget.");

            await AnsiConsole.Status()
                    .Spinner(Spinner.Known.Sand)
                    .SpinnerStyle(Style.Parse("bold turquoise2"))
                    .StartAsync("A encontrar programas instalados...", async ctx =>
                    {
                        await process.WaitForExitAsync();

                        if (process.ExitCode != 0)
                            throw new Exception($"Processo falhou com código {process.ExitCode}");
                        ;

                        Thread.Sleep(500);
                        AnsiConsole.Markup("[violet]:check_mark:A encontrar programas instalados.[/]");
                    });

            var programsList = new List<ProgramInfo>();
            await AnsiConsole.Status()
                    .Spinner(Spinner.Known.Sand)
                    .SpinnerStyle(Style.Parse("bold turquoise2"))
                    .StartAsync("A compilar informação...", async ctx =>
                    {
                        var json = await File.ReadAllTextAsync(jsonFullPath);
                        var programsList = JsonSerializer.Deserialize<List<ProgramInfo>>(json);

                        if (programsList == null || programsList.Count == 0)
                        {
                            Mensagens.Aviso("Não foram encontrados resultados!");
                        }

                        programsList
                    .OrderByDescending(x => x.Source)
                    .ThenBy(x => x.Id);

                        AnsiConsole.Markup("[violet]:check_mark:Terminado![/]");
                    });
            return programsList;
        }        
    }
}
