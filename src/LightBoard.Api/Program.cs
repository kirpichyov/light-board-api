using FluentValidation.AspNetCore;
using LightBoard.Api.Extensions;
using LightBoard.Api.Middleware.Exceptions;
using LightBoard.Api.Middleware.Filters;
using LightBoard.Api.Middleware.SessionKey;
using LightBoard.Api.Swagger;
using LightBoard.Api.Validators;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Shared.Api;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

// ----------- Application builder -----------
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider((_, options) =>
            {
                options.ValidateScopes = true;
                options.ValidateOnBuild = true;
            })
            .UseSerilog();

ConfigureSerilog();
ConfigureServices(builder.Services);
WebApplication app = builder.Build();
ConfigurePipeline(app);

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

// ----------- Configuration -----------
void ConfigureServices(IServiceCollection services)
{
    services.AddHttpContextAccessor();
    
    services.AddPostgreSqlContext(builder.Configuration, builder.Environment)
            .AddRedisContext(builder.Configuration)
            .AddApplicationServices()
            .AddApplicationOptions(builder.Configuration)
            .AddSessionKeyAuthorization();

    services.AddRouting(options => options.LowercaseUrls = true);
    
    services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(
                    new StringEnumConverter(new CamelCaseNamingStrategy())
                );
            })
            .AddFluentValidation(configuration =>
            {
                configuration.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>();
            })
            .AddMvcOptions(options =>
            {
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<FluentValidationFilter>();
            })
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);;

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGenNewtonsoftSupport()
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AssistEasy API"
                });
                            
                options.AddSecurityDefinition("SessionKey", new OpenApiSecurityScheme
                {
                    Name = ApiHeaders.SessionKey,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Basic",
                    In = ParameterLocation.Header,
                    Description = "Obtained unique SessionKey."
                });
                            
                options.OperationFilter<AuthOperationFilter>();
            });
    
}

void ConfigurePipeline(WebApplication webApplication)
{
    if (webApplication.Environment.IsDevelopment())
    {
        app.UseCors(policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin();
            policyBuilder.AllowAnyHeader();
            policyBuilder.AllowAnyMethod();
        });
        
        webApplication.UseSwagger();
        webApplication.UseSwaggerUI();
    }

    app.UseCors(corsPolicyBuilder =>
    {
        AuthOptions identityOptions = webApplication.Configuration.BindFromAppSettings<AuthOptions>();

        corsPolicyBuilder.AllowAnyHeader();
        corsPolicyBuilder.AllowAnyMethod();
        corsPolicyBuilder.WithOrigins(identityOptions.AllowedCorsList ?? Array.Empty<string>());
    });
    
    webApplication.UseSerilogRequestLogging();
    webApplication.UseHttpsRedirection();

    webApplication.UseAuthentication();
    webApplication.UseAuthorization();

    webApplication.MapControllers();
}

void ConfigureSerilog()
{
    const string consoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}";
    const string fileOutputTemplate = "{Timestamp:G} {Message}{NewLine:1}{Exception:1}";
    const string logsPath = "Logs/log-.txt";

    if (!Enum.TryParse(builder.Configuration["Serilog:MinimumLevel"], out LogEventLevel minimalLevel))
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