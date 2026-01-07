using Spectre.Console;

namespace console_tools.Utils;

public static class Mensagens
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
}