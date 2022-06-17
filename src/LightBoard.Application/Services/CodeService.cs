using LightBoard.Application.Abstractions.Factories;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Domain.Entities.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace LightBoard.Application.Services;

public class CodeService : ICodeService
{
    private readonly IKeysGenerator _keysGenerator;
    private readonly IServiceProvider _serviceProvider;

    public CodeService(IKeysGenerator keysGenerator, IServiceProvider serviceProvider)
    {
        _keysGenerator = keysGenerator;
        _serviceProvider = serviceProvider;
    }

    public TCode GenerateCode<TCode>(int symbolCount, string email, int minutesLifetime)
        where TCode : CodeBase
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var factory = serviceScope.ServiceProvider.GetRequiredService<IGeneratedCodeFactory<TCode>>();
        var expirationDate = DateTime.UtcNow.AddMinutes(minutesLifetime);
        return factory.Create(_keysGenerator.Generate(symbolCount), email, expirationDate);
    }
}