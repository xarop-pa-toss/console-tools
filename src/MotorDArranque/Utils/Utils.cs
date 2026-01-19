using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using MotorDArranque.Modelos;
using MotorDArranque.WingetOps;
using Spectre.Console;

namespace ConsoleTools.Utils;

public static class Utils
{
    public static void WriteGradient(string text, Color start, Color end)
    {
        for (int i = 0; i < text.Length; i++)
        {
            // Linear interpolation for RGB
            int r = (start.R + (end.R - start.R) * i / (text.Length - 1));
            int g = (start.G + (end.G - start.G) * i / (text.Length - 1));
            int b = (start.B + (end.B - start.B) * i / (text.Length - 1));

            // Print character with interpolated color
            AnsiConsole.Markup($"[rgb({r},{g},{b})]{text[i]}[/]");
        }
        AnsiConsole.WriteLine();
    }

    public class ProcessoResultado
    {
        public int CodigoErro { get; set; }
        public string DescErro { get; set; } = string.Empty;
        public string StdOut { get; set; } = string.Empty;
        public string StdErr { get; set; } = string.Empty;
    }

    public async static Task<ProcessoResultado> CorrerProcessoAsync(
        string nomeExe, string argumentos, bool capturarOutput = false, [CallerMemberName] string? caller = "")
    {
        var psi = new ProcessStartInfo
        {
            FileName = nomeExe,
            Arguments = argumentos,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = capturarOutput,
            RedirectStandardError = capturarOutput
        };

        using var process = Process.Start(psi)
                            ?? throw new InvalidOperationException($"Erro ao iniciar processo {nomeExe}.");

        string stdout = string.Empty;
        string stderr = string.Empty;
        if (capturarOutput)
        {
            stdout = await process.StandardOutput.ReadToEndAsync();
            stderr = await process.StandardError.ReadToEndAsync();
        }

        await process.WaitForExitAsync();

        string descErro = process.ExitCode == 0
            ? ""
            : psi.FileName == "winget"
                ? WingetCodigosErro.CodigosErro[process.ExitCode]
                : "";

        if (process.ExitCode != 0)
        {
            throw new Exception($"'{caller ?? "Um processo"}' terminou com erro:" +
                                $"\n{process.ExitCode} - {descErro}");
        }

        return new ProcessoResultado
        {
            CodigoErro = process.ExitCode,
            DescErro = descErro,
            StdOut = stdout,
            StdErr = stderr
        };
    }

    public static List<ProgramInfo> ParseExportJsonParaListaProgramas(string wingetExportJsonPath)
    {
        var json = File.ReadAllText(wingetExportJsonPath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var export = JsonSerializer.Deserialize<ExportJsonDTO>(json, options);

        List<ProgramInfo> listaProgramas =
            export!.Sources
                .SelectMany(source =>
                    source.Packages.Select(pkg =>
                        new ProgramInfo(
                            "", // Não presente neste JSON
                            pkg.PackageIdentifier,
                            pkg.Version,
                            "", //Não presente neste JSON
                            source.SourceDetails.Name
                        )))
                .ToList();

        return listaProgramas;
    }

    public static void ReiniciarPrograma(int delay, bool usePowershell = true)
    {
        string exePath = Assembly.GetExecutingAssembly().Location;
        ProcessStartInfo prcInfo;

        if (usePowershell)
        {
            AnsiConsole.MarkupLine("A reiniciar em Powershell (admin)...");
            prcInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = "-NoExit -Command \"$Host.UI.RawUI.BufferSize = " +
                            "New-Object Management.Automation.Host.Size(500, $Host.UI.RawUI.BufferSize.Height); " +
                            "& '" + exePath + "'\"",
                Verb = "runas",
                UseShellExecute = true
            };
        }
        else
        {
            AnsiConsole.MarkupLine("A reiniciar em CMD (admin)...");
            prcInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"\"{exePath}\"\"",
                UseShellExecute = true
            };
        }

        Thread.Sleep(delay);
        Process.Start(prcInfo);
        Environment.Exit(0);
    }
}