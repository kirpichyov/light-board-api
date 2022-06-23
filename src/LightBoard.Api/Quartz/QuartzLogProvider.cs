using Quartz.Logging;
using Serilog;
using LogLevel = Quartz.Logging.LogLevel;

namespace LightBoard.Api.Quartz;

public class QuartzLogProvider : ILogProvider
{
    public Logger GetLogger(string name)
    {
        return (level, func, exception, parameters) =>
        {
            if (func is null)
            {
                return true;
            }
                
            string log = func();
                    
            switch (level)
            {
                case LogLevel.Debug:
                case LogLevel.Trace:
                    Log.Debug(log, parameters);
                    return true;
                case LogLevel.Info:
                    Log.Information(log, parameters);
                    break;
                case LogLevel.Warn:
                    Log.Warning(log, parameters);
                    break;
                case LogLevel.Error:
                    Log.Error(exception, log, parameters);
                    break;
                case LogLevel.Fatal:
                    Log.Fatal(exception, log, parameters);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            return true;
        };
    }

    public IDisposable OpenNestedContext(string message)
    {
        throw new NotImplementedException();
    }

    public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
    {
        throw new NotImplementedException();
    }
}