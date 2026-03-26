# ConsoleTools Module Template

This project is a starter skeleton for building new modules that run inside ConsoleTools.

## What to change first

- `TemplateModule.Id`
- `TemplateModule.DisplayName`
- `TemplateModule.Description`
- Menu actions inside `RunAsync`
- Service registrations in `AddTemplateModule`

## Integration steps for a new module

1. Rename project/folder/namespace from `ConsoleTools.ModuleTemplate`.
2. Add your own services and dependencies.
3. In the host, register your module extension:

```csharp
services.AddYourModule();
```

4. Build solution and run `ConsoleTools`.

## Required contract

Modules must implement `IConsoleToolModule` from `ConsoleTools.Framework`.
