using LightBoard.Api.Extensions;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Shared.Api;
using Serilog;

namespace LightBoard.Api.Configurators;

public static class MiddlewareConfigurator
{
    public static void Apply(WebApplication webApplication)
    {
        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseCors(corsPolicyBuilder =>
            {
                corsPolicyBuilder.AllowAnyOrigin();
                corsPolicyBuilder.AllowAnyHeader();
                corsPolicyBuilder.AllowAnyMethod();
                corsPolicyBuilder.WithExposedHeaders(ApiHeaders.SessionKey);
            });
        
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI();
        }
        else
        {
            webApplication.UseCors(corsPolicyBuilder =>
            {
                AuthOptions identityOptions = webApplication.Configuration.BindFromAppSettings<AuthOptions>();

                corsPolicyBuilder.AllowAnyHeader();
                corsPolicyBuilder.AllowAnyMethod();
                corsPolicyBuilder.WithOrigins(identityOptions.AllowedCorsList ?? Array.Empty<string>());
                corsPolicyBuilder.WithExposedHeaders(ApiHeaders.SessionKey);
            });   
        }

        webApplication.UseSerilogRequestLogging();
        webApplication.UseHttpsRedirection();

        webApplication.UseAuthentication();
        webApplication.UseAuthorization();

        webApplication.MapControllers();
    }
}