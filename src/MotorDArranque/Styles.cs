using Spectre.Console;

namespace console_tools;

public static class Styles
{
    public static Style Base { get; } = new Style
    (
        foreground: Color.Turquoise2,
        background: Color.Violet
    );
    
    public static Style BaseInverted { get; } = new Style
    (
        foreground: Color.Violet,
        background: Color.Turquoise2
    );
}