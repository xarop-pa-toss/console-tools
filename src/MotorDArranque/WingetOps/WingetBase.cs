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
        static string jsonFullPath = Path.Combine(AppPaths.AppDirInUserTemp, "winget_instalados.json");

        public async static Task<List<ProgramInfo>> GetListaProgramasAsync()
        {
            // List<ProgramInfo> listaCompilada = CompilarListaProgramas(
            //     listaProgramasInstalados.Result,
            //     listaVersoesDisponiveis.Result
            // );
            var listaProgramas = await GetProgramasInstaladosAsync();
            listaProgramas = await GetVersoesDisponiveisAsync(listaProgramas);
            
            // await AnsiConsole.Status()
            //     .Spinner(Spinner.Known.Sand)
            //     .SpinnerStyle(Style.Parse("bold turquoise2"))
            //     .StartAsync("A compilar informação...", async ctx =>
            //     {
            //         var listaResult = await GetListaProgramasWingetCompletaAsync();
            //
            //         listaProgramas = listaResult
            //             .OrderByDescending(x => x.Name)
            //             .ToList();
            //
            //         AnsiConsole.Markup("[violet]:check_mark:Terminado![/]");
            //     });

            return listaProgramas;
        }        

        private async static Task<List<ProgramInfo>> GetProgramasInstaladosAsync()
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

            return Utils.ParseExportJsonParaListaProgramas(jsonFullPath);
        }

        private async static Task<List<ProgramInfo>> GetVersoesDisponiveisAsync(List<ProgramInfo> listaProgramas)
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Sand)
                .SpinnerStyle(Style.Parse("bold turquoise2"))
                .StartAsync("A procurar actualizações...", async ctx =>
                {
                    foreach (ProgramInfo prginfo in listaProgramas)
                    {
                        var resultadoProcesso = await Utils.CorrerProcessoAsync(
                            "winget",
                            $"show --id {prginfo.Id}",
                            true
                        );
                
                        string stdout = resultadoProcesso.StdOut;
                        // As primeiras duas linhas do winget show têm o formato:
                        // Found SumatraPDF [SumatraPDF.SumatraPDF]
                        // Version: 3.5.2
                        string? nome = null;
                        string? versao = null;

                        foreach (var linha in stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                        {
                            var l = linha.Trim();

                            if (nome == null && l.StartsWith("\rFound "))
                                prginfo.Name = l
                                    .Substring(6, l.IndexOf('[') - 6)
                                    .Trim();
                            else if (versao == null && l.StartsWith("Version:"))
                                prginfo.AvailableVersion = l
                                    .Replace("Version:", "")
                                    .Trim();

                            if (nome!= null && versao != null)
                                break;
                        }
                    }

                    Thread.Sleep(200);
                    AnsiConsole.Markup("[violet]:check_mark:A procurar actualizações.[/]");
                });
            return listaProgramas;
        }

        [Obsolete]
        private async static Task<List<ProgramInfo>> GetListaProgramasWingetCompletaAsync()
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

                listaProgramas.Add(new ProgramInfo(name, id, installed, available, "winget"));
            }
            #endregion
            return listaProgramas;
        }

        public async static Task ExportarListaAsync(string jsonExportPath)
        {
            //
            //TODO: Exportar Pacote para formato JSON "winget export"
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

        public async static Task ImportarListaAsync(string jsonImportPath)
        {
            //TODO: Criar novo Pacote a partir de um JSON "winget export"
        }
    }
}
