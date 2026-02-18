using System.Reflection;
using ConsoleTools.Utils;
using Microsoft.Extensions.Logging;
using MotorDArranque.Modelos;
using Spectre.Console;

namespace ConsoleTools.Modulos;

public class MenuHandler(ILogger<MenuHandler> logger)
{
    private readonly ILogger<MenuHandler> _logger = logger;

    public void Handle(Resultado resultado, bool imprimir = true, bool limparEcra = false)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        
        _logger.LogInformation(resultado.Info);
        Mensagens.ErroPanel(resultado.Info);
        _logger.LogWarning(resultado.Aviso);
        Mensagens.ErroPanel(resultado.Aviso);
        _logger.LogError(resultado.Erro);
        Mensagens.ErroPanel(resultado.Erro);
        
        AnsiConsole.WriteLine();
        
        if (limparEcra)
        {
            AnsiConsole.Markup("[bold]Enter[/] para voltar para o menu anterior.");
            Console.Read();
            AnsiConsole.Clear();
        }
    }
}
