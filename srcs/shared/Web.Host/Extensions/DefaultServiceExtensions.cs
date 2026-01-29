using datntdev.Microservice.Shared.Common;
using datntdev.Microservice.Shared.Web.Host.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Reflection;

namespace datntdev.Microservice.Shared.Web.Host.Extensions;

public static class DefaultServiceExtensions
{
    public static IServiceCollection AddDefaultServices(this IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddDefaultOpenTelemetry();
        services.AddDefaultServiceDiscovery(configs);
        services.AddDefaultHealthChecks();
        return services;
    }

    public static IServiceCollection AddDefaultOpenTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing.AddSource(Assembly.GetEntryAssembly()!.GetName().Name!)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            })
            .UseOtlpExporter();

        return services;
    }

    public static IServiceCollection AddDefaultHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<DefaultHealthCheck>("default", tags: ["health", "alive"]);
        return services;
    }

    public static IServiceCollection AddDefaultServiceDiscovery(this IServiceCollection services, IConfigurationRoot configs)
    {
        services.AddServiceDiscovery();

        services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();
            // Turn on service discovery by default
            http.AddServiceDiscovery();
        });

        var httpClientSection = configs.GetSection("HttpClients");
        httpClientSection.GetChildren().ToList().ForEach(kv =>
        {
            services.AddHttpClient(kv.Key, client =>
            {
                client.BaseAddress = new Uri(kv.Value!);
            });
        });

        return services;
    }

    public static WebApplication MapDefaultHealthChecks(this WebApplication app)
    {
        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        // All health checks must pass for app to be considered ready to accept traffic after starting

        // Only health checks tagged with the "health" tag must pass for app to be considered health
        app.MapHealthChecks(Constants.Endpoints.Health, new()
        {
            Predicate = r => r.Tags.Contains("health"),
        });
        // Only health checks tagged with the "alive" tag must pass for app to be considered alive
        app.MapHealthChecks(Constants.Endpoints.Aliveness, new()
        {
            Predicate = r => r.Tags.Contains("alive"),
        });

        return app;
    }
}