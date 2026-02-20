using Microsoft.Extensions.Logging;
using Spectre.Console;
namespace ConsoleTools.Utils;

public static class LoggerExtensions
{
    public static void MyLogInfo(this ILogger logger, string mensagem, bool tambemConsola = true)
    {
        logger.LogInformation(mensagem);
        
        if (tambemConsola)
            Mensagens.ErroPanel(mensagem);
    }

    public static void MyLogAviso(this ILogger logger, string mensagem, bool tambemConsola = true)
    {
        logger.LogWarning(mensagem);

        if (tambemConsola)
            Mensagens.AvisoPanel(mensagem);
    }

    public static void MyLogErro(this ILogger logger, string mensagem, bool tambemConsola = true)
    {
        logger.LogError(mensagem);

        if (tambemConsola)
            Mensagens.ErroPanel(mensagem);
    }
    
    public static void MyLogErro(this ILogger logger, string mensagem, Exception ex, bool tambemConsola = true)
    {
        logger.LogError(ex, mensagem);

        if (tambemConsola)
        {
            Mensagens.ErroPanel(mensagem);
            AnsiConsole.WriteException(ex);
        }
    }
}
