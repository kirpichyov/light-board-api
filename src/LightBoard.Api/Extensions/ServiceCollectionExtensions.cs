using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Mapping;
using LightBoard.Application.Services;
using LightBoard.DataAccess;
using LightBoard.DataAccess.Abstractions;
using LightBoard.DataAccess.Abstractions.Connection;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSqlContext(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddDbContext<PostgreSqlContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"), npgsql =>
                    {
                        npgsql.MigrationsAssembly("LightBoard.DataAccess.Migrations");
                    })
                    .UseSnakeCaseNamingConvention();

                if (environment.IsDevelopment())
                {
                    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
                    options.EnableSensitiveDataLogging();
                }
            }
        );
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }

    public static IServiceCollection AddRedisContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration["ConnectionStrings:RedisConnection"];

        services.AddSingleton<IRedisContext, RedisContext>(_ => new RedisContext(connectionString));
        services.AddScoped<IUserSessionsRepository, UserSessionsRepository>();
        
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHashingProvider, HashingProvider>();
        services.AddScoped<IKeysGenerator, KeysGenerator>();
        services.AddScoped<IApplicationMapper, ApplicationMapper>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IBlobService, BlobService>();
        services.AddScoped<IBoardsService, BoardsService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IColumnsService, ColumnsService>();
        
        return services;
    }

    public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomSettings<AuthOptions>(configuration);
        services.AddCustomSettings<BlobOptions>(configuration);
        
        return services;
    }

    public static TOptions BindFromAppSettings<TOptions>(
        this IConfiguration configuration,
        string? sectionName = null)
        where TOptions : new()
    {
        TOptions instance = new TOptions();

        sectionName ??= typeof(TOptions).Name;
        configuration.Bind(sectionName, instance);

        return instance;
    }
    
    public static void AddCustomSettings<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        string? sectionName = null)
        where TOptions : class
    {
        sectionName ??= typeof(TOptions).Name;
        services.AddOptions<TOptions>().Bind(configuration.GetSection(sectionName)).ValidateDataAnnotations();
    }
}