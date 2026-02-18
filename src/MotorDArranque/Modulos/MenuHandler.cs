using System.Reflection;
using ConsoleTools.Utils;
using Microsoft.Extensions.Logging;
using MotorDArranque.Modelos;
using Spectre.Console;

namespace ConsoleTools.Modulos;

public class MenuHandler(ILogger<MenuHandler> logger)
{
    private readonly ILogger<MenuHandler> _logger = logger;

    public void Handle(Resultado resultado, bool silent = false, bool limparEcra = false)
    {
        AnsiConsole.WriteLine();
        _logger.MyLogInfo(resultado.Info, tambemConsola: !silent);
        _logger.MyLogInfo(resultado.Aviso, tambemConsola: !silent);
        _logger.MyLogInfo(resultado.Erro, tambemConsola: !silent);
        AnsiConsole.WriteLine();
        
        if (limparEcra)
        {
            AnsiConsole.Markup("[bold]Enter[/] para continuar...");
            Console.Read();
            AnsiConsole.Clear();
        }
    }
}
