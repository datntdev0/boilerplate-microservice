using datntdev.Microservice.Shared.Communication.Handlers;
using datntdev.Microservice.Shared.Communication.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Shared.Communication.Extensions;

public static class HttpProxyServiceTypes
{
    public const string TypeDefault = "";
    public const string TypeApiKey = "apikey";

    public static readonly Dictionary<string, (Type, Func<IServiceProvider, object?, object>)> SrvClientTypes = new()
    {
        { "srv-identity", (typeof(ISrvIdentityHttpClient), (sp, key) =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                return new SrvIdentityHttpClient(httpClientFactory.CreateClient($"srv-identity-{key ?? TypeDefault}"));
            })
        },
        { "srv-admin", (typeof(ISrvAdminHttpClient), (sp, key) =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                return new SrvAdminHttpClient(httpClientFactory.CreateClient($"srv-admin-{key ?? TypeDefault}"));
            }) 
        },
        { "srv-notify", (typeof(ISrvNotifyHttpClient), (sp, key) =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                return new SrvNotifyHttpClient(httpClientFactory.CreateClient($"srv-notify-{key ?? TypeDefault}"));
            }) 
        },
        { "srv-payment", (typeof(ISrvPaymentHttpClient), (sp, key) =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                return new SrvPaymentHttpClient(httpClientFactory.CreateClient($"srv-payment-{key ?? TypeDefault}"));
            })
        }
    };
}

public static class HttpProxyServiceExtensions
{
    public static T GetRequiredHttpProxyService<T>(this IServiceProvider provider, bool useApiKey = false)
    {
        var service = useApiKey
            ? provider.GetKeyedService<T>(HttpProxyServiceTypes.TypeApiKey)
            : provider.GetKeyedService<T>(HttpProxyServiceTypes.TypeDefault);
        return service ?? throw new InvalidOperationException($"No HTTP proxy service of type {typeof(T).FullName} with key {(useApiKey ? HttpProxyServiceTypes.TypeApiKey : HttpProxyServiceTypes.TypeDefault)} is registered.");
    }

    public static void AddHttpProxyService(this IServiceCollection services, IConfigurationRoot configs)
    {
        AddHttpClient(services, configs); // Register HttpClients with different configurations (default and API key)

        var httpClientSection = configs.GetSection("HttpClients");
        httpClientSection.GetChildren().Where(kv => kv.Key.StartsWith("srv-")).ToList().ForEach(kv =>
        {
            var (type, factory) = HttpProxyServiceTypes.SrvClientTypes[kv.Key];
            services.AddKeyedScoped(type, HttpProxyServiceTypes.TypeDefault, implementationFactory: factory);
            services.AddKeyedScoped(type, HttpProxyServiceTypes.TypeApiKey, implementationFactory: factory);
        });
    }

    private static void AddHttpClient(this IServiceCollection services, IConfigurationRoot configs)
    {
        // Register AuthorizationHeaderHandler for propagating auth headers in inter-service communication
        services.AddTransient<AuthorizationHeaderHandler>();
        
        // Register named HttpClients with HttpClientFactory
        var apiKey = configs.GetValue<string>("HttpClients:ApiKey");
        var httpClientSection = configs.GetSection("HttpClients");
        httpClientSection.GetChildren().Where(kv => kv.Key.StartsWith("srv-")).ToList().ForEach(kv =>
        {
            services.AddHttpClient($"{kv.Key}-{HttpProxyServiceTypes.TypeDefault}", client =>
            {
                client.BaseAddress = new Uri(kv.Value!);
            }).AddHttpMessageHandler<AuthorizationHeaderHandler>();

            services.AddHttpClient($"{kv.Key}-{HttpProxyServiceTypes.TypeApiKey}", client =>
            {
                client.BaseAddress = new Uri(kv.Value!);
                client.DefaultRequestHeaders.Add("Authorization", $"ApiKey {apiKey}");
            });
        });
    }
}

