using datntdev.Microservice.Shared.Communication.Handlers;
using datntdev.Microservice.Shared.Communication.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservice.Shared.Communication.Extensions;

public static class HttpProxyServiceExtensions
{
    public static void AddHttpProxyService(this IServiceCollection services, IConfigurationRoot configs)
    {
        // Register AuthorizationHeaderHandler for propagating auth headers in inter-service communication
        services.AddTransient<AuthorizationHeaderHandler>();
        
        services.AddScoped(typeof(ISrvIdentityHttpClient), (sp) =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            return new SrvIdentityHttpClient(httpClientFactory.CreateClient("srv-identity"));
        });

        services.AddScoped(typeof(ISrvAdminHttpClient), (sp) =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            return new SrvAdminHttpClient(httpClientFactory.CreateClient("srv-admin"));
        });

        services.AddScoped(typeof(ISrvNotifyHttpClient), (sp) =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            return new SrvNotifyHttpClient(httpClientFactory.CreateClient("srv-notify"));
        });

        services.AddScoped(typeof(ISrvPaymentHttpClient), (sp) =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            return new SrvPaymentHttpClient(httpClientFactory.CreateClient("srv-payment"));
        });

        var httpClientSection = configs.GetSection("HttpClients");
        httpClientSection.GetChildren().ToList().ForEach(kv =>
        {
            services.AddHttpClient(kv.Key, client =>
            {
                client.BaseAddress = new Uri(kv.Value!);
            }).AddHttpMessageHandler<AuthorizationHeaderHandler>();
        });
    }
}
