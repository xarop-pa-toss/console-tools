using Microsoft.Extensions.Logging;

namespace ConsoleTools.Framework.Logging;

internal sealed class FileLogger(string category, FileLoggerProvider provider) : ILogger
{
    private readonly string _category = category;
    private readonly FileLoggerProvider _provider = provider;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _provider.MinimumLevel;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter(state, exception);
        if (string.IsNullOrWhiteSpace(message) && exception is null)
        {
            return;
        }

        var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{logLevel}] [{_category}] {message}";
        if (exception is not null)
        {
            line = line + Environment.NewLine + exception;
        }

        _provider.WriteLine(line);
    }
}

public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly string _logDirectory;
    private readonly string _filePrefix;
    private readonly object _sync = new();
    private StreamWriter? _writer;
    private DateTime _currentDate = DateTime.MinValue;
    private bool _disposed;

    public LogLevel MinimumLevel { get; }

    public FileLoggerProvider(
        string logDirectory = "logs",
        string filePrefix = "console-tools",
        LogLevel minimumLevel = LogLevel.Information)
    {
        _logDirectory = logDirectory;
        _filePrefix = filePrefix;
        MinimumLevel = minimumLevel;

        Directory.CreateDirectory(_logDirectory);
        OpenWriter(DateTime.Now.Date);
    }

    public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName, this);

    internal void WriteLine(string line)
    {
        lock (_sync)
        {
            if (_disposed)
            {
                return;
            }

            var today = DateTime.Now.Date;
            if (today != _currentDate)
            {
                OpenWriter(today);
            }

            _writer?.WriteLine(line);
            _writer?.Flush();
        }
    }

    private void OpenWriter(DateTime date)
    {
        _writer?.Dispose();

        _currentDate = date;
        var filePath = Path.Combine(_logDirectory, $"{_filePrefix}-{_currentDate:yyyy-MM-dd}.log");
        _writer = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read));
    }

    public void Dispose()
    {
        lock (_sync)
        {
            _disposed = true;
            _writer?.Dispose();
            _writer = null;
        }
    }
}
