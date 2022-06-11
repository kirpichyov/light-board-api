using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace LightBoard.Api.Configurators;

public static class SerilogConfigurator
{
    public static void Apply(IConfigurationRoot configuration)
    {
        const string consoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}";
        const string fileOutputTemplate = "{Timestamp:G} {Message}{NewLine:1}{Exception:1}";
        const string logsPath = "Logs/log-.txt";

        if (!Enum.TryParse(configuration["Serilog:MinimumLevel"], out LogEventLevel minimalLevel))
        {
            throw new ArgumentException($"Serilog logging level value '{minimalLevel}' is invalid");
        }

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(minimalLevel)
            .MinimumLevel.Override("Microsoft", minimalLevel)
            .Enrich.FromLogContext()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .WriteTo.Console(theme: SystemConsoleTheme.Literate, outputTemplate: consoleOutputTemplate)
            .WriteTo.File(path: logsPath, outputTemplate: fileOutputTemplate, rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}