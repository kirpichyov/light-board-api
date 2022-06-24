using LightBoard.Api.Quartz;
using LightBoard.Application.Abstractions.Factories;
using LightBoard.Application.Abstractions.Mapping;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Application.Factories;
using LightBoard.Application.Jobs;
using LightBoard.Application.Mapping;
using LightBoard.Application.Services;
using LightBoard.DataAccess;
using LightBoard.DataAccess.Abstractions;
using LightBoard.DataAccess.Abstractions.Connection;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.DataAccess.Repositories;
using LightBoard.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Logging;

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

                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                
                if (environment.IsDevelopment())
                {
                    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
                    options.EnableSensitiveDataLogging();
                }
            }
        );
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IGeneratedCodesRepository, GeneratedCodesRepository>();
        services.AddScoped<IUserNotificationsRepository, UserNotificationsRepository>();

        return services;
    }

    public static IServiceCollection AddRedisContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration["ConnectionStrings:RedisConnection"];

        services.AddSingleton<IRedisContext, RedisContext>(_ => new RedisContext(connectionString));

        bool.TryParse(configuration["UseInMemoryInsteadRedis"], out var useInMemory);
        if (useInMemory)
        {
            services.AddSingleton<IUserSessionsCache, InMemoryUserSessionsCache>();
        }
        else
        {
            services.AddScoped<IUserSessionsCache, UserSessionsRedisCache>();
        }

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHashingProvider, HashingProvider>();
        services.AddScoped<IKeysGenerator, KeysGenerator>();
        services.AddScoped<IHistoryRecordService, HistoryRecordService>();
        services.AddScoped<IApplicationMapper, ApplicationMapper>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICodeService, CodeService>();
        services.AddScoped<IGeneratedCodeFactory<ResetPasswordCode>, ResetPasswordCodeFactory>();
        services.AddScoped<IGeneratedCodeFactory<ConfirmEmailCode>, ConfirmEmailCodeFactory>();
        services.AddSingleton<IBlobService, BlobService>();
        services.AddScoped<IBoardsService, BoardsService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IMailingTemplateService, MailingTemplateService>();
        services.AddScoped<IColumnsService, ColumnsService>();
        services.AddScoped<ICardsService, CardsService>();
        services.AddScoped<ICardAttachmentService, CardAttachmentService>();
        services.AddScoped<IUserAvatarService, UserAvatarService>();
        services.AddScoped<ICardCommentsService, CardCommentsService>();
        services.AddScoped<IUserSessionsService, UserSessionsService>();
        
        return services;
    }

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(quartz =>
        {
            LogProvider.SetCurrentLogProvider(new QuartzLogProvider());
            quartz.SchedulerId = "Scheduler-Core";

            quartz.UseMicrosoftDependencyInjectionJobFactory();
            quartz.UseSimpleTypeLoader();
            quartz.UseInMemoryStore();
            quartz.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = 5;
            });
                
            quartz.ScheduleJob<UserNotificationsJob>(trigger =>
                {
                    string cronExpression = configuration["BackgroundJobs:UserNotificationsCronExpression"];

                    trigger.WithIdentity("UserNotifications")
                           .StartNow()
                           .WithCronSchedule(CronScheduleBuilder.CronSchedule(cronExpression));
                }
            );
        });
            
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
    
    public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomSettings<AuthOptions>(configuration);
        services.AddCustomSettings<BlobOptions>(configuration);
        services.AddCustomSettings<EmailTemplatesOptions>(configuration);
        services.AddCustomSettings<MailingOptions>(configuration);
        
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