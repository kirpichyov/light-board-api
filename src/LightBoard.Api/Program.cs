using LightBoard.Api.Configurators;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider((_, options) =>
            {
                options.ValidateScopes = true;
                options.ValidateOnBuild = true;
            })
            .UseSerilog();

SerilogConfigurator.Apply(builder.Configuration);
ServicesConfigurator.Apply(builder.Services, builder.Configuration, builder.Environment);
WebApplication app = builder.Build();
MiddlewareConfigurator.Apply(app);

var startupLogger = Log.ForContext<Program>();

try
{
    startupLogger.Information("Application is starting up");
    app.Run();
}
catch (Exception exception)
{
    startupLogger.Fatal(exception, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}
