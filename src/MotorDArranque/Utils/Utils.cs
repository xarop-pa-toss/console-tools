using MotorDArranque.Modelos;
using Spectre.Console;
using System.Diagnostics;
using System.Text.Json;

namespace MotorDArranque;
public static class Utils
{
    private static string _erro { get; } = "[red]:cross_mark: Erro: ";
    private static string _aviso { get; } = "[yellow]:warning: Aviso: ";

    public static string Erro(string mensagem)
    {
        return _erro + mensagem + "[/]";
    }
    public static string Aviso(string mensagem)
    {
        return _aviso + mensagem + "[/]";
    }


    public static void WriteGradient(string text, Color start, Color end)
    {
        for (int i = 0; i < text.Length; i++)
        {
            // Linear interpolation for RGB
            int r = (int)(start.R + (end.R - start.R) * i / (text.Length - 1));
            int g = (int)(start.G + (end.G - start.G) * i / (text.Length - 1));
            int b = (int)(start.B + (end.B - start.B) * i / (text.Length - 1));

            // Print character with interpolated color
            AnsiConsole.Markup($"[rgb({r},{g},{b})]{text[i]}[/]");
        }
        AnsiConsole.WriteLine();
    }

    public class ProcessoResultado
    {
        public int ExitCode { get; set; }
        public string StdOut { get; set; } = string.Empty;
        public string StdErr { get; set; } = string.Empty;
    }

    public static async Task<ProcessoResultado> CorrerProcessoAsync(
        string nomeFicheiro, string argumentos, bool capturarOutput = false)
    {
        var psi = new ProcessStartInfo
        {
            FileName = nomeFicheiro,
            Arguments = argumentos,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = capturarOutput,
            RedirectStandardError = capturarOutput
        };

        using var process = Process.Start(psi)
            ?? throw new InvalidOperationException($"Erro ao iniciar processo {nomeFicheiro}.");

        string stdout = string.Empty;
        string stderr = string.Empty;

        if (capturarOutput)
        {
            stdout = await process.StandardOutput.ReadToEndAsync();
            stderr = await process.StandardError.ReadToEndAsync();
        }

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            throw new Exception($"Processo falhou com código {process.ExitCode}.\n{stderr}");

        return new ProcessoResultado
        {
            ExitCode = process.ExitCode,
            StdOut = stdout,
            StdErr = stderr
        };
    }

    public static async Task<List<ProgramInfo>> ParseExportJsonParaListaProgramas(string wingetExportJsonPath)
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
                            Name: "", // Não presente neste JSON
                            Id: pkg.PackageIdentifier,
                            InstalledVersion: pkg.Version,
                            AvailableVersion: "", //Não presente neste JSON
                            Source: source.SourceDetails.Name
                        )))
                .OrderByDescending(x => x.Source)
                .ThenBy(x => x.Id)
                .ToList();

        return listaProgramas;
    }
}