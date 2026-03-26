namespace ConsoleTools.Framework;

public interface IConsoleToolModule
{
    string Id { get; }
    string DisplayName { get; }
    string Description { get; }
    Task RunAsync(CancellationToken cancellationToken = default);
}
