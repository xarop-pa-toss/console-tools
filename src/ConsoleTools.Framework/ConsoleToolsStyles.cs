using Spectre.Console;

namespace ConsoleTools.Framework;

public static class ConsoleToolsStyles
{
    public static Style PromptHighlight { get; } = new(
        foreground: Color.Turquoise2,
        background: Color.Violet,
        decoration: Decoration.Bold);

    public static Color HeaderColor { get; } = Color.Turquoise2;
    public static Color AccentColor { get; } = Color.Violet;
}
