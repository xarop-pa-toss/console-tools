using Spectre.Console;

namespace ConsoleTools.ConsoleUI;

/// <summary>
/// Interface for package info to allow selector to work with any package type
/// </summary>
public interface IPackageInfo
{
    string Name { get; }
    string Id { get; }
    string VersionString { get; }
    string AvailableVersionString { get; }
    Version Version { get; }
    Version AvailableVersion { get; }
}

/// <summary>
/// Pre-configured selector for package managers
/// </summary>
public static class PackageSelector
{
    /// <summary>
    /// Shows a multi-selection prompt for packages with standard columns
    /// </summary>
    /// <typeparam name="T">Package type implementing IPackageInfo</typeparam>
    /// <param name="packages">The packages to display</param>
    /// <param name="pageSize">Number of items per page (default: 25)</param>
    /// <param name="highlightColor">Highlight color (default: Violet)</param>
    /// <returns>List of selected packages</returns>
    public static List<T> SelectPackages<T>(
        IEnumerable<T> packages,
        int pageSize = 25,
        Color? highlightColor = null) where T : notnull, IPackageInfo
    {
        var packageList = packages.ToList();

        var config = new MultiColumnSelectorConfig<T>
        {
            Columns = new List<ColumnConfig<T>>
            {
                new() { Header = "Nome", ValueSelector = p => p.Name, MinWidth = 10 },
                new() { Header = "Id", ValueSelector = p => p.Id, MinWidth = 10 },
                new() { Header = "Instalado", ValueSelector = p => p.VersionString, MinWidth = 10 },
                new() { Header = "Disponivel", ValueSelector = p => p.AvailableVersionString, MinWidth = 10 },
                new()
                {
                    Header = "Actualizado?",
                    ValueSelector = p => p.Version < p.AvailableVersion
                        ? new Markup("[yellow]:check_mark:[/]").ToString()
                        : new Markup("[green]:check_mark:[/]").ToString(),
                    FixedWidth = 13
                }
            },
            DisplayFormatter = p => FormatPackageRow(p),
            PageSize = pageSize,
            HighlightColor = highlightColor ?? Color.Violet,
            HeaderColor = Color.Turquoise2,
            Required = false,
            TitlePadding = 8
        };

        return MultiColumnSelector.Show(packageList, config);
    }

    /// <summary>
    /// Shows a multi-selection prompt for packages and returns formatted strings
    /// </summary>
    /// <typeparam name="T">Package type implementing IPackageInfo</typeparam>
    /// <param name="packages">The packages to display</param>
    /// <param name="pageSize">Number of items per page (default: 25)</param>
    /// <param name="highlightColor">Highlight color (default: Violet)</param>
    /// <returns>List of formatted strings representing selected packages</returns>
    public static List<string> SelectPackagesAsStrings<T>(
        IEnumerable<T> packages,
        int pageSize = 25,
        Color? highlightColor = null) where T : notnull, IPackageInfo
    {
        var packageList = packages.ToList();

        var config = new MultiColumnSelectorConfig<T>
        {
            Columns = new List<ColumnConfig<T>>
            {
                new() { Header = "Nome", ValueSelector = p => p.Name, MinWidth = 10 },
                new() { Header = "Id", ValueSelector = p => p.Id, MinWidth = 10 },
                new() { Header = "Instalado", ValueSelector = p => p.VersionString, MinWidth = 10 },
                new() { Header = "Disponivel", ValueSelector = p => p.AvailableVersionString, MinWidth = 10 },
                new()
                {
                    Header = "Actualizado?",
                    ValueSelector = p => p.Version < p.AvailableVersion
                        ? new Markup("[yellow]:check_mark:[/]").ToString()
                        : new Markup("[green]:check_mark:[/]").ToString(),
                    FixedWidth = 13
                }
            },
            DisplayFormatter = p => FormatPackageRow(p),
            PageSize = pageSize,
            HighlightColor = highlightColor ?? Color.Violet,
            HeaderColor = Color.Turquoise2,
            Required = false,
            TitlePadding = 8
        };

        return MultiColumnSelector.ShowAsStrings(packageList, config);
    }

    private static string FormatPackageRow<T>(T package) where T : IPackageInfo
    {
        var columns = new List<ColumnConfig<T>>
        {
            new() { Header = "Nome", ValueSelector = p => p.Name, MinWidth = 10 },
            new() { Header = "Id", ValueSelector = p => p.Id, MinWidth = 10 },
            new() { Header = "Instalado", ValueSelector = p => p.VersionString, MinWidth = 10 },
            new() { Header = "Disponivel", ValueSelector = p => p.AvailableVersionString, MinWidth = 10 },
            new()
            {
                Header = "Actualizado?",
                ValueSelector = p => p.Version < p.AvailableVersion
                    ? new Markup("[yellow]:check_mark:[/]").ToString()
                    : new Markup("[green]:check_mark:[/]").ToString(),
                FixedWidth = 13
            }
        };

        var packages = new List<T> { package };
        var widths = new List<int>();

        // Calculate widths for all columns
        foreach (var column in columns)
        {
            if (column.FixedWidth.HasValue)
            {
                widths.Add(column.FixedWidth.Value);
            }
            else
            {
                var maxWidth = Math.Max(
                    column.Header.Length,
                    column.ValueSelector(package).Length
                ) + column.Padding;
                widths.Add(Math.Max(maxWidth, column.MinWidth + column.Padding));
            }
        }

        return MultiColumnSelector.FormatRow(package, columns, widths);
    }
}
