using Spectre.Console;

namespace ConsoleTools.ConsoleUI;

/// <summary>
/// Configuration for a column in the multi-column selector
/// </summary>
/// <typeparam name="T">The type of items being displayed</typeparam>
public class ColumnConfig<T>
{
    public required string Header { get; init; }
    public required Func<T, string> ValueSelector { get; init; }
    public Func<T, int>? WidthCalculator { get; init; }
    public int? FixedWidth { get; init; }
    public int MinWidth { get; init; } = 0;
    public int Padding { get; init; } = 3;
}

/// <summary>
/// Configuration for the multi-column selector
/// </summary>
/// <typeparam name="T">The type of items being displayed</typeparam>
public class MultiColumnSelectorConfig<T>
{
    public required List<ColumnConfig<T>> Columns { get; init; }
    public required Func<T, string> DisplayFormatter { get; init; }
    public int PageSize { get; init; } = 25;
    public Color HighlightColor { get; init; } = Color.Violet;
    public Color HeaderColor { get; init; } = Color.Turquoise2;
    public bool Required { get; init; } = false;
    public string? Title { get; init; }
    public int TitlePadding { get; init; } = 8;
}

/// <summary>
/// A reusable multi-column selector component for Spectre.Console
/// </summary>
public static class MultiColumnSelector
{
    /// <summary>
    /// Displays a multi-selection prompt with multiple columns
    /// </summary>
    /// <typeparam name="T">The type of items to select from</typeparam>
    /// <param name="items">The items to display</param>
    /// <param name="config">Configuration for the selector</param>
    /// <returns>List of selected items</returns>
    public static List<T> Show<T>(IEnumerable<T> items, MultiColumnSelectorConfig<T> config) where T : notnull
    {
        var itemList = items.ToList();

        // Calculate column widths
        var columnWidths = CalculateColumnWidths(itemList, config.Columns);

        // Build header
        var header = BuildHeader(config.Columns, columnWidths, config.HeaderColor, config.TitlePadding);

        // Create the multi-selection prompt
        var prompt = new MultiSelectionPrompt<T>()
            .Title(config.Title ?? header)
            .PageSize(config.PageSize)
            .HighlightStyle(config.HighlightColor)
            .UseConverter(item => config.DisplayFormatter(item));

        if (!config.Required)
        {
            prompt.NotRequired();
        }

        // Add choices
        prompt.AddChoices(itemList);

        // Show the prompt and return selected items
        return AnsiConsole.Prompt(prompt);
    }

    /// <summary>
    /// Displays a multi-selection prompt with multiple columns and returns formatted strings
    /// </summary>
    /// <typeparam name="T">The type of items to select from</typeparam>
    /// <param name="items">The items to display</param>
    /// <param name="config">Configuration for the selector</param>
    /// <returns>List of formatted strings representing selected items</returns>
    public static List<string> ShowAsStrings<T>(IEnumerable<T> items, MultiColumnSelectorConfig<T> config) where T : notnull
    {
        var itemList = items.ToList();

        // Calculate column widths
        var columnWidths = CalculateColumnWidths(itemList, config.Columns);

        // Build header
        var header = BuildHeader(config.Columns, columnWidths, config.HeaderColor, config.TitlePadding);

        // Format items as strings
        var formattedItems = itemList.Select(item => config.DisplayFormatter(item)).ToList();

        // Create the multi-selection prompt with strings
        var prompt = new MultiSelectionPrompt<string>()
            .Title(config.Title ?? header)
            .PageSize(config.PageSize)
            .HighlightStyle(config.HighlightColor);

        if (!config.Required)
        {
            prompt.NotRequired();
        }

        // Add choices
        prompt.AddChoices(formattedItems);

        // Show the prompt
        return AnsiConsole.Prompt(prompt);
    }

    private static List<int> CalculateColumnWidths<T>(List<T> items, List<ColumnConfig<T>> columns)
    {
        var widths = new List<int>();

        foreach (var column in columns)
        {
            if (column.FixedWidth.HasValue)
            {
                widths.Add(column.FixedWidth.Value);
            }
            else if (column.WidthCalculator != null)
            {
                var maxWidth = items.Any()
                    ? items.Max(column.WidthCalculator) + column.Padding
                    : column.Header.Length + column.Padding;
                widths.Add(Math.Max(maxWidth, column.MinWidth + column.Padding));
            }
            else
            {
                var maxWidth = items.Any()
                    ? items.Max(item => column.ValueSelector(item).Length) + column.Padding
                    : column.Header.Length + column.Padding;
                widths.Add(Math.Max(maxWidth, column.MinWidth + column.Padding));
            }
        }

        return widths;
    }

    private static string BuildHeader<T>(List<ColumnConfig<T>> columns, List<int> widths, Color headerColor, int titlePadding)
    {
        var headerParts = new List<string>();

        for (int i = 0; i < columns.Count; i++)
        {
            headerParts.Add(columns[i].Header.PadRight(widths[i]));
        }

        return $"[underline {headerColor}]{new string(' ', titlePadding)}{string.Concat(headerParts)}[/]";
    }

    /// <summary>
    /// Formats a row with multiple columns based on column configuration
    /// </summary>
    /// <typeparam name="T">The type of item to format</typeparam>
    /// <param name="item">The item to format</param>
    /// <param name="columns">Column configuration</param>
    /// <param name="widths">Calculated column widths</param>
    /// <param name="rowPadding">Padding at the start of the row</param>
    /// <returns>Formatted string</returns>
    public static string FormatRow<T>(T item, List<ColumnConfig<T>> columns, List<int> widths, int rowPadding = 2)
    {
        var parts = new List<string>();

        for (int i = 0; i < columns.Count; i++)
        {
            var value = columns[i].ValueSelector(item);
            parts.Add(value.PadRight(widths[i]));
        }

        return $"{new string(' ', rowPadding)}{string.Concat(parts)}";
    }
}
