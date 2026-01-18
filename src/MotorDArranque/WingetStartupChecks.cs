using System.Diagnostics;
using ConsoleTools;
using ConsoleTools.Utils;
using Spectre.Console;
using WGetNET;
namespace MotorDArranque;

public class WingetStartupChecks
{
    private readonly WinGet _wget;
    public WingetStartupChecks(WinGet wget)
    {
        _wget = wget;
    }

    public void Run()
    {
        if (!_wget.IsInstalled)
        {
            AnsiConsole.Markup(Mensagens.Erro("WinGet não encontrado no sistema. É necessário instalar para usar o programa."));
            if (AnsiConsole.Confirm("Instalar? (script powershell)"))
            {
                InstalarWingetComPowershell();
                Utils.ReiniciarPrograma(3000);
            }
        }

        var packMgr = new WinGetPackageManager();
        string wgetId = "Microsoft.AppInstaller";
        var wingetPackage = packMgr.GetInstalledPackages(wgetId, true).FirstOrDefault()!;
        
        if (wingetPackage.AvailableVersion > wingetPackage.Version)
        {
            AnsiConsole.Markup(Mensagens.Aviso(
                $"O WinGet está na versão [bold]{wingetPackage.VersionString}[/] mas está disponível a versão [bold]{wingetPackage.AvailableVersionString}[/]"));

            if (AnsiConsole.Confirm("Actualizar WinGet?"))
            {
                packMgr.UpgradePackage(wgetId);
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
                AnsiConsole.WriteLine(Mensagens.Erro(e.Data));
            errored = true;
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        
        AnsiConsole.WriteLine("");
        if (errored)
        {
            AnsiConsole.MarkupLine(Mensagens.Aviso("Ocorreram erros no script PS de instalação do Winget.\nÉ possível que não tenha siddo correctamente instalado."));
            return;
        }
        
        AnsiConsole.MarkupLine("[underline turquoise2]WinGet instalado com sucesso.[/]");
        AnsiConsole.MarkupLine("[bold violet]O programa irá reiniciar em modo administrador em breve.[/]");

        Utils.ReiniciarPrograma(3000);



    }
}