using ConsoleTools.Utils;
using Spectre.Console;
using System.Diagnostics;
using System.Text;
using WGetNET;
namespace MotorDArranque;

public class WingetStartup
{
    private readonly WinGet _wget;
    private readonly WinGetPackageManager _packMgr;
    public WingetStartup(WinGet wget, WinGetPackageManager packMgr)
    {
        _wget = wget;
        _packMgr = packMgr;
    }

    public void RunStartupVerif()
    {
        // [UNRELIABLE]
        //
        // if (!ShellDetector.IsRunningInPowerShell())
        // {
        //     Mensagens.Aviso("O programa está a correr em CMD mas recomenda-se que use Powershell.");
        //     if (AnsiConsole.Confirm("Reiniciar em Powershell?"))
        //     {
        //         Utils.ReiniciarPrograma(3000);
        //     }
        // }
        
        if (!_wget.IsInstalled)
        {
            Mensagens.ErroPanel("WinGet não encontrado no sistema. É necessário instalar para usar o programa.");
            if (AnsiConsole.Confirm("Instalar?"))
            {
                InstalarWingetComPowershell();
                ConsoleUtils.ReiniciarPrograma(3000);
            }
        }
        
        string wgetId = "Microsoft.AppInstaller";
        var wingetPackage = _packMgr.GetInstalledPackages(wgetId, true).FirstOrDefault()!;
        
        if (wingetPackage.AvailableVersion > wingetPackage.Version)
        {
            Mensagens.AvisoPanel($"O WinGet está na versão [bold]{wingetPackage.VersionString}[/] mas está disponível a versão [bold]{wingetPackage.AvailableVersionString}[/]");

            if (AnsiConsole.Confirm("Actualizar WinGet?"))
            {
                _packMgr.UpgradePackage(wgetId);
            }
        }
    }

    public void InstalarWingetComPowershell()
    {
        var prcInfo = new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"Write-Host 'Downloading WinGet...';" +
                        " Invoke-WebRequest https://aka.ms/getwinget -OutFile winget.msixbundle -Verbose;" +
                        " Write-Host 'Installing WinGet...'; Add-AppxPackage winget.msixbundle -Verbose\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };

        var process = new Process { StartInfo = prcInfo };

        // Capture all output streams
        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                AnsiConsole.WriteLine(e.Data);
                outputBuilder.AppendLine(e.Data);
            }
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                // Note: -Verbose output goes to stderr in PowerShell
                AnsiConsole.WriteLine($"[dim]{e.Data}[/]");
                errorBuilder.AppendLine(e.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        // Check exit code instead of just error stream
        AnsiConsole.WriteLine("");
        if (process.ExitCode != 0)
        {
            Mensagens.AvisoPanel("Ocorreram erros no script PS de instalação do Winget.\nÉ possível que não tenha sido correctamente instalado.");
            return;
        }

        AnsiConsole.MarkupLine("[underline turquoise2]WinGet instalado com sucesso.[/]");
        ConsoleUtils.ReiniciarPrograma(200);
    }
}