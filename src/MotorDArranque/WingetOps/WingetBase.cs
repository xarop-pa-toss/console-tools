using console_tools.Utils;
using MotorDArranque.Modelos;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MotorDArranque.WingetOps
{
    internal static class WingetBase
    {
        static string jsonFullPath = Path.Combine(AppPaths.UserTemp, "winget_instalados.json");

        public static async Task<List<ProgramInfo>> GetListaProgramasAsync()
        {
            //var listaProgramasInstalados = GetProgramasInstaladosAsync();
            //var listaVersoesDisponiveis = GetListaProgramasWingetCompletaAsync();
            //await Task.WhenAll(listaProgramasInstalados, listaVersoesDisponiveis);
            //List<ProgramInfo> listaCompilada = CompilarListaProgramas(
            //    listaProgramasInstalados.Result,
            //    listaVersoesDisponiveis.Result
            //);
            List<ProgramInfo> listaProgramas = new List<ProgramInfo>();

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Sand)
                .SpinnerStyle(Style.Parse("bold turquoise2"))
                .StartAsync("A compilar informação...", async ctx =>
                {
                    var listaResult = await GetListaProgramasWingetCompletaAsync();

                    listaProgramas = listaResult
                        .OrderByDescending(x => x.Source)
                        .ThenBy(x => x.Name)
                        .ToList();

                    AnsiConsole.Markup("[violet]:check_mark:Terminado![/]");
                });

            return listaProgramas;
        }        

        private static async Task<List<ProgramInfo>> GetProgramasInstaladosAsync()
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Sand)
                .SpinnerStyle(Style.Parse("bold turquoise2"))
                .StartAsync("A encontrar programas instalados...", async ctx =>
                {
                    await Utils.CorrerProcessoAsync(
                        "winget",
                        $"export --include-versions --output \"{jsonFullPath}\""
                    );

                    Thread.Sleep(200);
                    AnsiConsole.Markup("[violet]:check_mark:A encontrar programas instalados.[/]");
                });

            return await Utils.ParseExportJsonParaListaProgramas(jsonFullPath);
        }

        private static async Task<List<ProgramInfo>> GetListaProgramasWingetCompletaAsync()
        {
            var resultadoProcesso = await Utils.CorrerProcessoAsync(
                "winget",
                "upgrade --include-unknown --source winget",
                true
            );
            string stdout = resultadoProcesso.StdOut;

            var listaProgramas = new List<ProgramInfo>();
            var linhas = stdout
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .Skip(2)
                .ToList();

            #region AI CODE FOR PARSING WINGET STDOUT TABLE COLUMNS
            foreach (var lin in linhas.Take(linhas.Count - 1))
            {
                if (string.IsNullOrWhiteSpace(lin)) continue;

                string remaining = lin;
                string available = null;
                string installed = null;
                string id = null;

                // 1. Grab last uninterrupted string from the right (Disponível)
                int lastSpace = remaining.LastIndexOf(' ');
                if (lastSpace >= 0)
                {
                    available = remaining[(lastSpace + 1)..].Trim();
                    remaining = remaining[..lastSpace].TrimEnd();
                }
                else
                {
                    available = remaining;
                    remaining = "";
                }

                // 2. Grab second to last string (InstalledVersion)
                lastSpace = remaining.LastIndexOf(' ');
                if (lastSpace >= 0)
                {
                    installed = remaining[(lastSpace + 1)..].Trim();
                    remaining = remaining[..lastSpace].TrimEnd();
                }
                else
                {
                    installed = remaining;
                    remaining = "";
                }

                // 3. Grab third to last string (ID)
                lastSpace = remaining.LastIndexOf(' ');
                if (lastSpace >= 0)
                {
                    id = remaining[(lastSpace + 1)..].Trim();
                    remaining = remaining[..lastSpace].TrimEnd();
                }
                else
                {
                    id = remaining;
                    remaining = "";
                }

                // 4. Remaining is Name
                string name = remaining;

                listaProgramas.Add(new ProgramInfo(
                    Name: name,
                    Id: id,
                    InstalledVersion: installed,
                    AvailableVersion: available,
                    Source: "winget"
                ));
            }
            #endregion
            return listaProgramas;
        }

        public static async Task ExportarListaAsync(string jsonExportPath)
        {
            //await AnsiConsole.Status()
            //    .Spinner(Spinner.Known.Sand)
            //    .SpinnerStyle(Style.Parse("bold turquoise2"))
            //    .StartAsync("", async ctx =>
            //    {
            //        await Utils.CorrerProcessoAsync(
            //            "winget",
            //            $"export --include-versions --output \"{jsonExportPath}\""
            //        );

            //        Thread.Sleep(200);
            //        AnsiConsole.Markup("[violet]:check_mark:A encontrar programas instalados.[/]");
            //    });
        }

        public static async Task ImportarListaAsync(string jsonImportPath)
        {

        }
    }
}
