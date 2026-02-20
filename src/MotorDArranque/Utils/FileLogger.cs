using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace ConsoleTools.Utils;

internal sealed class FileLogger : ILogger
{
    private readonly string _category;
    private readonly FileLoggerProvider _provider;

    public FileLogger(string category, FileLoggerProvider provider)
    {
        _category = category;
        _provider = provider;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _provider.MinimumLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;
        if (formatter == null) return;

        string message = formatter(state, exception);
        if (string.IsNullOrEmpty(message) && exception == null) return;

        var logRecord = new System.Text.StringBuilder();
        logRecord.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        logRecord.Append(' ');
        logRecord.Append('[').Append(logLevel.ToString()).Append(']').Append(' ');
        logRecord.Append('[').Append(_category).Append(']').Append(' ');
        logRecord.Append(message);
        if (exception != null)
        {
            logRecord.AppendLine();
            logRecord.Append(exception.ToString());
        }

        _provider.WriteLine(logRecord.ToString());
    }
}

public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly string _logDirectory;
    private readonly string _filePrefix;
    private readonly object _lock = new();
    private StreamWriter? _writer;
    private DateTime _currentDate = DateTime.MinValue;
    private bool _disposed;

    public LogLevel MinimumLevel { get; }

    public FileLoggerProvider(string logDirectory = "logs", string filePrefix = "log", LogLevel minimumLevel = LogLevel.Information)
    {
        _logDirectory = logDirectory ?? "logs";
        _filePrefix = filePrefix ?? "log";
        MinimumLevel = minimumLevel;
        Directory.CreateDirectory(_logDirectory);
        OpenWriterFor(DateTime.Now);
    }

    public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName, this);

    internal void WriteLine(string line)
    {
        try
        {
            lock (_lock)
            {
                if (_disposed) return;
                var now = DateTime.Now.Date;
                if (now != _currentDate)
                {
                    OpenWriterFor(now);
                }

                _writer!.WriteLine(line);
                _writer.Flush();
            }
        }
        catch
        {
            // Swallow exceptions to avoid crashing the host when logging fails
        }
    }

    private void OpenWriterFor(DateTime date)
    {
        _writer?.Dispose();
        _currentDate = date;
        string fileName = $"{_filePrefix}-{_currentDate:yyyy-MM-dd}.log";
        string fullPath = Path.Combine(_logDirectory, fileName);
        _writer = new StreamWriter(new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.Read)) { AutoFlush = true };
    }

    public void Dispose()
    {
        lock (_lock)
        {
            _disposed = true;
            _writer?.Dispose();
            _writer = null;
        }
    }
}

public static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string logDirectory = "logs", string filePrefix = "log", LogLevel minimumLevel = LogLevel.Information)
    {
        var provider = new FileLoggerProvider(logDirectory, filePrefix, minimumLevel);
        builder.AddProvider(provider);
        return builder;
    }

    public static void LogWithConsole(this ILogger logger, LogLevel level, string message, bool printToConsole = false, Exception? ex = null, params object[] args)
    {
        if (ex != null)
            logger.Log(level, ex, message, args);
        else
            logger.Log(level, message, args);

        if (!printToConsole) return;

        try
        {
            if (level == LogLevel.Error || level == LogLevel.Critical)
                Mensagens.ErroPanel(message + (ex != null ? "\n" + ex.ToString() : ""));
            else if (level == LogLevel.Warning)
                Mensagens.AvisoPanel(message);
            else if (level == LogLevel.Information)
                Mensagens.SucessoPanel(message);
            else
                AnsiConsole.MarkupLine(message);
        }
        catch
        {
            // ignore
        }
    }
}
