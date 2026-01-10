using System.Runtime.CompilerServices;
using ConsoleTools.Utils;
using MotorDArranque.Modelos;
using Spectre.Console;

namespace MotorDArranque.WingetOps
{
    internal static class WingetBase
    {
        static string jsonFullPath = Path.Combine(AppPaths.AppDirInUserTemp, "winget_instalados.json");

        public async static Task<List<ProgramInfo>> GetListaProgramasAsync()
        {
            List<ProgramInfo> listaProgramas = new List<ProgramInfo>();
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Sand)
                .SpinnerStyle(Style.Parse("bold turquoise2"))
                .StartAsync("A encontrar programas instalados...", async ctx =>
                {
                    try
                    {
                        listaProgramas = await GetProgramasInstaladosAsync();
                    }
                    catch(Exception e)
                    {
                        AnsiConsole.MarkupLine(e.Message);
                    }
                    
                    Thread.Sleep(200);
                    AnsiConsole.Markup("[violet]:check_mark:A encontrar programas instalados.[/]");
                });

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Sand)
                .SpinnerStyle(Style.Parse("bold turquoise2"))
                .StartAsync("A procurar actualizações...", async ctx =>
                {
                    listaProgramas = await GetVersoesDisponiveisAsync(listaProgramas);
                    Thread.Sleep(200);
                    AnsiConsole.Markup("[violet]:check_mark:A procurar actualizações.[/]");
                });
            
            return listaProgramas
                .OrderByDescending(x => x.InstalledVersion != x.AvailableVersion)
                .ThenBy(x => x.Name)
                .ToList();
        }

        private async static Task<List<ProgramInfo>> GetProgramasInstaladosAsync()
        {
            var resultado = await Utils.CorrerProcessoAsync(
                "winget",
                $"export --include-versions --output \"{jsonFullPath}\""
            );
            
            if (!File.Exists(jsonFullPath))
            {
                throw new FileNotFoundException(Mensagens.Erro(
                    "Lista de Programas obtida com sucesso mas JSON não foi escrito." +
                    $"\nTem permissões de escrita na pasta [link]{Path.GetDirectoryName(jsonFullPath)}[/]?"));
            }

            return Utils.ParseExportJsonParaListaProgramas(jsonFullPath);
        }

        private async static Task<List<ProgramInfo>> GetVersoesDisponiveisAsync(List<ProgramInfo> listaProgramas)
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
                // No entanto, a primeira tem uma quantidade grande de \r, - e espaço em branco, pelo que se lida com a primeira linha separadamente
                string? nome = null;
                string? versao = null;

                var linhasEnumerator = stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries).GetEnumerator();
                if (linhasEnumerator.MoveNext())
                {
                    string l = linhasEnumerator.Current.ToString();
                    int foundIndex = l.IndexOf("Found");

                    if (foundIndex >= 0 && nome == null)
                    {
                        int charAmountRemove = foundIndex + "Found".Length;
                        nome = l.Substring(charAmountRemove, l.IndexOf('[') - charAmountRemove).Trim();
                    }
                }

                while (linhasEnumerator.MoveNext())
                {
                    string l = linhasEnumerator.Current.ToString();
                    if (versao == null && l.Trim().StartsWith("Version:"))
                        versao = l.Replace("Version:", "").Trim();
                    
                    if (nome != null && versao != null)
                    {
                        prginfo.Name = nome;
                        prginfo.AvailableVersion = versao;
                        break;
                    }
                }
            }
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