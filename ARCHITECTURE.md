# Console Tools Architecture

## Overview

The Console Tools solution has been refactored to follow Domain-Driven Design (DDD) principles with a layered architecture. This allows for better separation of concerns, reusability, and maintainability.

## Project Structure

```
console-tools/
├── src/
│   ├── ConsoleTools.Core/          # Domain layer
│   ├── ConsoleTools.Application/   # Application layer (future)
│   ├── ConsoleTools.ConsoleUI/     # UI components layer
│   ├── MotorDArranque/            # Application using the framework
│   └── TeleDroid/                  # Another application
```

### ConsoleTools.Core

**Purpose**: Domain models and core business logic

**Contents**:
- `Result` - Result type for operation outcomes
- `PackageInfo` - Domain model for package information
- Other domain entities and value objects

**Dependencies**: None (pure domain layer)

### ConsoleTools.Application

**Purpose**: Application services and use cases (to be implemented)

**Dependencies**:
- ConsoleTools.Core

### ConsoleTools.ConsoleUI

**Purpose**: Reusable console UI components built on Spectre.Console

**Key Components**:

#### 1. MultiColumnSelector

A generic, reusable component for creating multi-column selection prompts.

**Features**:
- Configurable columns with headers
- Automatic column width calculation
- Custom formatters for each column
- Pagination support
- Customizable colors and styles

**Usage Example**:
```csharp
var config = new MultiColumnSelectorConfig<MyDataType>
{
    Columns = new List<ColumnConfig<MyDataType>>
    {
        new() { Header = "Name", ValueSelector = item => item.Name, MinWidth = 10 },
        new() { Header = "Value", ValueSelector = item => item.Value.ToString(), MinWidth = 5 },
        new() { Header = "Status", ValueSelector = item => item.Status, FixedWidth = 15 }
    },
    DisplayFormatter = item => FormatMyItem(item),
    PageSize = 25,
    HighlightColor = Color.Violet
};

var selected = MultiColumnSelector.Show(myItems, config);
```

#### 2. PackageSelector

A pre-configured selector specifically for package management scenarios.

**Features**:
- Standard package columns (Name, Id, Installed Version, Available Version, Update Status)
- Color-coded update indicators
- Works with any package type implementing `IPackageInfo`

**Usage Example**:
```csharp
// Create wrapper for your package type
var packages = wingetPackages
    .Select(p => new PackageInfoWrapper(p))
    .ToList();

// Use the selector
var selectedPackages = PackageSelector.SelectPackagesAsStrings(packages);
```

#### IPackageInfo Interface

To use PackageSelector with your package type, implement the `IPackageInfo` interface:

```csharp
public interface IPackageInfo
{
    string Name { get; }
    string Id { get; }
    string VersionString { get; }
    string AvailableVersionString { get; }
    Version Version { get; }
    Version AvailableVersion { get; }
}
```

**Dependencies**:
- ConsoleTools.Core
- Spectre.Console

## Migration Example: ListagemProgramas

### Before (Monolithic)
```csharp
public async Task<Resultado> ListagemProgramas()
{
    var packages = await _packMgr.GetInstalledPackagesAsync();

    // Manual column width calculation
    int nameWidth = packages.Max(p => p.Name.Length) + 3;
    int idWidth = packages.Max(p => p.Id.Length) + 3;
    // ... more width calculations

    // Manual header construction
    string headers = string.Concat(
        "[underline turquoise2]",
        new string(' ', 8),
        "Nome".PadRight(nameWidth),
        // ... more header building
    );

    // Manual choice formatting
    var choices = packages.Select(p => string.Concat(
        new string(' ', 2),
        p.Name.PadRight(nameWidth),
        // ... more formatting
    ));

    var selected = AnsiConsole.Prompt(
        new MultiSelectionPrompt<string>()
            .Title(headers)
            .AddChoices(choices)
    );

    return Resultado.Ok();
}
```

### After (DDD with Reusable Components)
```csharp
public async Task<Resultado> ListagemProgramas()
{
    var packages = await _packMgr.GetInstalledPackagesAsync();
    var wrapped = packages
        .Select(p => new PackageInfoWrapper(p))
        .ToList();

    var selected = PackageSelector.SelectPackagesAsStrings(wrapped);

    return Resultado.Ok();
}
```

**Benefits**:
- ~50 lines reduced to ~10 lines
- Logic encapsulated in reusable component
- Easy to maintain and test
- Consistent UI across applications

## Adding New Applications

To create a new application in the Console Tools solution:

1. Create your project: `dotnet new console -n YourApp -o src/YourApp`
2. Add references:
   ```bash
   dotnet add src/YourApp reference src/ConsoleTools.Core
   dotnet add src/YourApp reference src/ConsoleTools.ConsoleUI
   ```
3. Use the reusable components in your application

## Best Practices

1. **Domain Layer (Core)**: Keep it pure, no external dependencies
2. **UI Components**: Make them generic and reusable
3. **Applications**: Consume the framework, don't modify it
4. **Interfaces**: Use interfaces to decouple from specific implementations
5. **Wrappers**: Use adapter pattern (e.g., `PackageInfoWrapper`) when working with external libraries

## Future Enhancements

- **ConsoleTools.Application**: Add application services for common operations
- **More UI Components**: Progress bars, input forms, tables, etc.
- **Testing**: Unit tests for Core and UI components
- **Documentation**: XML comments and API documentation
