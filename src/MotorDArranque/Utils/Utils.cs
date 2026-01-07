using Spectre.Console;

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
}