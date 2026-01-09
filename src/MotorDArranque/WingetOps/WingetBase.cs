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
    internal static class WingetBase
    {
        public static async Task<List<ProgramInfo>> GetListaProgramasInstalados()
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

                        Thread.Sleep(500);
                        AnsiConsole.Markup("[violet]:check_mark:A encontrar programas instalados.[/]");
                    });

            var programasLista = new List<ProgramInfo>();
            await AnsiConsole.Status()
                    .Spinner(Spinner.Known.Sand)
                    .SpinnerStyle(Style.Parse("bold turquoise2"))
                    .StartAsync("A compilar informação...", async ctx =>
                    {
                        var json = await File.ReadAllTextAsync(jsonFullPath);
                        programasLista = JsonSerializer.Deserialize<List<ProgramInfo>>(json);

                        if (programasLista == null || programasLista.Count == 0)
                        {
                            Mensagens.Aviso("Não foram encontrados programas instalados com o Winget!");
                        }


                    programasLista
                    .OrderByDescending(x => x.Source)
                    .ThenBy(x => x.Name);

                    AnsiConsole.Markup("[violet]:check_mark:Terminado![/]");
                    });

                
            return programasLista;
        }        

        public static async Task ExportarListaAsync(string jsonImportPath)
        {

        }

        public static async Task ImportarListaAsync(string jsonImportPath)
        {

        }

        private string GetVersoesDisponiveis(string programId)
        {
            var prinfo = new ProcessStartInfo
            {
                FileName = "winget",
                Arguments = $"upgrade --include-unknown",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(prinfo)
                ?? throw new InvalidOperationException("Erro ao iniciar processo Winget.");

        }
    }
}
