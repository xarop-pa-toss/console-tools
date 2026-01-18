using System.Diagnostics;
using ConsoleTools;
using ConsoleTools.Utils;
using Spectre.Console;
using WGetNET;
namespace MotorDArranque;

public class WingetStartupChecks
{
    private readonly WinGet _wget;
    private readonly WinGetPackageManager _packMgr;
    public WingetStartupChecks(WinGet wget, WinGetPackageManager packMgr)
    {
        _wget = wget;
        _packMgr = packMgr;
    }

    public void Run()
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
            Mensagens.Erro("WinGet não encontrado no sistema. É necessário instalar para usar o programa.");
            if (AnsiConsole.Confirm("Instalar? (script powershell)"))
            {
                InstalarWingetComPowershell();
                Utils.ReiniciarPrograma(3000);
            }
        }

        var _packMgr = new WinGetPackageManager();
        string wgetId = "Microsoft.AppInstaller";
        var wingetPackage = _packMgr.GetInstalledPackages(wgetId, true).FirstOrDefault()!;
        
        if (wingetPackage.AvailableVersion > wingetPackage.Version)
        {
            Mensagens.Aviso($"O WinGet está na versão [bold]{wingetPackage.VersionString}[/] mas está disponível a versão [bold]{wingetPackage.AvailableVersionString}[/]");

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
            Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"Invoke-WebRequest https://aka.ms/getwinget -OutFile winget.msixbundle; Add-AppxPackage winget.msixbundle\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var process = new Process { StartInfo = prcInfo };

        bool errored = false;
        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
                AnsiConsole.WriteLine(e.Data);
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
                Mensagens.Erro(e.Data);
            errored = true;
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        
        AnsiConsole.WriteLine("");
        if (errored)
        {
            Mensagens.Aviso("Ocorreram erros no script PS de instalação do Winget.\nÉ possível que não tenha sido correctamente instalado.");
            return;
        }
        
        AnsiConsole.MarkupLine("[underline turquoise2]WinGet instalado com sucesso.[/]");
    }
}