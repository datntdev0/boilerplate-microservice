using datntdev.Microservice.Shared.Application.Authorization;
using datntdev.Microservice.Shared.Common.Exceptions;
using datntdev.Microservice.Shared.Web.Host.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation;
using OpenIddict.Validation.AspNetCore;

namespace datntdev.Microservice.Shared.Web.Host.Extensions;

public static class SecurityExtensions
{
    public static IServiceCollection AddDefaultSecurity(this IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddCorsPolicy(configs);
        return services;
    }

    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IConfigurationRoot configs)
    {
        var authority = configs["OpenIddict:Authority"];
        ArgumentException.ThrowIfNullOrEmpty(authority);

        services.AddScoped<SessionAppProvider>();

        services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareHandler>();

        services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        services.AddOpenIddict().AddValidation(options =>
        {
            options.SetIssuer(authority);
            options.UseSystemNetHttp();
            options.UseAspNetCore();
            options.AddEventHandler<OpenIddictValidationEvents.ProcessChallengeContext>(
                builder => builder.UseInlineHandler(context => throw ExceptionUnauthorized.Default()));
        });

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