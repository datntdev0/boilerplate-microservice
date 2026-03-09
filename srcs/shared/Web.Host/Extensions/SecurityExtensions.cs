using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Shared.Web.Host.Extensions;

public static class SecurityExtensions
{
    public static IServiceCollection AddDefaultSecurity(this IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddCorsPolicy(configs);
        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfigurationRoot configs)
    {
        var allowedOrigins = configs.GetSection("AllowedOrigins").Get<string>()?.Split(';') ?? [];
        if (allowedOrigins.Length == 0) return services;

        return services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }
}