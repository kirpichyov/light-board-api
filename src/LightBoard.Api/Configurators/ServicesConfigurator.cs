using FluentValidation.AspNetCore;
using LightBoard.Api.Extensions;
using LightBoard.Api.Middleware.Exceptions;
using LightBoard.Api.Middleware.Filters;
using LightBoard.Api.Middleware.SessionKey;
using LightBoard.Api.Swagger;
using LightBoard.Api.Validators;
using LightBoard.Shared.Api;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace LightBoard.Api.Configurators;

public static class ServicesConfigurator
{
    public static void Apply(
        IServiceCollection services,
        ConfigurationManager appConfiguration,
        IWebHostEnvironment environment)
    {
        services.AddHttpContextAccessor();
    
        services.AddPostgreSqlContext(appConfiguration, environment)
                .AddRedisContext(appConfiguration)
                .AddApplicationServices(appConfiguration)
                .AddApplicationOptions(appConfiguration)
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
                        Title = "LightBoard API"
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
}