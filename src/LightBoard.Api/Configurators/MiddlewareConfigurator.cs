using LightBoard.Api.Extensions;
using LightBoard.Application.Abstractions.Options;
using Serilog;

namespace LightBoard.Api.Configurators;

public static class MiddlewareConfigurator
{
    public static void Apply(WebApplication webApplication)
    {
        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseCors(policyBuilder =>
            {
                policyBuilder.AllowAnyOrigin();
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();
            });
        
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI();
        }

        webApplication.UseCors(corsPolicyBuilder =>
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
}