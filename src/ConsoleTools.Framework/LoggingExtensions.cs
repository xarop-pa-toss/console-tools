using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ConsoleTools.Framework.Logging;

public static class LoggingExtensions
{
    public static IServiceCollection AddConsoleToolsLogging(this IServiceCollection services, string logsDirectory)
    {
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddSimpleConsole(options =>
            {
                options.SingleLine = true;
                options.TimestampFormat = "HH:mm:ss ";
                options.ColorBehavior = LoggerColorBehavior.Enabled;
            });
            builder.AddProvider(new FileLoggerProvider(logsDirectory, "console-tools", LogLevel.Information));
        });

        return services;
    }
}
