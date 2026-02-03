using ConsoleTools.Utils;
using Microsoft.Extensions.Logging;
using MotorDArranque.Modelos;
using Spectre.Console;

namespace ConsoleTools.Modulos;

public class MenuHandler(ILogger<MenuHandler> logger)
{
    private readonly ILogger<MenuHandler> _logger = logger;

    public void Handle(Resultado resultado)
    {
        if (!resultado.IsSucesso)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
            _logger.LogError(resultado.Erro);
            Mensagens.ErroPanel(resultado.Erro);
            AnsiConsole.WriteLine();
        }
    }
}
